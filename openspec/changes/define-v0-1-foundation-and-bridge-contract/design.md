## Context

Sem Bridges currently has a minimal repository with a README, MIT license, OpenSpec configuration, and contributor guidance. The GitHub issue ladder already points toward a stack-agnostic semantic code intelligence system: analyzer bridges produce compiler/static-analysis facts, an MCP server exposes those facts to coding agents, and a Claude Code plugin can later package the experience.

The central design pressure is avoiding accidental C#-only or Claude-only architecture while still shipping v0.1 through a Roslyn bridge first.

## Goals / Non-Goals

**Goals:**

- Make the README and repository layout explain Sem Bridges within one minute.
- Define a language-neutral bridge output envelope that Roslyn can implement first.
- Define bridge capability discovery so the MCP server can route commands without hard-coding all future bridge behavior.
- Keep v0.1 focused on understanding code, not editing code.
- Preserve room for future analyzer bridges without implementing them now.

**Non-Goals:**

- No analyzer implementation in this change.
- No MCP server implementation in this change.
- No Claude Code plugin implementation in this change.
- No custom C# parser; Roslyn remains the source of truth for C#.
- No TypeScript, Java, Python, or Rust bridge implementation until the Roslyn bridge proves the contract.

## Decisions

### Use a package-oriented monorepo layout

Adopt these top-level directories:

- `packages/analyzer-roslyn` for the first .NET analyzer bridge.
- `packages/mcp-server` for the agent-facing MCP server.
- `schemas` for shared JSON schemas and schema examples.
- `fixtures` for small test repositories.
- `docs` for architecture, bridge contract, development, and usage documentation.
- `plugins/claude-code` for Claude Code packaging only after core MCP behavior exists.

Alternative considered: a Roslyn-first repository with `src/` and `tests/`. That would be simpler at first, but it would make the project look like a C# analyzer rather than a bridge system.

### Define one shared output envelope

All bridge commands should emit a compact JSON envelope:

- `schemaVersion`
- `bridge`
- `kind`
- `generatedAt`
- `data`
- optional `metadata`
- optional `bridgeSpecific`

Common fields stay language-neutral. Bridge-specific terms belong under `bridgeSpecific` or nested metadata. This lets Roslyn expose useful details without forcing future bridges to mimic Roslyn concepts.

Alternative considered: separate schemas per command with no shared envelope. That would reduce ceremony per file, but it would complicate MCP routing, validation, and agent prompting.

### Make capabilities the bridge handshake

Each bridge should support a `capabilities` command that reports the bridge name, schema version, supported commands, limitations, and optional command metadata. The MCP server should use this handshake to decide which tools can run successfully.

Alternative considered: the MCP server owns a static list of bridge features. That is easier for v0.1 but brittle once optional or third-party bridges exist.

### Keep MCP tool names stable across bridges

MCP tools should express semantic tasks, such as `sem_snapshot`, `sem_diagnostics`, and `sem_symbol_search`, while accepting a bridge selector that defaults to `roslyn` for v0.1. Tool names should not include language names unless a future capability is truly language-specific.

Alternative considered: `roslyn_snapshot`, `roslyn_diagnostics`, and similar names. That would be clear for v0.1, but it would leak the first bridge into the public agent interface.

## Risks / Trade-offs

- Language-neutral schema becomes too generic to be useful -> Mitigation: allow `bridgeSpecific` details while keeping top-level fields stable.
- Foundation work delays visible demos -> Mitigation: keep this change small and immediately follow with the Roslyn CLI foundation.
- Future bridge design overfits imagined languages -> Mitigation: document future bridge expectations, but implement only Roslyn in v0.1.
- Repository layout creates empty directories too early -> Mitigation: add only documented layout or lightweight placeholders where needed by tooling.
