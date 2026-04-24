using System.Data;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Reports;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

// Služba pro načítání reportovacích dat z databáze
public class ReportService : IReportService
{
    // Databázový kontext
    private readonly ApplicationDbContext _dbContext;

    // Konstruktor pro injekci databázového kontextu
    public ReportService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Vrací souhrn obchodních dat za zadané období
    public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime dateFrom, DateTime dateTo)
    {
        // Výsledné DTO reportu
        var result = new SalesSummaryDto();

        // Databázové připojení z EF Core kontextu
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro stored procedure
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_GetSalesSummary";
        command.CommandType = CommandType.StoredProcedure;

        // Parametr počátečního data
        var dateFromParameter = command.CreateParameter();
        dateFromParameter.ParameterName = "@DateFrom";
        dateFromParameter.Value = dateFrom;
        command.Parameters.Add(dateFromParameter);

        // Parametr koncového data
        var dateToParameter = command.CreateParameter();
        dateToParameter.ParameterName = "@DateTo";
        dateToParameter.Value = dateTo;
        command.Parameters.Add(dateToParameter);

        // Spuštění readeru nad výsledkem procedury
        await using var reader = await command.ExecuteReaderAsync();

        // Načtení prvního řádku výsledku
        if (await reader.ReadAsync())
        {
            // Celkový počet objednávek
            result.TotalOrders = reader.GetInt32(reader.GetOrdinal("TotalOrders"));

            // Počet potvrzených objednávek
            result.ConfirmedOrders = reader.GetInt32(reader.GetOrdinal("ConfirmedOrders"));

            // Celkový počet faktur
            result.TotalInvoices = reader.GetInt32(reader.GetOrdinal("TotalInvoices"));

            // Počet uhrazených faktur
            result.PaidInvoices = reader.GetInt32(reader.GetOrdinal("PaidInvoices"));

            // Celková hodnota faktur
            result.TotalInvoiceAmount = reader.GetDecimal(reader.GetOrdinal("TotalInvoiceAmount"));

            // Celková hodnota přijatých plateb
            result.TotalPaidAmount = reader.GetDecimal(reader.GetOrdinal("TotalPaidAmount"));

            // Zbývající částka k úhradě
            result.RemainingAmount = reader.GetDecimal(reader.GetOrdinal("RemainingAmount"));
        }

        // Vrácení naplněného DTO
        return result;
    }

    // Vrací přehled neuhrazených faktur
    public async Task<List<UnpaidInvoiceDto>> GetUnpaidInvoicesAsync()
    {
        // Výsledný seznam neuhrazených faktur
        var result = new List<UnpaidInvoiceDto>();

        // Databázové připojení z EF Core kontextu
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro stored procedure
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_GetUnpaidInvoices";
        command.CommandType = CommandType.StoredProcedure;

        // Spuštění readeru nad výsledkem procedury
        await using var reader = await command.ExecuteReaderAsync();

        // Načítání jednotlivých řádků výsledku
        while (await reader.ReadAsync())
        {
            // Jedna položka reportu
            var item = new UnpaidInvoiceDto
            {
                //Obsah
                InvoiceNumber = reader["InvoiceNumber"]?.ToString() ?? string.Empty,
                CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]),
                IsOverdue = Convert.ToBoolean(reader["IsOverdue"])
            };

            // Přidání položky do výsledného seznamu
            result.Add(item);
        }

        // Vrácení seznamu neuhrazených faktur
        return result;
    }

    // Vrací přehled skladových upozornění
    public async Task<List<StockAlertDto>> GetStockAlertsAsync()
    {
        // Výsledný seznam skladových upozornění
        var result = new List<StockAlertDto>();

        // Databázové připojení z EF Core kontextu
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro stored procedure
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_GetStockAlerts";
        command.CommandType = CommandType.StoredProcedure;

        // Spuštění readeru nad výsledkem procedury
        await using var reader = await command.ExecuteReaderAsync();

        // Načítání jednotlivých řádků výsledku
        while (await reader.ReadAsync())
        {
            // Jedna položka reportu
            var item = new StockAlertDto
            {
                // Kód produktu
                ProductCode = reader["ProductCode"]?.ToString() ?? string.Empty,

                // Název produktu
                ProductName = reader["ProductName"]?.ToString() ?? string.Empty,

                // Kód skladu
                WarehouseCode = reader["WarehouseCode"]?.ToString() ?? string.Empty,

                // Název skladu
                WarehouseName = reader["WarehouseName"]?.ToString() ?? string.Empty,

                // Aktuální množství na skladě
                Quantity = Convert.ToDecimal(reader["Quantity"]),

                // Rezervované množství
                ReservedQuantity = Convert.ToDecimal(reader["ReservedQuantity"]),

                // Minimální skladová zásoba
                MinimumStock = Convert.ToDecimal(reader["MinimumStock"]),

                // Disponibilní množství
                AvailableQuantity = Convert.ToDecimal(reader["AvailableQuantity"]),

                // Stav skladu
                StockStatus = reader["StockStatus"]?.ToString() ?? string.Empty
            };

            // Přidání položky do výsledného seznamu
            result.Add(item);
        }

        // Vrácení seznamu skladových upozornění
        return result;
    }
    // Vrací obchodní výsledky podle zákazníků za zadané období
    public async Task<List<SalesByCustomerDto>> GetSalesByCustomerAsync(DateTime dateFrom, DateTime dateTo)
    {
        // Výsledný seznam reportu podle zákazníků
        var result = new List<SalesByCustomerDto>();

        // Databázové připojení z EF Core kontextu
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro stored procedure
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_GetSalesByCustomer";
        command.CommandType = CommandType.StoredProcedure;

        // Parametr počátečního data
        var dateFromParameter = command.CreateParameter();
        dateFromParameter.ParameterName = "@DateFrom";
        dateFromParameter.Value = dateFrom;
        command.Parameters.Add(dateFromParameter);

        // Parametr koncového data
        var dateToParameter = command.CreateParameter();
        dateToParameter.ParameterName = "@DateTo";
        dateToParameter.Value = dateTo;
        command.Parameters.Add(dateToParameter);

        // Spuštění readeru nad výsledkem procedury
        await using var reader = await command.ExecuteReaderAsync();

        // Načítání jednotlivých řádků výsledku
        while (await reader.ReadAsync())
        {
            // Jedna položka reportu
            var item = new SalesByCustomerDto
            {
                // ID zákazníka
                CustomerId = Convert.ToInt32(reader["CustomerId"]),

                // Název zákazníka
                CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,

                // Počet objednávek
                TotalOrders = Convert.ToInt32(reader["TotalOrders"]),

                // Počet faktur
                TotalInvoices = Convert.ToInt32(reader["TotalInvoices"]),

                // Celková fakturovaná částka
                TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]),

                // Celková uhrazená částka
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),

                // Zbývající částka k úhradě
                RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"])
            };

            // Přidání položky do výsledného seznamu
            result.Add(item);
        }

        // Vrácení seznamu reportu podle zákazníků
        return result;
    }

    // Vrací přehled nejprodávanějších produktů za zadané období
    public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime dateFrom, DateTime dateTo)
    {
        // Výsledný seznam reportu produktů
        var result = new List<TopSellingProductDto>();

        // Databázové připojení z EF Core kontextu
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro stored procedure
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_GetTopSellingProducts";
        command.CommandType = CommandType.StoredProcedure;

        // Parametr počátečního data
        var dateFromParameter = command.CreateParameter();
        dateFromParameter.ParameterName = "@DateFrom";
        dateFromParameter.Value = dateFrom;
        command.Parameters.Add(dateFromParameter);

        // Parametr koncového data
        var dateToParameter = command.CreateParameter();
        dateToParameter.ParameterName = "@DateTo";
        dateToParameter.Value = dateTo;
        command.Parameters.Add(dateToParameter);

        // Spuštění readeru nad výsledkem procedury
        await using var reader = await command.ExecuteReaderAsync();

        // Načítání jednotlivých řádků výsledku
        while (await reader.ReadAsync())
        {
            // Jedna položka reportu
            var item = new TopSellingProductDto
            {
                // ID produktu
                ProductId = Convert.ToInt32(reader["ProductId"]),

                // Kód produktu
                ProductCode = reader["ProductCode"]?.ToString() ?? string.Empty,

                // Název produktu
                ProductName = reader["ProductName"]?.ToString() ?? string.Empty,

                // Celkové prodané množství
                TotalQuantitySold = Convert.ToDecimal(reader["TotalQuantitySold"]),

                // Počet objednávek s produktem
                OrderCount = Convert.ToInt32(reader["OrderCount"]),

                // Celková tržba za produkt
                TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
            };

            // Přidání položky do výsledného seznamu
            result.Add(item);
        }

        // Vrácení seznamu reportu produktů
        return result;
    }

}