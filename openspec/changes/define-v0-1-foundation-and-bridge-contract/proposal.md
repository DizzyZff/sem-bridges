## Why

Sem Bridges needs a clear public foundation before implementation begins so contributors understand the product, the first v0.1 target, and the boundary between the core system and Roslyn as the first bridge. This is timely because the repository is still minimal, so foundational choices can be made explicitly instead of emerging accidentally from the first code package.

## What Changes

- Define Sem Bridges as a stack-agnostic semantic code intelligence project for AI coding agents.
- Establish Roslyn/C# as the first analyzer bridge while keeping the product contract language-neutral.
- Document the v0.1 scope, non-goals, package layout, and contributor-facing repository structure.
- Define the initial shared output envelope and schema kinds needed by analyzer bridges and the MCP server.
- Capture the bridge abstraction so future TypeScript, Java, Python, or Rust bridges can fit without changing MCP tool names.
- Prepare the repository for implementation of the Roslyn CLI foundation and later MCP server work.

## Capabilities

### New Capabilities

- `project-foundation`: Defines the v0.1 product direction, public README expectations, repository layout, and non-goals.
- `bridge-contract`: Defines the shared JSON envelope, schema kinds, versioning expectations, and language-neutral contract boundaries for analyzer bridge output.
- `bridge-architecture`: Defines the analyzer bridge abstraction, capability discovery model, routing expectations, and future bridge package strategy.

### Modified Capabilities

- None.

## Impact

- Affects documentation, repository layout decisions, schema artifacts, and OpenSpec project context.
- Does not implement analyzer, MCP server, plugin, or runtime behavior yet.
- Unblocks follow-up implementation work for the Roslyn analyzer CLI, schema examples, fixture projects, MCP routing, and CI.
