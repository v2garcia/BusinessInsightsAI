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