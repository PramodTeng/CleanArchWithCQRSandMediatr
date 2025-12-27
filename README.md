# CleanArchWithCQRSandMediatr

A sample .NET solution demonstrating Clean Architecture with CQRS (Command Query Responsibility Segregation) using MediatR. The solution is split into projects following the typical Clean Architecture layers: API, Application, Domain and Infrastructure.

## Overview

This repository is intended as a learning/reference implementation that shows how to:
- Structure a .NET solution using Clean Architecture principles
- Implement CQRS with MediatR for request/handler separation
- Keep domain logic isolated from infrastructure and presentation

## Solution projects

- CleanArchWithCQRSandMediatr.Api - ASP.NET Core Web API (entry point)
- CleanArchWithCQRSandMediatr.Application - Application layer (CQRS handlers, DTOs, services)
- CleanArchWithCQRSandMediatr.Domain - Domain entities, value objects and domain logic
- CleanArchWithCQRSandMediatr.Infrastructure - Persistence, external services and implementations

(See folders in repository root for each project.)

## Prerequisites

- .NET SDK (recommended: 7.0 or later)
- A code editor such as Visual Studio, VS Code, or JetBrains Rider

## Getting started

1. Clone the repository

   git clone https://github.com/PramodTeng/CleanArchWithCQRSandMediatr.git
   cd CleanArchWithCQRSandMediatr

2. Restore and build

   dotnet restore
   dotnet build

3. Run the Web API

   cd CleanArchWithCQRSandMediatr.Api
   dotnet run

The API should start and listen on the configured port (see launchSettings or application configuration).

## Common commands

- Build solution: `dotnet build`
- Run API: `dotnet run --project CleanArchWithCQRSandMediatr.Api`

## Architecture notes

- The Domain project contains business entities and domain rules. It should have no external dependencies.
- The Application project contains CQRS commands/queries and their handlers (MediatR), DTOs and application services.
- The Infrastructure project contains concrete implementations such as EF Core DbContext, repositories, external integrations, and DI registrations.
- The Api project is the composition root: it configures DI, middleware and exposes the HTTP endpoints.

## Contributing

Contributions, suggestions and fixes are welcome. Consider opening an issue to discuss larger changes.

Suggested workflow:
- Fork the repository
- Create a feature branch
- Run and add tests if applicable
- Open a pull request describing your changes

## License

No license file found in this repository. If you want to add a license, create a LICENSE file (for example, MIT) in the repository root.

## Contact

Maintainer: GitHub user @PramodTeng


--
README generated and added by GitHub Copilot.
