# Business Insights AI Platform 🚀

Advanced B2B SaaS Multi-Tenant Platform built with **.NET 8**, **Clean Architecture**, and **Applied AI**. 

This repository serves as a public portfolio demonstrating enterprise-grade backend development, modern design patterns, and secure AI integration.

## 🌟 Key Architectural Features (Progressive MVP)

* **Clean Architecture:** Strict decoupling of Domain, Application, Infrastructure, and WebAPI layers.
* **Robust Multi-Tenancy:** Secure data isolation using dynamic **Global Query Filters** in EF Core based on JWT Claims. Prevent cross-tenant data leaks natively.
* **Hybrid Persistence Layer:** Optimized for performance. Uses **EF Core** for secure, structured transactional writes (CRUD) and **Dapper** for high-speed analytical queries.
* **Asynchronous Processing:** (In Progress) Event-driven background tasks using **RabbitMQ** and .NET Hosted Services.
* **Secure Applied AI:** (In Progress) Text-to-SQL semantic engine powered by OpenAI API, strictly sandboxed to enforce tenant data isolation.

## 🛠️ Tech Stack

* **Backend:** ASP.NET Core 8 Web API
* **Database:** SQL Server 2022 (Running on Docker)
* **ORMs:** Entity Framework Core 8 & Dapper
* **Messaging:** RabbitMQ
* **Observability:** Serilog with Correlation IDs

---
*Note: This project is under active development as part of a structured sprint roadmap.*