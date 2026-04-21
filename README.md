# MiniERP_2.0

Projekt je zaměřený na postupné budování modulárního ERP systému pro firemní prostředí.  

Cílem je vytvoření stabilního backendového základu, na který budeme moci dále napojovat další části systému podle potřeb firmy.

---

# Technologie

Projekt je postaven na backend stacku:

- .NET 8
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Swagger

---

# Databázový návrh

Databázová struktura již nyní obsahuje připravené klíčové oblasti systému:

- Users / Roles / UserRoles *(uživatelé, role, oprávnění)*
- Customers *(zákazníci)*
- Suppliers *(dodavatelé)*
- Categories *(kategorie produktů)*
- Products *(produkty)*
- Warehouses *(sklady)*
- Stock / StockMovements *(zásoby a pohyby skladu)*
- Orders / OrderItems *(objednávky)*
- Invoices / InvoiceItems *(fakturace)*
- Payments *(platby)*
- AuditLogs *(auditní záznamy systému)*


`Create_database.sql` - soubor Inicializačním SQL scriptem databáze


---

# Architektura projektu

Projekt používá vícevrstvou strukturu:


MiniERP.sln
API/        - Web API vrstva
Core/       - sdílené modely a logika
Data/       - databázová vrstva / EF Core
Tests/      - automatické testy (připravuje se)

API_STRUCTURE.md - soubor stromové struktury 
  -  Last Update - LOG 1.2.1

---

# Moduly

## 1. Order-to-Cash *(Hotovo)*

První modul systému.  
Řeší proces od vytvoření zákazníka, přes objednávku, rezervaci skladu, vystavení faktury až po evidenci úhrady.

➡ Detailní popis procesu je v souboru: RunBook_ORDER_TO_CASH

