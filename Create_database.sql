-- =============================================
-- FOR: MiniERP
-- Finální script pro vytvoření databáze 
-- SQL server / Dbeaver
-- =============================================

SET NOCOUNT ON;

-- -- Vytvoření databáze pokud neexistuje --
IF DB_ID(N'MiniERP_Dev') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE MiniERP_Dev');
END;

USE MiniERP_Dev;

-- =============================================
-- Bezpečné mazání při opakovaném testování
-- =============================================

IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NOT NULL DROP TABLE dbo.AuditLogs;
IF OBJECT_ID(N'dbo.Payments', N'U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID(N'dbo.InvoiceItems', N'U') IS NOT NULL DROP TABLE dbo.InvoiceItems;
IF OBJECT_ID(N'dbo.Invoices', N'U') IS NOT NULL DROP TABLE dbo.Invoices;
IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID(N'dbo.Orders', N'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID(N'dbo.StockMovements', N'U') IS NOT NULL DROP TABLE dbo.StockMovements;
IF OBJECT_ID(N'dbo.Stock', N'U') IS NOT NULL DROP TABLE dbo.Stock;
IF OBJECT_ID(N'dbo.Warehouses', N'U') IS NOT NULL DROP TABLE dbo.Warehouses;
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID(N'dbo.Suppliers', N'U') IS NOT NULL DROP TABLE dbo.Suppliers;
IF OBJECT_ID(N'dbo.Customers', N'U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NOT NULL DROP TABLE dbo.UserRoles;
IF OBJECT_ID(N'dbo.Roles', N'U') IS NOT NULL DROP TABLE dbo.Roles;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;

-- =============================================
-- 1. USERS / ROLES
-- =============================================

CREATE TABLE dbo.Users
(
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Username        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(150) NOT NULL,
    PasswordHash    NVARCHAR(255) NOT NULL,
    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NOT NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt       DATETIME2 NULL
);

ALTER TABLE dbo.Users
ADD CONSTRAINT UQ_Users_Username UNIQUE (Username);

ALTER TABLE dbo.Users
ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);

CREATE TABLE dbo.Roles
(
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name            NVARCHAR(50) NOT NULL,
    Description     NVARCHAR(255) NULL
);

ALTER TABLE dbo.Roles
ADD CONSTRAINT UQ_Roles_Name UNIQUE (Name);

CREATE TABLE dbo.UserRoles
(
    UserId          INT NOT NULL,
    RoleId          INT NOT NULL,
    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId)
);

ALTER TABLE dbo.UserRoles
ADD CONSTRAINT FK_UserRoles_Users
FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);

ALTER TABLE dbo.UserRoles
ADD CONSTRAINT FK_UserRoles_Roles
FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id);

-- =============================================
-- 2. CUSTOMERS / SUPPLIERS
-- =============================================

CREATE TABLE dbo.Customers
(
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerType    NVARCHAR(20) NOT NULL,
    CompanyName     NVARCHAR(200) NULL,
    FirstName       NVARCHAR(100) NULL,
    LastName        NVARCHAR(100) NULL,
    Email           NVARCHAR(150) NULL,
    Phone           NVARCHAR(50) NULL,
    Street          NVARCHAR(150) NULL,
    City            NVARCHAR(100) NULL,
    ZipCode         NVARCHAR(20) NULL,
    Country         NVARCHAR(100) NULL,
    ICO             NVARCHAR(20) NULL,
    DIC             NVARCHAR(30) NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Customers_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt       DATETIME2 NULL,
    CONSTRAINT CK_Customers_CustomerType CHECK (CustomerType IN ('Person', 'Company'))
);

CREATE TABLE dbo.Suppliers
(
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyName     NVARCHAR(200) NOT NULL,
    ContactName     NVARCHAR(150) NULL,
    Email           NVARCHAR(150) NULL,
    Phone           NVARCHAR(50) NULL,
    Street          NVARCHAR(150) NULL,
    City            NVARCHAR(100) NULL,
    ZipCode         NVARCHAR(20) NULL,
    Country         NVARCHAR(100) NULL,
    ICO             NVARCHAR(20) NULL,
    DIC             NVARCHAR(30) NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_Suppliers_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Suppliers_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt       DATETIME2 NULL
);

-- =============================================
-- 3. CATEGORIES / PRODUCTS
-- =============================================

CREATE TABLE dbo.Categories
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name                NVARCHAR(100) NOT NULL,
    Description         NVARCHAR(255) NULL,
    ParentCategoryId    INT NULL,
    IsActive            BIT NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT (1)
);

ALTER TABLE dbo.Categories
ADD CONSTRAINT FK_Categories_ParentCategory
FOREIGN KEY (ParentCategoryId) REFERENCES dbo.Categories(Id);

CREATE TABLE dbo.Products
(
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Code            NVARCHAR(50) NOT NULL,
    Name            NVARCHAR(200) NOT NULL,
    Description     NVARCHAR(MAX) NULL,
    CategoryId      INT NOT NULL,
    SupplierId      INT NULL,
    PurchasePrice   DECIMAL(18,2) NOT NULL,
    SalePrice       DECIMAL(18,2) NOT NULL,
    VatRate         DECIMAL(5,2) NOT NULL,
    Unit            NVARCHAR(20) NOT NULL,
    MinimumStock    DECIMAL(18,2) NOT NULL CONSTRAINT DF_Products_MinimumStock DEFAULT (0),
    IsService       BIT NOT NULL CONSTRAINT DF_Products_IsService DEFAULT (0),
    IsActive        BIT NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT (1),
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Products_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt       DATETIME2 NULL,
    CONSTRAINT CK_Products_PurchasePrice CHECK (PurchasePrice >= 0),
    CONSTRAINT CK_Products_SalePrice CHECK (SalePrice >= 0),
    CONSTRAINT CK_Products_MinimumStock CHECK (MinimumStock >= 0),
    CONSTRAINT CK_Products_VatRate CHECK (VatRate >= 0)
);

ALTER TABLE dbo.Products
ADD CONSTRAINT UQ_Products_Code UNIQUE (Code);

ALTER TABLE dbo.Products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id);

ALTER TABLE dbo.Products
ADD CONSTRAINT FK_Products_Suppliers
FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(Id);

-- =============================================
-- 4. WAREHOUSES / STOCK
-- =============================================

CREATE TABLE dbo.Warehouses
(
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Code            NVARCHAR(30) NOT NULL,
    Name            NVARCHAR(100) NOT NULL,
    Description     NVARCHAR(255) NULL,
    Street          NVARCHAR(150) NULL,
    City            NVARCHAR(100) NULL,
    ZipCode         NVARCHAR(20) NULL,
    Country         NVARCHAR(100) NULL,
    IsActive        BIT NOT NULL CONSTRAINT DF_Warehouses_IsActive DEFAULT (1)
);

ALTER TABLE dbo.Warehouses
ADD CONSTRAINT UQ_Warehouses_Code UNIQUE (Code);

CREATE TABLE dbo.Stock
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    WarehouseId         INT NOT NULL,
    ProductId           INT NOT NULL,
    Quantity            DECIMAL(18,2) NOT NULL CONSTRAINT DF_Stock_Quantity DEFAULT (0),
    ReservedQuantity    DECIMAL(18,2) NOT NULL CONSTRAINT DF_Stock_ReservedQuantity DEFAULT (0),
    LastUpdatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Stock_LastUpdatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT CK_Stock_Quantity CHECK (Quantity >= 0),
    CONSTRAINT CK_Stock_ReservedQuantity CHECK (ReservedQuantity >= 0)
);

ALTER TABLE dbo.Stock
ADD CONSTRAINT UQ_Stock_Warehouse_Product UNIQUE (WarehouseId, ProductId);

ALTER TABLE dbo.Stock
ADD CONSTRAINT FK_Stock_Warehouses
FOREIGN KEY (WarehouseId) REFERENCES dbo.Warehouses(Id);

ALTER TABLE dbo.Stock
ADD CONSTRAINT FK_Stock_Products
FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id);

CREATE TABLE dbo.StockMovements
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StockId             INT NOT NULL,
    WarehouseId         INT NOT NULL,
    ProductId           INT NOT NULL,
    MovementType        NVARCHAR(30) NOT NULL,
    Quantity            DECIMAL(18,2) NOT NULL,
    QuantityBefore      DECIMAL(18,2) NOT NULL,
    QuantityAfter       DECIMAL(18,2) NOT NULL,
    ReservedBefore      DECIMAL(18,2) NOT NULL,
    ReservedAfter       DECIMAL(18,2) NOT NULL,
    ReferenceType       NVARCHAR(30) NULL,
    ReferenceId         INT NULL,
    Note                NVARCHAR(255) NULL,
    CreatedByUserId     INT NOT NULL,
    CreatedAt           DATETIME2 NOT NULL CONSTRAINT DF_StockMovements_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_StockMovements_Stock
        FOREIGN KEY (StockId) REFERENCES dbo.Stock(Id),
    CONSTRAINT FK_StockMovements_Warehouses
        FOREIGN KEY (WarehouseId) REFERENCES dbo.Warehouses(Id),
    CONSTRAINT FK_StockMovements_Products
        FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id),
    CONSTRAINT FK_StockMovements_Users
        FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(Id),
    CONSTRAINT CK_StockMovements_MovementType
        CHECK (MovementType IN ('IN', 'OUT', 'RESERVE', 'RELEASE', 'ADJUSTMENT_PLUS', 'ADJUSTMENT_MINUS')),
    CONSTRAINT CK_StockMovements_ReferenceType
        CHECK (ReferenceType IS NULL OR ReferenceType IN ('Order', 'Manual', 'StockAdjustment', 'Import')),
    CONSTRAINT CK_StockMovements_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_StockMovements_QuantityBefore CHECK (QuantityBefore >= 0),
    CONSTRAINT CK_StockMovements_QuantityAfter CHECK (QuantityAfter >= 0),
    CONSTRAINT CK_StockMovements_ReservedBefore CHECK (ReservedBefore >= 0),
    CONSTRAINT CK_StockMovements_ReservedAfter CHECK (ReservedAfter >= 0)
);

-- =============================================
-- 5. ORDERS / ORDER ITEMS
-- =============================================

CREATE TABLE dbo.Orders
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderNumber         NVARCHAR(50) NOT NULL,
    CustomerId          INT NOT NULL,
    OrderDate           DATETIME2 NOT NULL,
    RequiredDate        DATETIME2 NULL,
    Status              NVARCHAR(30) NOT NULL,
    Subtotal            DECIMAL(18,2) NOT NULL CONSTRAINT DF_Orders_Subtotal DEFAULT (0),
    VatTotal            DECIMAL(18,2) NOT NULL CONSTRAINT DF_Orders_VatTotal DEFAULT (0),
    TotalAmount         DECIMAL(18,2) NOT NULL CONSTRAINT DF_Orders_TotalAmount DEFAULT (0),
    Currency            NVARCHAR(10) NOT NULL CONSTRAINT DF_Orders_Currency DEFAULT ('CZK'),
    Note                NVARCHAR(500) NULL,
    CreatedByUserId     INT NOT NULL,
    CreatedAt           DATETIME2 NOT NULL CONSTRAINT DF_Orders_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt           DATETIME2 NULL,
    CONSTRAINT CK_Orders_Status CHECK (Status IN ('Draft', 'Confirmed', 'Cancelled', 'Completed')),
    CONSTRAINT CK_Orders_Subtotal CHECK (Subtotal >= 0),
    CONSTRAINT CK_Orders_VatTotal CHECK (VatTotal >= 0),
    CONSTRAINT CK_Orders_TotalAmount CHECK (TotalAmount >= 0)
);

ALTER TABLE dbo.Orders
ADD CONSTRAINT UQ_Orders_OrderNumber UNIQUE (OrderNumber);

ALTER TABLE dbo.Orders
ADD CONSTRAINT FK_Orders_Customers
FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(Id);

ALTER TABLE dbo.Orders
ADD CONSTRAINT FK_Orders_Users
FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(Id);

CREATE TABLE dbo.OrderItems
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderId             INT NOT NULL,
    ProductId           INT NOT NULL,
    ItemName            NVARCHAR(200) NOT NULL,
    Quantity            DECIMAL(18,2) NOT NULL,
    UnitPrice           DECIMAL(18,2) NOT NULL,
    VatRate             DECIMAL(5,2) NOT NULL,
    DiscountPercent     DECIMAL(5,2) NULL CONSTRAINT DF_OrderItems_DiscountPercent DEFAULT (0),
    LineSubtotal        DECIMAL(18,2) NOT NULL,
    LineVatAmount       DECIMAL(18,2) NOT NULL,
    LineTotal           DECIMAL(18,2) NOT NULL,
    CONSTRAINT CK_OrderItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_OrderItems_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT CK_OrderItems_VatRate CHECK (VatRate >= 0),
    CONSTRAINT CK_OrderItems_DiscountPercent CHECK (DiscountPercent >= 0 AND DiscountPercent <= 100),
    CONSTRAINT CK_OrderItems_LineSubtotal CHECK (LineSubtotal >= 0),
    CONSTRAINT CK_OrderItems_LineVatAmount CHECK (LineVatAmount >= 0),
    CONSTRAINT CK_OrderItems_LineTotal CHECK (LineTotal >= 0)
);

ALTER TABLE dbo.OrderItems
ADD CONSTRAINT FK_OrderItems_Orders
FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id);

ALTER TABLE dbo.OrderItems
ADD CONSTRAINT FK_OrderItems_Products
FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id);

-- =============================================
-- 6. INVOICES / INVOICE ITEMS
-- =============================================

CREATE TABLE dbo.Invoices
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    InvoiceNumber       NVARCHAR(50) NOT NULL,
    OrderId             INT NULL,
    CustomerId          INT NOT NULL,
    IssueDate           DATETIME2 NOT NULL,
    DueDate             DATETIME2 NOT NULL,
    PaidDate            DATETIME2 NULL,
    Status              NVARCHAR(30) NOT NULL,
    Subtotal            DECIMAL(18,2) NOT NULL CONSTRAINT DF_Invoices_Subtotal DEFAULT (0),
    VatTotal            DECIMAL(18,2) NOT NULL CONSTRAINT DF_Invoices_VatTotal DEFAULT (0),
    TotalAmount         DECIMAL(18,2) NOT NULL CONSTRAINT DF_Invoices_TotalAmount DEFAULT (0),
    Currency            NVARCHAR(10) NOT NULL CONSTRAINT DF_Invoices_Currency DEFAULT ('CZK'),
    Note                NVARCHAR(500) NULL,
    CreatedByUserId     INT NOT NULL,
    CreatedAt           DATETIME2 NOT NULL CONSTRAINT DF_Invoices_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt           DATETIME2 NULL,
    CONSTRAINT CK_Invoices_Status CHECK (Status IN ('Draft', 'Issued', 'Paid', 'Overdue', 'Cancelled')),
    CONSTRAINT CK_Invoices_Subtotal CHECK (Subtotal >= 0),
    CONSTRAINT CK_Invoices_VatTotal CHECK (VatTotal >= 0),
    CONSTRAINT CK_Invoices_TotalAmount CHECK (TotalAmount >= 0)
);

ALTER TABLE dbo.Invoices
ADD CONSTRAINT UQ_Invoices_InvoiceNumber UNIQUE (InvoiceNumber);

ALTER TABLE dbo.Invoices
ADD CONSTRAINT FK_Invoices_Orders
FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id);

ALTER TABLE dbo.Invoices
ADD CONSTRAINT FK_Invoices_Customers
FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(Id);

ALTER TABLE dbo.Invoices
ADD CONSTRAINT FK_Invoices_Users
FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(Id);

CREATE TABLE dbo.InvoiceItems
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    InvoiceId           INT NOT NULL,
    ProductId           INT NULL,
    ItemName            NVARCHAR(200) NOT NULL,
    Quantity            DECIMAL(18,2) NOT NULL,
    UnitPrice           DECIMAL(18,2) NOT NULL,
    VatRate             DECIMAL(5,2) NOT NULL,
    DiscountPercent     DECIMAL(5,2) NULL CONSTRAINT DF_InvoiceItems_DiscountPercent DEFAULT (0),
    LineSubtotal        DECIMAL(18,2) NOT NULL,
    LineVatAmount       DECIMAL(18,2) NOT NULL,
    LineTotal           DECIMAL(18,2) NOT NULL,
    CONSTRAINT CK_InvoiceItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_InvoiceItems_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT CK_InvoiceItems_VatRate CHECK (VatRate >= 0),
    CONSTRAINT CK_InvoiceItems_DiscountPercent CHECK (DiscountPercent >= 0 AND DiscountPercent <= 100),
    CONSTRAINT CK_InvoiceItems_LineSubtotal CHECK (LineSubtotal >= 0),
    CONSTRAINT CK_InvoiceItems_LineVatAmount CHECK (LineVatAmount >= 0),
    CONSTRAINT CK_InvoiceItems_LineTotal CHECK (LineTotal >= 0)
);

ALTER TABLE dbo.InvoiceItems
ADD CONSTRAINT FK_InvoiceItems_Invoices
FOREIGN KEY (InvoiceId) REFERENCES dbo.Invoices(Id);

ALTER TABLE dbo.InvoiceItems
ADD CONSTRAINT FK_InvoiceItems_Products
FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id);

-- =============================================
-- 7. PAYMENTS
-- =============================================

CREATE TABLE dbo.Payments
(
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    InvoiceId           INT NOT NULL,
    PaymentDate         DATETIME2 NOT NULL,
    Amount              DECIMAL(18,2) NOT NULL,
    PaymentMethod       NVARCHAR(30) NOT NULL,
    ReferenceNumber     NVARCHAR(100) NULL,
    Note                NVARCHAR(255) NULL,
    CreatedByUserId     INT NOT NULL,
    CreatedAt           DATETIME2 NOT NULL CONSTRAINT DF_Payments_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT CK_Payments_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Payments_PaymentMethod CHECK (PaymentMethod IN ('Cash', 'Transfer', 'Card'))
);

ALTER TABLE dbo.Payments
ADD CONSTRAINT FK_Payments_Invoices
FOREIGN KEY (InvoiceId) REFERENCES dbo.Invoices(Id);

ALTER TABLE dbo.Payments
ADD CONSTRAINT FK_Payments_Users
FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(Id);

-- =============================================
-- 8. AUDIT LOGS
-- =============================================

CREATE TABLE dbo.AuditLogs
(
    Id                  BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TableName           NVARCHAR(100) NOT NULL,
    RecordId            INT NOT NULL,
    ActionType          NVARCHAR(20) NOT NULL,
    OldValuesJson       NVARCHAR(MAX) NULL,
    NewValuesJson       NVARCHAR(MAX) NULL,
    ChangedByUserId     INT NOT NULL,
    ChangedAt           DATETIME2 NOT NULL CONSTRAINT DF_AuditLogs_ChangedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT CK_AuditLogs_ActionType CHECK (ActionType IN ('INSERT', 'UPDATE', 'DELETE'))
);

ALTER TABLE dbo.AuditLogs
ADD CONSTRAINT FK_AuditLogs_Users
FOREIGN KEY (ChangedByUserId) REFERENCES dbo.Users(Id);

-- =============================================
-- Doporučené indexy
-- =============================================

CREATE INDEX IX_Orders_CustomerId ON dbo.Orders(CustomerId);
CREATE INDEX IX_Orders_CreatedByUserId ON dbo.Orders(CreatedByUserId);
CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON dbo.OrderItems(ProductId);

CREATE INDEX IX_Invoices_CustomerId ON dbo.Invoices(CustomerId);
CREATE INDEX IX_Invoices_OrderId ON dbo.Invoices(OrderId);
CREATE INDEX IX_InvoiceItems_InvoiceId ON dbo.InvoiceItems(InvoiceId);

CREATE INDEX IX_Products_CategoryId ON dbo.Products(CategoryId);
CREATE INDEX IX_Products_SupplierId ON dbo.Products(SupplierId);

CREATE INDEX IX_Stock_ProductId ON dbo.Stock(ProductId);

CREATE INDEX IX_StockMovements_StockId ON dbo.StockMovements(StockId);
CREATE INDEX IX_StockMovements_ProductId ON dbo.StockMovements(ProductId);
CREATE INDEX IX_StockMovements_WarehouseId ON dbo.StockMovements(WarehouseId);
CREATE INDEX IX_StockMovements_CreatedAt ON dbo.StockMovements(CreatedAt DESC);
CREATE INDEX IX_StockMovements_MovementType ON dbo.StockMovements(MovementType);
CREATE INDEX IX_StockMovements_ReferenceType_ReferenceId ON dbo.StockMovements(ReferenceType, ReferenceId);

CREATE INDEX IX_Payments_InvoiceId ON dbo.Payments(InvoiceId);

