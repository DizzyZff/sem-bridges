using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

const string BridgeName = "roslyn";
const string SchemaVersion = "0.1";

JsonSerializerOptions JsonOptions = new()
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false
};

return await RunAsync(args);

async Task<int> RunAsync(string[] args)
{
    if (args.Length == 0 || IsHelp(args[0]))
    {
        PrintHelp();
        return 0;
    }

    if (IsVersion(args[0]))
    {
        Console.WriteLine(GetVersion());
        return 0;
    }

    return args[0] switch
    {
        "capabilities" => WriteJson(Envelope("capabilities", new CapabilitiesData(
            BridgeName,
            SchemaVersion,
            ["capabilities", "load"],
            [".sln", ".slnx", ".csproj", "directory"],
            [
                "snapshot, diagnostics, symbol-search, references, public-api, and type-graph are not implemented in this foundation release.",
                "Directory input resolution only inspects the immediate directory.",
                ".slnx loading depends on installed MSBuild and Roslyn workspace support."
            ]))),
        "load" => await RunLoadAsync(args),
        _ => WriteJson(ErrorEnvelope(
            "unknown_command",
            $"Unknown command '{args[0]}'.",
            new { command = args[0], supportedCommands = new[] { "capabilities", "load" } }), 2)
    };
}

async Task<int> RunLoadAsync(string[] args)
{
    if (args.Length != 2)
    {
        return WriteJson(ErrorEnvelope(
            "invalid_arguments",
            "The load command requires exactly one path argument.",
            new { usage = "sem-bridges-roslyn load <path>" }), 2);
    }

    var resolution = ResolveInput(args[1]);
    if (resolution.Error is not null)
    {
        return WriteJson(resolution.Error, 2);
    }

    var target = resolution.Target!;

    try
    {
        RegisterMsBuild();
    }
    catch (Exception ex)
    {
        return WriteJson(ErrorEnvelope(
            "workspace_setup_failed",
            "MSBuild could not be registered before creating the Roslyn workspace.",
            new { target.Path, target.Kind, error = ex.Message }), 2);
    }

    try
    {
        using var workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += (_, e) => Console.Error.WriteLine(e.Diagnostic.Message);

        if (target.Kind == "project")
        {
            var project = await workspace.OpenProjectAsync(target.Path);
            return WriteJson(Envelope("load", new LoadData(
                "project",
                target.Path,
                target.ResolvedFrom,
                project.Name,
                project.FilePath,
                1,
                project.DocumentIds.Count,
                workspace.Diagnostics.Select(d => d.Message).ToArray())));
        }

        var solution = await workspace.OpenSolutionAsync(target.Path);
        var projects = solution.Projects.ToArray();
        return WriteJson(Envelope("load", new LoadData(
            "solution",
            target.Path,
            target.ResolvedFrom,
            Path.GetFileNameWithoutExtension(target.Path),
            solution.FilePath,
            projects.Length,
            projects.Sum(project => project.DocumentIds.Count),
            workspace.Diagnostics.Select(d => d.Message).ToArray())));
    }
    catch (Exception ex)
    {
        return WriteJson(ErrorEnvelope(
            "load_failed",
            "Roslyn could not load the resolved project or solution.",
            new { target.Path, target.Kind, error = ex.Message }), 2);
    }
}

void RegisterMsBuild()
{
    if (MSBuildLocator.IsRegistered)
    {
        return;
    }

    MSBuildLocator.RegisterDefaults();
}

InputResolution ResolveInput(string input)
{
    var fullPath = Path.GetFullPath(input);

    if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
    {
        return InputResolution.FromError(ErrorEnvelope(
            "invalid_path",
            "Path does not exist.",
            new { path = fullPath }));
    }

    if (File.Exists(fullPath))
    {
        var extension = Path.GetExtension(fullPath).ToLowerInvariant();
        return extension switch
        {
            ".csproj" => InputResolution.FromTarget(new LoadTarget("project", fullPath, "file")),
            ".sln" or ".slnx" => InputResolution.FromTarget(new LoadTarget("solution", fullPath, "file")),
            _ => InputResolution.FromError(ErrorEnvelope(
                "unsupported_input",
                "Path exists but is not a supported analyzer input.",
                new { path = fullPath, supportedInputKinds = new[] { ".sln", ".slnx", ".csproj", "directory" } }))
        };
    }

    var solutionCandidates = Directory
        .EnumerateFiles(fullPath, "*.*", SearchOption.TopDirectoryOnly)
        .Where(path => IsSolutionLike(path))
        .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    var projectCandidates = Directory
        .EnumerateFiles(fullPath, "*.csproj", SearchOption.TopDirectoryOnly)
        .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    var candidates = solutionCandidates.Concat(projectCandidates).ToArray();
    if (candidates.Length > 1)
    {
        return InputResolution.FromError(ErrorEnvelope(
            "ambiguous_directory",
            "Directory contains multiple plausible project or solution inputs. Pass an explicit path.",
            new { path = fullPath, candidates }));
    }

    if (solutionCandidates.Length == 1)
    {
        return InputResolution.FromTarget(new LoadTarget("solution", solutionCandidates[0], "directory"));
    }

    if (projectCandidates.Length == 1)
    {
        return InputResolution.FromTarget(new LoadTarget("project", projectCandidates[0], "directory"));
    }

    return InputResolution.FromError(ErrorEnvelope(
        "unsupported_input",
        "Directory does not contain a supported project or solution input in its immediate children.",
        new { path = fullPath, supportedInputKinds = new[] { ".sln", ".slnx", ".csproj", "directory" } }));
}

bool IsSolutionLike(string path)
{
    var extension = Path.GetExtension(path).ToLowerInvariant();
    return extension is ".sln" or ".slnx";
}

SemEnvelope Envelope(string kind, object data)
{
    return new SemEnvelope(SchemaVersion, BridgeName, kind, DateTimeOffset.UtcNow, data);
}

SemEnvelope ErrorEnvelope(string code, string message, object? metadata = null)
{
    return Envelope("error", new ErrorData(code, message, metadata));
}

int WriteJson(SemEnvelope envelope, int exitCode = 0)
{
    Console.WriteLine(JsonSerializer.Serialize(envelope, JsonOptions));
    return exitCode;
}

bool IsHelp(string value)
{
    return value is "-h" or "--help" or "help";
}

bool IsVersion(string value)
{
    return value is "--version" or "-v" or "version";
}

string GetVersion()
{
    return typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? typeof(Program).Assembly.GetName().Version?.ToString()
        ?? "0.1.0";
}

void PrintHelp()
{
    Console.WriteLine("""
    Sem Bridges Roslyn Analyzer

    Usage:
      sem-bridges-roslyn <command> [arguments]

    Commands:
      capabilities          Emit bridge capabilities as Sem Bridges JSON.
      load <path>           Resolve and load a .sln, .slnx, .csproj, or directory input.
      help                  Show this help text.
      version               Show the analyzer CLI version.

    Options:
      -h, --help            Show this help text.
      -v, --version         Show the analyzer CLI version.
    """);
}

sealed record SemEnvelope(
    string SchemaVersion,
    string Bridge,
    string Kind,
    DateTimeOffset GeneratedAt,
    object Data);

sealed record CapabilitiesData(
    string Bridge,
    string SchemaVersion,
    string[] SupportedCommands,
    string[] SupportedInputKinds,
    string[] Limitations);

sealed record ErrorData(
    string Code,
    string Message,
    object? Metadata);

sealed record LoadData(
    string InputKind,
    string Path,
    string ResolvedFrom,
    string Name,
    string? FilePath,
    int ProjectCount,
    int DocumentCount,
    string[] WorkspaceDiagnostics);

sealed record LoadTarget(
    string Kind,
    string Path,
    string ResolvedFrom);

sealed record InputResolution(LoadTarget? Target, SemEnvelope? Error)
{
    public static InputResolution FromTarget(LoadTarget target)
    {
        return new InputResolution(target, null);
    }

    public static InputResolution FromError(SemEnvelope error)
    {
        return new InputResolution(null, error);
    }
}
