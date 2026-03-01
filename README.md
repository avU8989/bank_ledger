- the Core references nothing
- App references the Core because it has the domains and factories
- Infrastructure references Core because it implements the repo

We need an additional API layer, which references Core, App, Infrastructure
 --> ASP.NET Core --> WEB API project --> lies outside of the Core/App/Infrastructure and wires things together

So the structure would be like: 

Core = domain (models + business rules)
App = use-cases(services)
Infrastructure = database/files (SQL, JSON repos)
API = how the outside world would talk to our system (our API endpoints)

## How should i approach a project like the Bank Ledger ?

First of all before designing and implementing a system, i start from the business rules and use-cases not from the technology. I first clarify what the system must do (for a bank ledger, deposit , withdrawl and transfer) and write down the main flows and edge cases. 

Then i split the solution into layers with single responsibility: 
- Core/Domain: the domain model and invariant (e.g. Transaction, LedgerEntry, money rules) --> no databse, no http -- pure business logic
- Application: the use-cases that orchestrate the domain (e.g. TransactionService.ApplyAsync) and depend on the interfaces like repositories
- Infrastructure: implementations of those interfaces (SQLite/JSON), mapping, and external integrations
- Web: endpoints/controllers that handle HTTP, validate requests, map DTOs to domain commands/transactions and call the application layer 

I implement it vertically, one end-to-end use case first (e.g. transfere), with an in-memory repository + unit tests, and only after the behaviour is correct i migrate to a SQLite/JSON. --> that will allow me to prove the correctness/robustness early. 

Normally from what ive learned in the DSE course and reading the Open API design patterns, it is recommended to have a contract-first (design first) approach : 
    - Draft an Open API (YAML/JSON) with endpoints + schemas + examples
    - Validate it (linting + schema validation)
    - Team review (do we need endpoint A/B? Are status codes right? Pagination? Auth? Versioning?, does it adhere to our requirements?)
    - if all agree we lock in the contract 

We can generate Service Stubs for integration testing and/or generate a client to test on 

Contract Tests/Smoke Tests (usefull early)
    - API matches the Open API design ? 
    - Requests/Responses validate against the schema? 
    - Endpoints exists + correct status codes /content types?

Full integration tests (usually after some logic)
    - once the service is implemented 
    - integration tests for real flows

In my opinion one should do a smoke test/contract test first, then during writing the business logic test it with unit tests, then after that there will be a full integration tests to check if the endpoint is reachable and does exactly what we want to have, and then maybe additionally check it with postman ? 

# Clean Architecture Notes 

## Goal

Build a system where **business rules don’t depend on technical details** (DB, web framework, external services).  
Dependencies point **inward** toward the core.

---

## Layering / Projects

### 1) Domain (Core)
Contains the pure business model:

- Entities
- Value Objects
- Domain rules / invariants
- Domain events (optional)

**Depends on nothing.**  
This layer should not know about web controllers, HTTP, files, etc.

---

### 2) Application
Contains the **use cases** (application logic / orchestration):

- Commands & Queries (use cases)
- Handlers (implement each use case)
- DTOs (request/response models)
- Interfaces / Ports (e.g., repository abstractions, clock, current user)
- Validation (e.g., FluentValidation)
- Cross-cutting behaviors (logging, validation pipelines, authorization, etc.)

**Depends only on Domain.**  
Application defines *what it needs*, but not *how it’s done*.

---

### 3) Infrastructure
Contains technical implementations:

- Repository implementations / persistence adapters
- External services (email, file storage, identity provider integrations)
- Third-party API clients

**Depends on Application (+ Domain).**  
Infrastructure implements the interfaces (“ports”) defined by Application.

---

### 4) Web (API/UI)
The delivery mechanism:

- Controllers / endpoints
- Authentication & authorization setup
- Middleware / filters
- Request/response mapping
- DI composition root (wiring Application + Infrastructure)

**Depends on Application** (and usually references Infrastructure for dependency injection wiring).

---

## Core Rules

### Dependency Rule (inward dependencies)
`Web → Application → Domain`  
Infrastructure is “plugged in” from the outside.

### DTO boundary
- Web layer speaks in **DTOs** (request/response models).
- **Domain entities should not leak** to the outside world.

### Ports & Adapters
- **Interfaces (ports)** live in **Application**.
- **Implementations (adapters)** live in **Infrastructure**.

This keeps the core stable and testable.

---

## Use Case Pattern (Commands/Queries)

Instead of a large “god service” like:

- `AccountService.CreateAccount()`
- `AccountService.TransferMoney()`
- `AccountService.CloseAccount()`
- etc.

We use **one feature per use case**:

- `CreateAccountCommand` + `CreateAccountHandler`
- `TransferMoneyCommand` + `TransferMoneyHandler`
- `GetAccountQuery` + `GetAccountHandler`

**Why it helps**
- Each feature is small and focused
- Easier to understand and maintain
- Easier to test (one use case = one unit to verify)
- Cleaner PRs and change isolation

> Note: Handlers are essentially “application services”, just split by feature.

---

## Testing Strategy

### Unit tests (fast)
Test the **Application** layer use cases with mocks/fakes for ports.

- No database
- No web server
- No infrastructure dependencies

### Integration tests (realistic)
Test **Infrastructure** with a real database (or container) to verify:

- EF Core mapping
- queries, persistence, transactions
- migrations, constraints, etc.

---

## Relation to Onion / Hexagonal Architecture

Clean Architecture is very close to **Onion** and **Hexagonal (Ports & Adapters)**:
We have UI -> Business Logic Layer -> Data Access Layer
- Keep business logic in the center
- Push technical details outward
- Use interfaces at boundaries

---

## Summary

Clean Architecture gives:
- clear boundaries
- testable business logic
- separation between “what the system does” (Application/Domain) and “how it runs” (Infrastructure/Web)

Tradeoff:
- more classes/files, but each is smaller and easier to reason about.

# Monolithic System vs Microservices: 
