# TicketAS

A domain-driven IT support ticketing system built with C# and .NET 8, currently containing the domain layer (`Core`) and its unit test suite (`Core.Tests`).

## Overview

TicketAS models the lifecycle of an IT support ticket—from initial incident reporting through agent assignment, conversation, and final resolution. Key domain rules include:

* **Qualification:** Agents must meet an incident category's seniority and specialization requirements before they can be assigned.
* **Explicit Lifecycle:** Domain state machines govern valid actions (e.g., tickets cannot be resolved before starting, and closed tickets are immutable).
* **Preserved History:** Reassigning an agent adds a new participant without removing former ones, preserving full message history and attribution.

## Project Structure

```text
TicketAS/
├── Core/                      # Domain layer
│   ├── Users/                 # User aggregate & value objects (UserName, Email, UserPassword)
│   ├── Reporters/             # Reporter aggregate & ReporterState hierarchy
│   ├── Agents/                # Agent aggregate, AgentState hierarchy, Seniority, & Specialization
│   ├── Incidents/             # Incident aggregate, Category, Scope, Environment, Severity, & SLA
│   ├── Tickets/               # Ticket aggregate, TicketLifecycle state machine, & Conversation
│   ├── Enums/                 # Shared domain enums
│   ├── Exceptions/            # Domain-specific exceptions per aggregate
│   └── Utilities/             # Base abstractions (Entity<T>, StrongTypedId)
└── Core.Tests/                # xUnit test suite mirroring the Core directory layout
```

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Restore, Build & Test

1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run tests:**
   ```bash
   dotnet test
   ```

## License

This project is licensed under the MIT License 