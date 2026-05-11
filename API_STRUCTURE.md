# MiniERP API – aktuální stromová struktura

Backend struktura **MiniERP_2.0**.  
Cílem je rychlá orientace v tom, kde se nachází jednotlivé vrstvy API, jakou mají roli a jaké soubory aktuálně obsahují.
Vytvořeno k Logu: `2.0.57`

---

# Struktura solution projektu

Projekt je rozdělen do několika hlavních částí, aby byla oddělena API vrstva, business logika a práce s databází.

---

## API / MiniERP.API 

Hlavní ASP.NET Core Web API projekt.  
Obsahuje controllery, DTO objekty, services, validace a konfiguraci aplikace.

- **Controllers** – slouží k přijímání HTTP požadavků a vrací odpovědi klientovi  
- **DTOs** – zajišťují přenos dat mezi klientem a API  
- **Services** – obsahují business logiku aplikace  
- **Validators** – kontrolují vstupní data request modelů  
- **Auth & Users** – zajišťují autentizaci, autorizaci a správu uživatelů (ASP.NET Identity + JWT)  
- **Reports** – poskytují business a bezpečnostní reporty systému  
- **Seed** – obsahuje inicializační data aplikace  
- **Config** – obsahuje konfiguraci aplikace a prostředí  

---

## Data / MiniERP.Data

Datová vrstva projektu.  
Obsahuje databázové entity, DbContext a konfiguraci přístupu k databázi přes Entity Framework Core.

- **Entities** – představují databázové modely aplikace a mapují tabulky databáze  
- **Auth Entities** – entity pro autentizaci, role, refresh tokeny a auditní logy  
- **ApplicationDbContext** – hlavní databázový kontext pro komunikaci s databází  
- **EF Core Configuration** – konfigurace relací, mapování a databázového přístupu  



---- 


## MiniERP.API

## Controllers

Controllers představují **vstupní body API**.  
Přijímají HTTP požadavky, volají service vrstvu a vrací odpovědi klientovi.

- `AuthController.cs`
- `CustomersController.cs`
- `InvoicesController.cs`
- `OrdersController.cs`
- `PaymentsController.cs`
- `ProductsController.cs`
- `ReportsController.cs`
- `SecurityReportsController.cs`
- `StockController.cs`
- `StockMovementsController.cs`
<<<<<<< HEAD
=======
- `UsersController.cs`
>>>>>>> 7e253a6 (Update API structure documentation)

---

## DTOs

DTOs slouží pro **přenos dat mezi API a klientem**.  
Jsou rozdělené podle modulů, aby bylo jasné, které requesty a response modely patří ke které části systému.


### DTOs / Auth

DTO objekty pro **autentizaci a práci s tokeny**.

- `CurrentUserResponse.cs`
- `LoginRequest.cs`
- `LoginResponse.cs`
- `LogoutRequest.cs`
- `RefreshToken.cs`

### DTOs / Customers

DTO objekty pro práci se zákazníky.

- `CreateCustomerRequest.cs`
- `CustomerDetailDto.cs`
- `CustomerListItemDto.cs`
- `UpdateCustomerRequest.cs`

### DTOs / Invoices

DTO objekty pro faktury a jejich detail.

- `InvoiceDetailDto.cs`
- `InvoiceItemDetailDto.cs`
- `InvoiceListItemDto.cs`

### DTOs / Orders

DTO objekty pro objednávky a položky objednávek.

- `CreateOrderItemRequest.cs`
- `CreateOrderRequest.cs`
- `OrderDetailDto.cs`
- `OrderItemDetailDto.cs`
- `OrderListItemDto.cs`
- `UpdateOrderRequest.cs`

### DTOs / Payments

DTO objekty pro platby.

- `CreatePaymentRequest.cs`
- `PaymentDetailDto.cs`
- `PaymentListItemDto.cs`

### DTOs / Products

DTO objekty pro produkty.

- `CreateProductRequest.cs`
- `ProductDetailDto.cs`
- `ProductListItemDto.cs`
- `UpdateProductRequest.cs`

### DTOs / Reports

DTO objekty pro **business reporty a souhrny**.

- `SalesSummaryDto.cs`
- `SalesByCustomerDto.cs`
- `TopSellingProductDto.cs`
- `UnpaidInvoiceDto.cs`
- `StockAlertDto.cs`

### DTOs / Stock

DTO objekty pro skladové zásoby.

- `CreateStockRequest.cs`
- `StockDetailDto.cs`
- `StockListItemDto.cs`
- `UpdateStockRequest.cs`

### DTOs / StockMovements

DTO objekty pro přehled a detail skladových pohybů.

- `StockMovementDetailDto.cs`
- `StockMovementListItemDto.cs`

### DTOs / Users

DTO objekty pro **správu uživatelů**.

- `CreateUserRequest.cs`
- `ResetPasswordRequest.cs`
- `UpdateUserRolesRequest.cs`
- `UserDetailDto.cs`
- `UserListItemDto.cs`


---

## Services

Service vrstva obsahuje **business logiku aplikace**.  
Je rozdělena na implementace, rozhraní a pomocné result objekty.

### Services / Implementations

Konkrétní implementace jednotlivých služeb.

- `AuthService.cs`
- `CustomerService.cs`
- `InvoiceService.cs`
- `OrderService.cs`
- `PaymentService.cs`
- `ProductService.cs`
- `ReportService.cs`
- `SecurityReportService.cs`
- `StockMovementService.cs`
- `StockService.cs`
- `UserService.cs`

### Services / Interfaces

Rozhraní definující kontrakty pro service vrstvu.  
Controllers pracují proti interface, ne přímo proti implementacím.

- `IAuthService.cs`
- `ICustomerService.cs`
- `IInvoiceService.cs`
- `IOrderService.cs`
- `IPaymentService.cs`
- `IProductService.cs`
- `IReportService.cs`
- `ISecurityReportService.cs`
- `IStockMovementService.cs`
- `IStockService.cs`
- `IUserService.cs`

---

### Services / Results

Pomocné návratové modely pro složitější operace v service vrstvě.

#### Soubory
- `CreateInvoiceFromOrderResult.cs`
- `ReserveStockResult.cs`

---

## Validators

Validátory kontrolují vstupní data request modelů dřív, než se zpracují v business logice.

### Validators / Customers

- `CreateCustomerRequestValidator.cs`
- `UpdateCustomerRequestValidator.cs`

### Validators / Orders

- `CreateOrderItemRequestValidator.cs`
- `CreateOrderRequestValidator.cs`
- `UpdateOrderRequestValidator.cs`

### Validators / Payments

- `CreatePaymentRequestValidator.cs`

### Validators / Products

- `CreateProductRequestValidator.cs`
- `UpdateProductRequestValidator.cs`

### Validators / Stock

- `CreateStockRequestValidator.cs`
- `UpdateStockRequestValidator.cs`

---

## Kořenové soubory projektu MiniERP.API

Tyto soubory tvoří základ konfigurace a spuštění API projektu.

- `.gitignore`
- `appsettings.Development.json`
- `appsettings.json`
- `MiniERP.API.csproj`
- `Program.cs`

---

## Data / MiniERP.Data

## Entities

Entities představují **databázové modely aplikace**.  
Mapují tabulky databáze pomocí Entity Framework Core a slouží jako základ pro práci s uloženými daty.

- `Customer.cs`
- `Invoice.cs`
- `InvoiceItem.cs`
- `OrderItem.cs`
- `Orders.cs`
- `Payment.cs`
- `Product.cs`
- `Stock.cs`
- `StockMovement.cs`

### Entities / Auth

Entity pro autentizaci, správu uživatelů, refresh tokeny a auditní záznamy.

- `ApplicationUser.cs`
- `ApplicationRole.cs`
- `RefreshToken.cs`
- `AuthAuditLog.cs`