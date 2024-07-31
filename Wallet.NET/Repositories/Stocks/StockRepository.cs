using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.NET.Data;
using Wallet.NET.Models;

namespace Wallet.NET.Repositories.Stocks
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateStockAsync(Stock stock)
        {
            if (stock == null)
            {
                throw new ArgumentNullException(nameof(stock));
            }

            stock.Ticker = stock.Ticker.ToUpperInvariant();
            stock.Exchange = stock.Exchange.ToUpperInvariant();

            var existingStock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.Ticker == stock.Ticker);

            if (existingStock != null)
            {
                throw new InvalidOperationException($"A stock with ticker '{stock.Ticker}' already exists.");
            }

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStockAsync(int stockId)
        {
            var stock = await _context.Stocks.FindAsync(stockId);

            if (stock == null)
            {
                throw new KeyNotFoundException($"Stock with id {stockId} not found.");
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int stockId)
        {
            return await _context.Stocks.Where(s => s.Id == stockId).FirstOrDefaultAsync();
        }

        public async Task<Stock?> GetStocksByTickerAsync(string ticker)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(s => s.Ticker == ticker.ToUpperInvariant());
        }
        public async Task<List<Stock>> GetStocksBySearchTermAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return await _context.Stocks
                    .AsNoTracking()
                    .ToListAsync();
            }

            return await _context.Stocks
                .Where(s => s.Ticker.Contains(query.ToUpperInvariant()) || s.Exchange.Contains(query.ToUpperInvariant()))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateStockAsync(Stock updatedStock)
        {
            if (updatedStock == null)
            {
                throw new ArgumentNullException(nameof(updatedStock));
            }

            var existingStock = await _context.Stocks.FindAsync(updatedStock.Id);

            if (existingStock == null)
            {
                throw new KeyNotFoundException($"Stock with id {updatedStock.Id} not found.");
            }

            existingStock.Ticker = updatedStock.Ticker.ToUpperInvariant();
            existingStock.Exchange = updatedStock.Exchange.ToUpperInvariant();

            _context.Stocks.Update(existingStock);
            await _context.SaveChangesAsync();
        }

        public async Task AddStockToUserAsync(string userId, int stockId)
        {
            var userStock = new UserStock
            {
                UserId = userId,
                StockId = stockId
            };

            _context.UserStocks.Add(userStock);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveStockTFromUserAsync(string userId, int stockId)
        {
            var userStock = await _context.UserStocks
                .FirstOrDefaultAsync(us => us.UserId == userId && us.StockId == stockId);

            if (userStock is null)
            {
                return;
            }
            _context.UserStocks.Remove(userStock);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Stock>> GetUserStocksAsync(string userId)
        {
            return await _context.UserStocks
                .Where(us => us.UserId == userId)
                .Select(us => us.Stock)
                .ToListAsync();
        }
    }
}