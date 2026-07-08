## ADDED Requirements

### Requirement: Analyzer bridge abstraction
The architecture SHALL define analyzer bridges as separate implementations that accept semantic commands and return schema-aligned JSON outputs.

#### Scenario: New bridge type is considered
- **WHEN** contributors evaluate a future TypeScript, Java, Python, or Rust bridge
- **THEN** they can compare it against the documented analyzer bridge abstraction

### Requirement: Capability discovery handshake
Each analyzer bridge SHALL expose a capabilities command that reports bridge name, schema version, supported commands, limitations, and relevant command metadata.

#### Scenario: MCP server starts with configured bridge
- **WHEN** the MCP server initializes a configured analyzer bridge
- **THEN** it can call the capabilities command to discover supported behavior before invoking semantic tools

### Requirement: Stable semantic MCP tool names
The agent-facing MCP tools SHALL use semantic task names rather than bridge-specific names, with bridge selection handled through configuration or parameters.

#### Scenario: Roslyn is the default v0.1 bridge
- **WHEN** an agent calls a semantic MCP tool without specifying a bridge
- **THEN** the system can route to Roslyn by default without naming the tool after Roslyn

### Requirement: Future bridge package strategy
The architecture SHALL document expected package naming and placement for future analyzer bridges without requiring those bridges in v0.1.

#### Scenario: Repository expands beyond Roslyn
- **WHEN** a later change adds another analyzer bridge
- **THEN** contributors can place and name the package consistently with the established strategy

### Requirement: Required and optional commands
The architecture SHALL distinguish commands expected for v0.1 usefulness from optional commands that bridges may omit while reporting limitations through capabilities.

#### Scenario: Bridge lacks an advanced command
- **WHEN** an analyzer bridge does not support a semantic command such as `typeGraph`
- **THEN** capability discovery reports the limitation so the MCP server can fail gracefully
