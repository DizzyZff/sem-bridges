# Sem Bridges

Sem Bridges exposes compiler and static-analysis facts to AI coding agents through the Model Context Protocol (MCP). Instead of asking agents to infer architecture from file search alone, Sem Bridges gives them structured semantic context: project snapshots, diagnostics, symbol search, references, public APIs, type graphs, and bridge capabilities.

The project is stack-agnostic by design. Roslyn/C# is the first analyzer bridge for v0.1, not the whole product. The long-term shape is a shared bridge contract that can support additional analyzer backends for TypeScript, Java, Python, Rust, and other ecosystems once the Roslyn bridge proves the contract.

## v0.1 Focus

v0.1 should prove that an AI coding agent can understand a C#/.NET repository better with compiler-backed facts than with raw text search alone.

Initial work focuses on:

- A language-neutral Sem Bridges output contract.
- A Roslyn analyzer bridge that emits schema-aligned JSON.
- An MCP server that exposes semantic tools to agents.
- Fixture projects and examples that demonstrate diagnostics, snapshots, and symbol search.

## How It Fits Together

```text
AI coding agent
      |
      v
MCP server
      |
      v
Analyzer bridge, such as Roslyn
      |
      v
Compiler/static-analysis facts
```

MCP is the agent boundary. Analyzer bridges are responsible for reading code with the best available language tooling and returning compact, schema-aligned facts.

## Repository Layout

The intended layout is:

- `packages/analyzer-roslyn` - .NET/Roslyn analyzer bridge.
- `packages/mcp-server` - TypeScript MCP server and bridge routing layer.
- `schemas` - shared JSON schema and examples.
- `fixtures` - small test projects used by analyzer and MCP tests.
- `docs` - architecture, bridge contract, development, and usage documentation.
- `plugins/claude-code` - optional Claude Code packaging after the core MCP server exists.
- `openspec` - change proposals, specs, designs, and task plans.

Some directories may not exist until their first implementation slice lands.

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

Machine-facing commands write one Sem Bridges JSON envelope to stdout. Help and version commands are human-readable:

```bash
dotnet run --project packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj -- --help
dotnet run --project packages/analyzer-roslyn/SemBridges.Analyzer.Roslyn.csproj -- --version
```

## v0.1 Non-Goals

- No automatic code modification.
- No full IDE replacement.
- No custom C# parser outside Roslyn.
- No TypeScript, Java, Python, or Rust bridge implementation until the Roslyn bridge proves the contract.
- No Claude-only architecture; Claude Code packaging must stay separate from the core MCP server.

## Current Status

This repository is in the foundation phase. The active design direction is tracked through OpenSpec changes under `openspec/changes`.
