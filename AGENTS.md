# Repository Guidelines

## Project Structure & Module Organization

This repository uses a package-oriented layout for a stack-agnostic semantic code intelligence system. Roslyn/C# is the first analyzer bridge, not the whole product.

When adding implementation code, keep the structure predictable:

- `packages/analyzer-roslyn` for the .NET/Roslyn analyzer bridge.
- `packages/mcp-server` for the TypeScript MCP server and bridge routing layer.
- `schemas` for JSON schemas and compact examples.
- `fixtures` for small test projects used by analyzer and MCP tests.
- `docs` for architecture, bridge contract, development, and usage documentation.
- `plugins/claude-code` for optional Claude Code packaging after the core MCP server exists.
- `openspec` for change proposals, specs, designs, and task plans.

Some directories may not exist until their first implementation slice lands.
Do not introduce a top-level `src`, `tests`, or `assets` layout unless the public docs and OpenSpec direction are updated together.

## Build, Test, and Development Commands

No build system, package manifest, or test runner is configured yet. Do not document commands without adding the matching tooling.

Before submitting changes, run checks that apply to touched files. Examples for future additions:

- `npm test` for JavaScript or TypeScript tests, once `package.json` exists.
- `python -m pytest` for Python tests, once a Python test suite exists.
- `make build` only if a `Makefile` defines that target.

Document new required commands in `README.md`.

## Coding Style & Naming Conventions

Follow the conventions of the language or framework introduced in each module. Prefer clear names. Use lowercase, hyphenated names for docs where possible, except `README.md`, `LICENSE`, and `AGENTS.md`.

Keep formatting automated once tooling is added. If adding a formatter or linter, commit its configuration and document the command.

## Testing Guidelines

Add tests alongside behavior changes. Place analyzer and MCP tests with their package unless the ecosystem has a stronger convention. Use `fixtures` for small cross-package sample projects. Test names should describe behavior, such as `test_builds_bridge_from_valid_input`.

Until a test framework is added, include a short manual verification note in pull requests for any behavior change.

## Commit & Pull Request Guidelines

Existing commits use short, imperative summaries such as `Initial commit` and `Add MIT License to the project`. Start with a capitalized verb and keep the first line concise.

Pull requests should include a description, motivation, verification steps, and linked issues when applicable. Include sample output for user-facing changes. Keep PRs focused.

## Security & Configuration Tips

Do not commit secrets, local credentials, or machine-specific configuration. If environment variables become necessary, document them with safe placeholder values in a sample env file.
