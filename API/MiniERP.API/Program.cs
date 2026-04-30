using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniERP.API.Seed;
using MiniERP.API.Services.Implementations;
using MiniERP.API.Services.Interfaces;
using MiniERP.API.Validators.Customers;
using MiniERP.Data;
using MiniERP.Data.Entities.Auth;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Registrace databázového kontextu
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrace ASP.NET Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        // Nastavení pravidel pro heslo
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        // Nastavení pravidel pro uživatele
        options.User.RequireUniqueEmail = true;

        // Nastavení blokace po neúspěšných pokusech
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // Registrace JWT autentizace
builder.Services
    .AddAuthentication(options =>
    {
        // Výchozí schéma autentizace
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Načtení JWT konfigurace
        var jwtSection = builder.Configuration.GetSection("Jwt");

        // Nastavení pravidel pro ověřování JWT tokenu
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ověření vydavatele tokenu
            ValidateIssuer = true,

            // Ověření publika tokenu
            ValidateAudience = true,

            // Ověření podpisového klíče
            ValidateIssuerSigningKey = true,

            // Ověření expirace tokenu
            ValidateLifetime = true,

            // Platný issuer
            ValidIssuer = jwtSection["Issuer"],

            // Platné audience
            ValidAudience = jwtSection["Audience"],

            // Tajný podpisový klíč
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

// Registrace služeb pro dependency injection
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Registrace služby pro auth reporty
builder.Services.AddScoped<IAuthReportService, AuthReportService>();

// Registrace služby pro správu uživatelů
builder.Services.AddScoped<IUserService, UserService>();

// Registrace autentizační služby
builder.Services.AddScoped<IAuthService, AuthService>();

// Přidání podpory pro controllery
builder.Services.AddControllers();

// Zapnutí FluentValidation auto-validace
builder.Services.AddFluentValidationAutoValidation();

// Registrace validátorů z assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

// Přidání Swaggeru pro testování API
builder.Services.AddEndpointsApiExplorer();
// Přidání Swaggeru s podporou JWT autorizace
builder.Services.AddSwaggerGen(options =>
{
    // Definice Bearer tokenu pro Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Zadej JWT token ve formátu: Bearer {token}"
    });

    // Nastavení požadavku na Bearer token
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

// Zapnutí Swaggeru v development prostředí
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Přesměrování na HTTPS
app.UseHttpsRedirection();

// Zapnutí autentizace
app.UseAuthentication();

// Zapnutí autorizace
app.UseAuthorization();

// Mapování controllerů
app.MapControllers();

// Vytvoření základních Identity dat
using (var scope = app.Services.CreateScope())
{
    // Spuštění seedování rolí a výchozího admin účtu
    await IdentitySeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
}

app.Run();