## ADDED Requirements

### Requirement: Public product definition
The repository README SHALL define Sem Bridges as a stack-agnostic semantic code intelligence project for AI coding agents, with Roslyn/C# identified as the first bridge rather than the whole product.

#### Scenario: New visitor reads README
- **WHEN** a new visitor opens the README
- **THEN** they can identify the project purpose, first bridge, agent-facing MCP boundary, and v0.1 focus within one minute

### Requirement: Initial repository layout
The repository SHALL document an initial layout that separates analyzer bridges, MCP server code, schemas, fixtures, docs, and plugin packaging.

#### Scenario: Contributor chooses where new work belongs
- **WHEN** a contributor needs to add analyzer, MCP, schema, fixture, documentation, or plugin work
- **THEN** the documented layout identifies the expected top-level directory for that work

### Requirement: v0.1 non-goals
The repository SHALL document v0.1 non-goals, including no automatic code modification, no full IDE replacement, no custom C# parser outside Roslyn, and no additional language bridge implementations before the Roslyn bridge proves the contract.

#### Scenario: Scope question arises
- **WHEN** a proposed v0.1 task conflicts with a documented non-goal
- **THEN** contributors can defer that task without changing the core v0.1 plan

### Requirement: OpenSpec project context
The OpenSpec project configuration SHALL capture the project stack direction, architectural boundaries, and contribution conventions relevant to future change proposals.

#### Scenario: Future proposal is created
- **WHEN** OpenSpec instructions are requested for a future change
- **THEN** the project context guides artifacts toward the established Sem Bridges architecture
