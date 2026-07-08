## ADDED Requirements

### Requirement: Shared output envelope
Analyzer bridge commands SHALL emit outputs using a shared JSON envelope with `schemaVersion`, `bridge`, `kind`, `generatedAt`, and `data` fields.

#### Scenario: Bridge command returns successful output
- **WHEN** an analyzer bridge command completes successfully
- **THEN** the output includes the shared envelope fields and places command-specific content under `data`

### Requirement: Language-neutral top-level contract
The shared contract SHALL keep top-level fields language-neutral and place Roslyn-specific or future bridge-specific details under `bridgeSpecific` or nested metadata.

#### Scenario: Roslyn output includes compiler-specific details
- **WHEN** Roslyn emits data that has no language-neutral equivalent
- **THEN** those details appear outside the common top-level contract

### Requirement: Initial schema kinds
The schema SHALL define initial output kinds for `solutionSnapshot`, `diagnostics`, `symbolSearch`, `references`, `publicApi`, `typeGraph`, and `capabilities`.

#### Scenario: MCP server validates bridge output
- **WHEN** the MCP server receives bridge output for a supported semantic command
- **THEN** the output kind matches one of the documented schema kinds

### Requirement: Schema versioning strategy
The bridge contract SHALL document how `schemaVersion` changes are handled and which fields are required versus optional.

#### Scenario: Contract evolves after v0.1
- **WHEN** a later change adds optional fields to the output envelope or data payloads
- **THEN** contributors can determine whether the change requires a schema version update

### Requirement: Schema examples
The repository SHALL include compact sample JSON for snapshot, diagnostics, and symbol search outputs.

#### Scenario: Agent prompt or test needs an example
- **WHEN** a contributor needs to understand the expected output shape
- **THEN** examples show realistic, compact payloads aligned with the shared envelope
