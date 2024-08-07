using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.DTOs;
using Wallet.NET.Models;

namespace Wallet.NET.Services.Stocks
{
    public interface IStockService
    {
        Task<StockInfoDTO?> GetStockInfoAsync(string ticker, string exchange);
        Task<bool> IsValidStockAsync(string ticker, string exchange);
        Task<Stock> ValidateAndCreateStockAsync(Stock newStock);
        Task AddStockToUserAsync(string userId, int stockId);
        Task RemoveStockToUserAsync(string userId, int stockId);
        Task<List<Stock>> GetUserStocksAsync(string userId);
        Task<StockReportPriceInfoDTO?> GetStockPriceInfoAsync(Stock stock);
    }
}