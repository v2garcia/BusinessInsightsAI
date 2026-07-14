# Business Insights AI Platform 🚀

Advanced B2B SaaS Multi-Tenant Platform built with **.NET 8**, **Clean Architecture**, **SQL Server**, **MassTransit/RabbitMQ**, and **Secure Applied AI**.

## 📐 Architecture Diagram

```mermaid
graph TD
    Client[HTTP Client / POS / ERP] -->|JWT Auth / X-Tenant-Id| API[ASP.NET Core 8 Web API]
    API -->|1. Extract Context| Middleware[TenantMiddleware & CorrelationId]
    API -->|2. Write Fast| EFCore[EF Core Context + Global Filter]
    EFCore -->|Store Transactional Data| DB[(SQL Server 2022)]
    
    API -->|3. Publish IngestedEvent| MT[MassTransit Bus]
    MT -->|Async Queue| RMQ((RabbitMQ Broker))
    RMQ -->|Consume Event| Worker[Background Worker Service]
    Worker -->|4. Compute Analytics Async| DB
    
    Client -->|Ask Insight: Natural Language| API
    API -->|5. Request Schema-Only SQL| OpenAI[OpenAI API gpt-4o-mini]
    OpenAI -->|6. Return Sandboxed SQL String| API
    API -->|7. Execute Ultra-Fast passing @TenantId| Dapper[Dapper Core Engine]
    Dapper -->|Query Filtered Data| DB

```

## 🚀 Quick Start

You can run the entire infrastructure, including the database and message broker, locally with a single command:

```bash

docker compose up --build -d

```

## Infrastructure Services Available Locally

- SQL Server: `localhost,1433`

- RabbitMQ Management Portal: `http://localhost:15672`

- RabbitMQ credentials: `guest / guest`

- Swagger API Documentation: `http://localhost:<port>/swagger`

## 🧠 Key Senior Highlights Demonstrated

### Zero Data Leakage

Built-in dynamic Global Query Filters applied at the EF Core level help enforce tenant isolation and reduce the risk of cross-tenant data exposure.

### Hybrid Performance

The platform uses Entity Framework Core for safe transactional writes and Dapper for optimized analytical reads.

### Distributed Observability

Structured telemetry is implemented with Serilog and a consistent `CorrelationId` across HTTP requests, enabling easier troubleshooting and request tracing.

### Asynchronous Processing

MassTransit and RabbitMQ decouple heavy background processing from the Web API request lifecycle, allowing the API to respond quickly while workers process analytical tasks asynchronously.
