# API_STRUCTURE.md

# MiniERP API – aktuální stromová struktura

Tento soubor popisuje aktuální strukturu backend části projektu MiniERP.  
Cílem je rychlá orientace v tom, kde se nachází jednotlivé vrstvy API, jakou mají roli a jaké soubory aktuálně obsahují.

---

# API / MiniERP.API

Hlavní backend projekt postavený na ASP.NET Core Web API.  
-  Obsahuje controllery
-  DTO objekty
-  service vrstvu
-  validace a základní konfiguraci aplikace.

---

## Controllers

Controllers představují vstupní body API.  
Přijímají HTTP požadavky, volají service vrstvu a vrací odpovědi klientovi.

### Soubory
- `CustomersController.cs`
- `InvoicesController.cs`
- `OrdersController.cs`
- `PaymentsController.cs`
- `ProductsController.cs`
- `StockController.cs`
- `StockMovementsController.cs`
- `WeatherForecastController.cs`

---

## DTOs

DTOs slouží pro přenos dat mezi API a klientem.  
Jsou rozdělené podle modulů, aby bylo jasné, které requesty a response modely patří ke které části systému.

---

### DTOs / Customers

DTO objekty pro práci se zákazníky.

#### Soubory
- `CreateCustomerRequest.cs`
- `CustomerDetailDto.cs`
- `CustomerListItemDto.cs`
- `UpdateCustomerRequest.cs`

---

### DTOs / Invoices

DTO objekty pro faktury a jejich detail.

#### Soubory
- `InvoiceDetailDto.cs`
- `InvoiceItemDetailDto.cs`
- `InvoiceListItemDto.cs`

---

### DTOs / Orders

DTO objekty pro objednávky a položky objednávek.

#### Soubory
- `CreateOrderItemRequest.cs`
- `CreateOrderRequest.cs`
- `OrderDetailDto.cs`
- `OrderItemDetailDto.cs`
- `OrderListItemDto.cs`
- `UpdateOrderRequest.cs`

---

### DTOs / Payments

DTO objekty pro platby.

#### Soubory
- `CreatePaymentRequest.cs`
- `PaymentDetailDto.cs`
- `PaymentListItemDto.cs`

---

### DTOs / Products

DTO objekty pro produkty.

#### Soubory
- `CreateProductRequest.cs`
- `ProductDetailDto.cs`
- `ProductListItemDto.cs`
- `UpdateProductRequest.cs`

---

### DTOs / Reports

DTO objekty pro reporty a souhrny.

#### Soubory
- `SalesSummaryDto.cs`

---

### DTOs / Stock

DTO objekty pro skladové zásoby.

#### Soubory
- `CreateStockRequest.cs`
- `StockDetailDto.cs`
- `StockListItemDto.cs`
- `UpdateStockRequest.cs`

---

### DTOs / StockMovements

DTO objekty pro přehled a detail skladových pohybů.

#### Soubory
- `StockMovementDetailDto.cs`
- `StockMovementListItemDto.cs`

---

## Properties

Složka s konfiguračními soubory projektu pro spuštění aplikace v různých profilech.

### Soubory
- `launchSettings.json`

---

## Services

Service vrstva obsahuje business logiku aplikace.  
Je rozdělena na implementace, rozhraní a pomocné result objekty.

---

### Services / Implementations

Konkrétní implementace jednotlivých služeb.

#### Soubory
- `CustomerService.cs`
- `InvoiceService.cs`
- `OrderService.cs`
- `PaymentService.cs`
- `ProductService.cs`
- `StockMovementService.cs`
- `StockService.cs`

---

### Services / Interfaces

Rozhraní definující kontrakty pro service vrstvu.  
Controllers pracují proti interface, ne přímo proti implementacím.

#### Soubory
- `ICustomerService.cs`
- `IInvoiceService.cs`
- `IOrderService.cs`
- `IPaymentService.cs`
- `IProductService.cs`
- `IStockMovementService.cs`
- `IStockService.cs`

---

### Services / Results

Pomocné návratové modely pro složitější operace v service vrstvě.

#### Soubory
- `CreateInvoiceFromOrderResult.cs`
- `ReserveStockResult.cs`

---

## Validators

Validátory kontrolují vstupní data request modelů dřív, než se zpracují v business logice.  
Projekt používá validace rozdělené podle modulů.

---

### Validators / Customers

Validace pro zákazníky.

#### Soubory
- `CreateCustomerRequestValidator.cs`
- `UpdateCustomerRequestValidator.cs`

---

### Validators / Orders

Validace pro objednávky a jejich položky.

#### Soubory
- `CreateOrderItemRequestValidator.cs`
- `CreateOrderRequestValidator.cs`
- `UpdateOrderRequestValidator.cs`

---

### Validators / Payments

Validace pro platby.

#### Soubory
- `CreatePaymentRequestValidator.cs`

---

### Validators / Products

Validace pro produkty.

#### Soubory
- `CreateProductRequestValidator.cs`
- `UpdateProductRequestValidator.cs`

---

### Validators / Stock

Validace pro sklad.

#### Soubory
- `CreateStockRequestValidator.cs`
- `UpdateStockRequestValidator.cs`

---

## Kořenové soubory projektu MiniERP.API

Tyto soubory tvoří základ konfigurace a spuštění API projektu.

### Soubory
- `.gitignore`
- `appsettings.Development.json`
- `appsettings.json`
- `MiniERP.API.csproj`
- `Program.cs`
- `WeatherForecast.cs`

---

# Stručné shrnutí architektury API

Aktuální API projekt je rozdělen do těchto hlavních částí:

- **Controllers** – přijímají požadavky a vrací odpovědi
- **DTOs** – přenos dat mezi klientem a API
- **Services** – business logika systému
- **Validators** – kontrola vstupních dat
- **Properties / config soubory** – konfigurace spuštění a prostředí


---
