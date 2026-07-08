# sem-bridges

Stack-agnostic semantic code intelligence for AI coding agents. Roslyn/C# is the
first analyzer bridge.

## Roslyn Analyzer

The initial Roslyn bridge lives in `packages/analyzer-roslyn`.

Build it with:

```bash
dotnet build packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj
```

Show bridge capabilities:

```bash
dotnet run --project packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj -- capabilities
```

Smoke-test loading a C# project or solution:

```bash
dotnet run --project packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj -- load fixtures/dotnet/SemBridges.Sample
```

Machine-facing commands write one Sem Bridges JSON envelope to stdout. Help and
version commands are human-readable:

```bash
dotnet run --project packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj -- --help
dotnet run --project packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj -- --version
```
