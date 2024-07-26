using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.Models;

namespace Wallet.NET.Repositories.Stocks
{
    public interface IStockRepository
    {
        Task<Stock?> GetStockByIdAsync(int stockId);
        Task<List<Stock>> GetStocksBySearchTermAsync(string query);
        Task<List<Stock>> GetAllStocksAsync();
        Task CreateStockAsync(Stock stock);
        Task UpdateStockAsync(Stock updatedStock);
        Task DeleteStockAsync(int stockId);
    }
}