using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wallet.NET.DTOs;

namespace Wallet.NET.Services.Stocks
{
    public class StockService : IStockService
    {
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

        public async Task<bool> IsValidStock(string ticker, string exchange)
        {
            var stockInfo = await GetStockInfoAsync(ticker.ToUpperInvariant(), exchange.ToUpperInvariant());

            if (stockInfo is null)
            {
                return false;
            }
            return true;
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