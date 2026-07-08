# Bridge Contract

Sem Bridges analyzer bridges return compact JSON facts through a shared envelope. The envelope gives the MCP server and agents a stable shape while allowing each bridge to include language-specific details where needed.

## Envelope

Every successful bridge command returns:

```json
{
  "schemaVersion": "0.1.0",
  "bridge": "roslyn",
  "kind": "solutionSnapshot",
  "generatedAt": "2026-07-01T18:00:00Z",
  "data": {}
}
```

Required fields:

- `schemaVersion` - Sem Bridges schema version used by the payload.
- `bridge` - analyzer bridge that produced the output, such as `roslyn`.
- `kind` - payload kind.
- `generatedAt` - UTC timestamp for when the bridge generated the output.
- `data` - command-specific payload.

Optional fields:

- `metadata` - language-neutral execution metadata, such as input path, elapsed time, or truncation flags.
- `bridgeSpecific` - details that are useful but not portable across bridges.

## Initial Kinds

The v0.1 contract defines these kinds:

- `solutionSnapshot` - project and document structure.
- `diagnostics` - compiler or analyzer diagnostics.
- `symbolSearch` - semantic symbol search results.
- `references` - references to a selected symbol.
- `publicApi` - public surface for a project or solution.
- `typeGraph` - inheritance, interface, and implementation relationships.
- `capabilities` - bridge-supported commands and limitations.

## Language-Neutral Boundary

Top-level envelope fields must not contain Roslyn-only concepts. If Roslyn emits details such as target framework monikers, compilation options, or diagnostic categories that may not map to other ecosystems, those details belong under `data`, `metadata`, or `bridgeSpecific` with clear names.

## Versioning

Sem Bridges uses semantic versioning for `schemaVersion`.

- Patch changes add clarifications or compatible examples.
- Minor changes add optional fields or new kinds while preserving existing behavior.
- Major changes remove or rename fields, alter required fields, or change payload meaning.

Required fields are stable within a major version. Optional fields may be absent and consumers must handle that gracefully.

## Examples

Examples live under `schemas/examples` and are intentionally small enough for agent prompts, tests, and documentation snippets.
