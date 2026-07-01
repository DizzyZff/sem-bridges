# Repository Guidelines

## Project Structure & Module Organization

This repository is currently minimal. The root contains `README.md` for project overview material and `LICENSE` for the MIT license. No source, test, or asset directories exist yet.

When adding implementation code, keep the structure predictable:

- `src/` for application or library code.
- `tests/` for automated tests that mirror `src/` where practical.
- `docs/` for design notes and contributor documentation.
- `assets/` for static files, fixtures, or sample inputs.

Keep top-level files reserved for repository-wide metadata and entry points.

## Build, Test, and Development Commands

No build system, package manager manifest, or test runner is configured yet. Do not invent commands in documentation without adding the matching tooling.

Before submitting changes, run checks that apply to the files you touched. Examples for future additions:

- `npm test` for JavaScript or TypeScript tests, once `package.json` exists.
- `python -m pytest` for Python tests, once a Python test suite exists.
- `make build` only if a `Makefile` defines that target.

Document new required commands in `README.md` when introducing tooling.

## Coding Style & Naming Conventions

Follow the conventions of the language or framework introduced in each module. Prefer clear names over abbreviations. Use lowercase, hyphenated names for Markdown and documentation files where possible, except conventional files such as `README.md`, `LICENSE`, and `AGENTS.md`.

Keep formatting automated once tooling is added. If adding a formatter or linter, commit its configuration and document the command.

## Testing Guidelines

Add tests alongside meaningful behavior changes. Place tests under `tests/` unless the chosen ecosystem has a stronger convention. Test names should describe behavior, such as `test_builds_bridge_from_valid_input` or `creates bridge from valid input`.

Until a test framework is added, include a short manual verification note in pull requests for any behavior change.

## Commit & Pull Request Guidelines

Existing commits use short, imperative summaries such as `Initial commit` and `Add MIT License to the project`. Continue that style: start with a capitalized verb and keep the first line concise.

Pull requests should include a description, motivation, verification steps, and linked issues when applicable. Include screenshots or sample output for user-facing changes. Keep PRs focused; separate unrelated setup, feature work, and documentation changes when practical.

## Security & Configuration Tips

Do not commit secrets, local credentials, or machine-specific configuration. If environment variables become necessary, document them with safe placeholder values in a sample env file.
