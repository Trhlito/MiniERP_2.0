using Microsoft.EntityFrameworkCore;
using MiniERP.API.Services.Implementations;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using MiniERP.API.Validators.Customers;

var builder = WebApplication.CreateBuilder(args);

// -- Registrace CustomerService pro dependency injection --
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Registrace ProductService pro dependency injection --
builder.Services.AddScoped<IProductService, ProductService>();

// Registrace OrderService pro dependency injection --
builder.Services.AddScoped<IOrderService, OrderService>();

// Registrace StockService pro dependency injection --
builder.Services.AddScoped<IStockService, StockService>();

// Registrace StockMovementService 
builder.Services.AddScoped<IStockMovementService, StockMovementService>();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
    
// -- Přidání podpory pro controllery --
builder.Services.AddControllers();

// -- Zapnutí FluentValidation auto-validace --
builder.Services.AddFluentValidationAutoValidation();

// -- Registrace všech validatorů z assembly --
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();


// -- Přidání Swaggeru pro testování API --
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// -- Registrace databázového kontextu s connection stringem z konfigurace --
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// -- Registrace CustomerService pro dependency injection --
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// -- Zapnutí Swaggeru v development prostředí --
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -- Přesměrování na HTTPS --
app.UseHttpsRedirection();

// -- Zapnutí autorizace --
app.UseAuthorization();

// -- Mapování controllerů --
app.MapControllers();

app.Run();