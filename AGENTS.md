# AGENTS.md

## Build/Lint/Test Commands
- Build: `dotnet build`
- Clean: `dotnet clean`
- Lint: `dotnet format` (install with `dotnet tool install -g dotnet-format` if needed; check with `--check`)
- Test: No test projects configured; add xUnit/NUnit for testing. Run all: `dotnet test`. Run single test: `dotnet test --filter "FullyQualifiedName=TestClass.TestMethod"`

## Code Style Guidelines
- **Naming**: PascalCase for classes, methods, properties, interfaces (I prefix). camelCase for locals/params. _camelCase for private fields.
- **Formatting**: 4 spaces indent, CRLF line endings, braces on new line, spaces around operators. Follow .editorconfig rules.
- **Imports**: Group system directives first, separate groups, outside namespace. Use implicit usings.
- **Types**: Nullable enabled. Prefer explicit types over var for clarity, except where obvious.
- **Async/Await**: Use async/await for all I/O operations. Avoid blocking calls.
- **Error Handling**: Use try-catch in async methods. Throw exceptions for invalid states. Validate inputs with FluentValidation in handlers.
- **Architecture**: Clean Architecture with CQRS/MediatR. Domain entities simple, no logic. Handlers in Application layer. Use AutoMapper for mappings.
- **Dependency Injection**: Register services in DependencyInjection.cs files per layer.
- **CQRS**: Use MediatR for commands/queries. Controllers send requests via Mediator.
- **Other**: Expression-bodied members preferred for properties/accessors. Readonly fields warning. No unnecessary suppressions.