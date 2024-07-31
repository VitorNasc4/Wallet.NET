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
        Task<Stock?> GetStocksByTickerAsync(string ticker);
        Task<List<Stock>> GetStocksBySearchTermAsync(string query);
        Task<List<Stock>> GetAllStocksAsync();
        Task CreateStockAsync(Stock stock);
        Task UpdateStockAsync(Stock updatedStock);
        Task DeleteStockAsync(int stockId);
        Task AddStockToUserAsync(string userId, int stockId);
        Task RemoveStockTFromUserAsync(string userId, int stockId);
        Task<List<Stock>> GetUserStocksAsync(string userId);
    }
}