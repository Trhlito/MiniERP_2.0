# MiniERP

MiniERP je studijní backend projekt vytvořený v ASP.NET Core.  
Cílem projektu bylo pochopit návrh backendu pro ERP systém – práci s databází, business logikou, API vrstvou a autentizací.

Projekt vznikal postupně jako praktický trénink backend vývoje.

---

## Co jsem se na projektu naučil

- návrh databázového modelu (Orders, Customers, Stock, Invoices, Payments)
- tvorba REST API v ASP.NET Core
- oddělení vrstev (Controller → Service → DTO → Data)
- práce se SQL Serverem a stored procedures
- řešení business logiky (Order-to-Cash flow)
- autentizace a autorizace pomocí ASP.NET Identity + JWT
- základní audit a bezpečnostní logování

---

## Použité technologie

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server (Docker)
- ASP.NET Identity
- JWT (Bearer autentizace)
- Swagger (OpenAPI)

---

## Funkcionalita

### Základní moduly
- Customers (CRUD)
- Orders + OrderItems
- Stock + StockMovements
- Invoices + InvoiceItems
- Payments

### Order-to-Cash flow
- vytvoření objednávky
- rezervace skladu
- vytvoření faktury z objednávky
- registrace platby
- změna stavu objednávky a faktury

### Reporting (SQL stored procedures)
- Sales summary
- Unpaid invoices
- Sales by customer
- Top selling products
- Stock alerts

### Autentizace a bezpečnost
- login přes ASP.NET Identity
- JWT token
- refresh token lifecycle (rotation)
- logout (zneplatnění tokenu)
- role-based access control (Admin, Manager, User)

### Audit a security reporting
- Auth audit logy (login, refresh, logout)
- přehled neúspěšných přihlášení
- audit konkrétního uživatele
- správa refresh tokenů (revoke, cleanup)

---

## Architektura

Projekt je rozdělen do vrstev:

- **Controllers** – vstupní body API
- **Services** – business logika
- **DTOs** – data přenášená přes API
- **Data (EF Core)** – databázový přístup

Kritická business logika (např. sklad, fakturace) je částečně přesunuta do SQL stored procedures, aby byla zajištěna konzistence dat na úrovni databáze.

---

## Spuštění projektu

1. Spustit SQL Server (např. přes Docker)
2. Vytvořit databázi pomocí SQL scriptu:


# Swagger API Overview

## Core Modules 

![Swagger Main 1](docs/images/Swagger_API.png)


## Post Order (example)

![Swagger_post_orders](docs/images/swagger_post_orders.png)



## SQL Schéma kterého se držíme 

![Swagger_post_orders](docs/images/SQL.png)



---
# Moduly

## 1. Order-to-Cash *(Hotovo)*

První modul systému.  
Řeší proces od vytvoření zákazníka, přes objednávku, rezervaci skladu, vystavení faktury až po evidenci úhrady.

- Detailní popis procesu je v souboru: RunBook_ORDER_TO_CASH

## Testování autentizace
- použít endpoint /api/Auth/login
- získaný JWT token vložit do Swagger Authorize
- následně testovat chráněné endpointy


## Další rozvoj ->
- doplnění validací (FluentValidation)
- stránkování a filtrování endpointů
- archivace historických logů
- rozšíření reporting modulu
- lepší správa warehouse (více skladů)