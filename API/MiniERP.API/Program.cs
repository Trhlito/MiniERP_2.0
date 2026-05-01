using FluentValidation;                                 // knihovna pro validaci vstupních modelů
using FluentValidation.AspNetCore;                      // integrace FluentValidation do ASP.NET Core pipeline
using Microsoft.AspNetCore.Authentication.JwtBearer;    // podpora JWT autentizace
using Microsoft.AspNetCore.Identity;                    // ASP.NET Identity pro správu uživatelů a rolí
using Microsoft.EntityFrameworkCore;                    // ORM pro práci s databází
using Microsoft.IdentityModel.Tokens;                   // validace a práce s bezpečnostními tokeny
using MiniERP.API.Seed;                                 // seedování výchozích dat při startu aplikace
using MiniERP.API.Services.Implementations;             // implementace aplikačních služeb
using MiniERP.API.Services.Interfaces;                  // rozhraní aplikačních služeb
using MiniERP.API.Validators.Customers;                 // validátory pro zákazníky
using MiniERP.Data;                                     // databázový kontext aplikace
using MiniERP.Data.Entities.Auth;                       // entity pro autentizaci a autorizaci
using System.Text;                                      // pro práci s textem a kódováním
using Microsoft.OpenApi.Models;                         // konfigurace Swaggeru a OpenAPI

var builder = WebApplication.CreateBuilder(args);

// Konfigurace databázového připojení
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfigurace ASP.NET Identity a bezpečnostních pravidel
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;

        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Konfigurace JWT autentizace a validace tokenu
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

// Registrace aplikačních služeb pro jednotlivé moduly ERP
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Registrace bezpečnostních a uživatelských služeb
builder.Services.AddScoped<ISecurityReportService, SecurityReportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Přidání controllerů a validační vrstvy
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

// Konfigurace Swaggeru včetně podpory JWT autorizace
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Zadej JWT token ve formátu: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Zapnutí Swaggeru pouze v development prostředí
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware pipeline pro zabezpečení a routování
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicializace základních Identity dat při spuštění aplikace
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
}

app.Run();