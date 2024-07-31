using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wallet.NET.DTOs;
using Wallet.NET.Models;
using Wallet.NET.Repositories.Stocks;

namespace Wallet.NET.Services.Stocks
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task AddStockToUserAsync(string userId, int stockId)
        {
            await _stockRepository.AddStockToUserAsync(userId, stockId);
        }

        public async Task<StockInfoDTO?> GetStockInfoAsync(string ticker, string exchange)
        {
            string url = exchange switch
            {
                "BOVESPA" => $"https://www.google.com/finance/quote/{ticker}:BVMF",
                "NASDAQ" => $"https://www.google.com/finance/quote/{ticker}:NASDAQ",
                _ => throw new ArgumentException("Invalid market")
            };

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);

                var priceNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'YMlKec fxKbKc')]");
                var openingPriceNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'P6K39c')]");


                if (priceNode is null || openingPriceNode is null)
                {
                    Console.WriteLine("Error on get Price Node and Opening Price Node");
                    return null;
                }

                var priceString = priceNode.InnerText;
                var priceDecimal = ParseStringToDecimal(priceString, exchange);

                var openingPriceString = openingPriceNode.InnerText;
                var openingPriceDecimal = ParseStringToDecimal(openingPriceString, exchange);

                var dailyChange = priceDecimal - openingPriceDecimal;

                return new StockInfoDTO
                {
                    CurrentValue = priceDecimal,
                    DailyChange = dailyChange
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Stock>> GetUserStocksAsync(string userId)
        {
            return await _stockRepository.GetUserStocksAsync(userId);
        }

        public async Task<bool> IsValidStockAsync(string ticker, string exchange)
        {
            var stockInfo = await GetStockInfoAsync(ticker.ToUpperInvariant(), exchange.ToUpperInvariant());

            if (stockInfo is null)
            {
                return false;
            }
            return true;
        }

        public async Task RemoveStockToUserAsync(string userId, int stockId)
        {
            await _stockRepository.RemoveStockTFromUserAsync(userId, stockId);
        }

        public async Task<Stock> ValidateAndCreateStockAsync(Stock newStock)
        {
            var isValidExchange = Stock.IsValidExchange(newStock.Exchange);
            if (!isValidExchange)
            {
                throw new Exception("Exchange not valid");
            }

            var isValidStock = await IsValidStockAsync(newStock.Ticker, newStock.Exchange);
            if (!isValidStock)
            {
                throw new Exception("Ticker not found for this Exchange");
            }

            var existingStock = await _stockRepository.GetStocksByTickerAsync(newStock.Ticker);
            if (existingStock == null)
            {
                await _stockRepository.CreateStockAsync(newStock);

                var registeredStock = await _stockRepository.GetStocksByTickerAsync(newStock.Ticker);
                return registeredStock is not null ? registeredStock : newStock;
            }

            return existingStock;
        }

        private decimal ParseStringToDecimal(string price, string exchange)
        {
            string country = exchange switch
            {
                "BOVESPA" => $"pt-BR",
                "NASDAQ" => $"en-US",
                _ => throw new ArgumentException("Invalid market")
            };

            string currency = exchange switch
            {
                "BOVESPA" => $"R$",
                "NASDAQ" => $"$",
                _ => throw new ArgumentException("Invalid market")
            };

            var culture = new CultureInfo(country);

            string priceWithoutCurrency = price.Replace(currency, "").Trim();

            decimal priceDecimal = decimal.Parse(priceWithoutCurrency, culture);

            if (exchange == "BOVESPA")
            {
                priceDecimal = priceDecimal / 100;
            }

            return priceDecimal;
        }
    }
}