using System.Data;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Reports;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _dbContext;

    public ReportService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Souhrn obchodních dat za zadané období
    public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime dateFrom, DateTime dateTo)
    {
        // list obch. dat + dat. připojení
        var result = new SalesSummaryDto();
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu --  pamatovat si! 
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro proceduru
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

        // reader
        await using var reader = await command.ExecuteReaderAsync();

        // Načtení prvního řádku výsledku
        if (await reader.ReadAsync())
        {
            result.TotalOrders = reader.GetInt32(reader.GetOrdinal("TotalOrders"));
            result.ConfirmedOrders = reader.GetInt32(reader.GetOrdinal("ConfirmedOrders"));
            result.TotalInvoices = reader.GetInt32(reader.GetOrdinal("TotalInvoices"));
            result.PaidInvoices = reader.GetInt32(reader.GetOrdinal("PaidInvoices"));
            result.TotalInvoiceAmount = reader.GetDecimal(reader.GetOrdinal("TotalInvoiceAmount"));
            result.TotalPaidAmount = reader.GetDecimal(reader.GetOrdinal("TotalPaidAmount"));
            result.RemainingAmount = reader.GetDecimal(reader.GetOrdinal("RemainingAmount"));
        }
        return result;
    }

    // Přehled neuhrazených faktur
    public async Task<List<UnpaidInvoiceDto>> GetUnpaidInvoicesAsync()
    {
        // Seznam neuhrazených faktur + dat. připojení
        var result = new List<UnpaidInvoiceDto>();
        var connection = _dbContext.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Databázový příkaz pro proceduru
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_GetUnpaidInvoices";
        command.CommandType = CommandType.StoredProcedure;

        await using var reader = await command.ExecuteReaderAsync();

        // Načítání jednotlivých řádků výsledku
        while (await reader.ReadAsync())
        {
            var item = new UnpaidInvoiceDto
            {
                InvoiceNumber = reader["InvoiceNumber"]?.ToString() ?? string.Empty,
                CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]),
                IsOverdue = Convert.ToBoolean(reader["IsOverdue"])
            };
            result.Add(item);
        }
        return result;
    }

    // Vrací přehled skladových upozornění
    public async Task<List<StockAlertDto>> GetStockAlertsAsync()
    {
        // List skladových upozornění + dat. připojení
        var result = new List<StockAlertDto>();
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

        // reader
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var item = new StockAlertDto
            {
                ProductCode = reader["ProductCode"]?.ToString() ?? string.Empty,
                ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                WarehouseCode = reader["WarehouseCode"]?.ToString() ?? string.Empty,
                WarehouseName = reader["WarehouseName"]?.ToString() ?? string.Empty,
                Quantity = Convert.ToDecimal(reader["Quantity"]),
                ReservedQuantity = Convert.ToDecimal(reader["ReservedQuantity"]),
                MinimumStock = Convert.ToDecimal(reader["MinimumStock"]),
                AvailableQuantity = Convert.ToDecimal(reader["AvailableQuantity"]),
                StockStatus = reader["StockStatus"]?.ToString() ?? string.Empty
            };
            result.Add(item);
        }
        return result;
    }
    // Obchodní výsledky podle zákazníků za zadané období
    public async Task<List<SalesByCustomerDto>> GetSalesByCustomerAsync(DateTime dateFrom, DateTime dateTo)
    {
        // List reportu podle zákazníků + dat. připojení
        var result = new List<SalesByCustomerDto>();
        var connection = _dbContext.Database.GetDbConnection();


        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro proceduru
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

        while (await reader.ReadAsync())
        {
            var item = new SalesByCustomerDto
            {
                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,
                TotalOrders = Convert.ToInt32(reader["TotalOrders"]),
                TotalInvoices = Convert.ToInt32(reader["TotalInvoices"]),
                TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
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
        // List reportu produktů + dat. připojení
        var result = new List<TopSellingProductDto>();
        var connection = _dbContext.Database.GetDbConnection();

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

        // reader
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var item = new TopSellingProductDto
            {
                ProductId = Convert.ToInt32(reader["ProductId"]),
                ProductCode = reader["ProductCode"]?.ToString() ?? string.Empty,
                ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                TotalQuantitySold = Convert.ToDecimal(reader["TotalQuantitySold"]),
                OrderCount = Convert.ToInt32(reader["OrderCount"]),
                TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
            };

            // Přidání položky do výsledného seznamu
            result.Add(item);
        }

        // Vrácenílistu reportu produktů
        return result;
    }

}