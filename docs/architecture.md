# Architecture

Sem Bridges is organized around one boundary: analyzer bridges produce semantic facts, and the MCP server exposes those facts to AI coding agents.

```text
Agent
  |
  v
MCP server
  |
  v
Analyzer bridge
  |
  v
Language compiler or static-analysis API
```

## Layers

### Analyzer Bridges

Analyzer bridges are separate implementations that understand a language ecosystem and return Sem Bridges JSON. The first bridge is Roslyn for C#/.NET because Roslyn and MSBuildWorkspace can load projects, inspect symbols, and report diagnostics using the same compiler model as normal .NET builds.

Future bridges should live beside the Roslyn bridge rather than inside it. Expected package names:

- `packages/analyzer-roslyn`
- `packages/analyzer-typescript`
- `packages/analyzer-java`
- `packages/analyzer-python`
- `packages/analyzer-rust`

Future bridge names may change when implementation details are known, but they should keep the `packages/analyzer-*` pattern.

### MCP Server

The MCP server is the agent-facing boundary. It should expose stable semantic tools and route requests to configured analyzer bridges. The server should not require agents to know Roslyn-specific command names for common semantic tasks.

Initial semantic tool names should stay bridge-neutral:

- `sem_capabilities`
- `sem_snapshot`
- `sem_diagnostics`
- `sem_symbol_search`
- `sem_references`
- `sem_public_api`
- `sem_type_graph`

For v0.1, the default bridge can be `roslyn`. Later bridges should use the same MCP tool names where they support the same semantic task.

### Plugin Packaging

Claude Code plugin packaging belongs under `plugins/claude-code` after the core MCP server exists. Plugin instructions can teach Claude Code when to call semantic tools, but the plugin must not become the only supported integration path.

## Capability Discovery

Every bridge should expose a `capabilities` command. The MCP server calls it before relying on bridge behavior.

A capabilities payload should report:

- bridge name
- schema version
- supported commands
- unsupported or partial commands
- known limitations
- optional command metadata, such as filters or input requirements

This handshake lets the MCP server fail gracefully when a bridge lacks a command such as `typeGraph`.

## Required and Optional Commands

The minimum useful v0.1 Roslyn bridge should support:

- `capabilities`
- `solutionSnapshot`
- `diagnostics`

Commands that make the system more valuable, but can arrive after the foundation, include:

- `symbolSearch`
- `references`
- `publicApi`
- `typeGraph`

Bridges that omit an advanced command must report that limitation through capabilities.

## Contract Boundary

The shared schema must stay language-neutral at the envelope level. Bridge-specific implementation details, such as Roslyn workspace type or compiler diagnostic category, belong under `bridgeSpecific` or nested metadata rather than top-level fields.
