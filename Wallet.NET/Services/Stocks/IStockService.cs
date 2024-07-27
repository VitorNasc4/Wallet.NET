using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.DTOs;

namespace Wallet.NET.Services.Stocks
{
    public interface IStockService
    {
        Task<StockInfoDTO?> GetStockInfoAsync(string ticker, string exchange);
        Task<bool> IsValidStock(string ticker, string exchange);
    }
}