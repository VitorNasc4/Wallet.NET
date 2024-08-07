using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Wallet.NET.Models;

namespace Wallet.NET.Services.News
{
    public class NewsService : INewsService
    {
        private readonly IMemoryCache _cache;

        public NewsService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<List<NewsArticle>?> GetAllNewsArticlesAsync()
        {
            var url = "https://www.google.com/finance/?hl=en";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
            var response = await client.GetStringAsync(url);

            await Task.Delay(3000);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var newsNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'yY3Lee')]");

            if (newsNodes is null)
            {
                throw new Exception("Could not get News Nodes");
            }

            var newsList = new List<NewsArticle>();

            foreach (var node in newsNodes)
            {
                var newsTittleNode = node.SelectSingleNode(".//div[@class='Yfwt5']");
                var newsTittle = newsTittleNode != null ? WebUtility.HtmlDecode(newsTittleNode.InnerText.Trim()) : string.Empty;

                var newsLinkNode = node.SelectSingleNode(".//div[1]//div[1]//a");
                var newsLink = newsLinkNode != null ? newsLinkNode.GetAttributeValue("href", string.Empty) : string.Empty;

                var fontNode = node.SelectSingleNode(".//div[@class='sfyJob']");
                var font = fontNode != null ? WebUtility.HtmlDecode(fontNode.InnerText.Trim()) : string.Empty;

                if (string.IsNullOrEmpty(newsTittle) || string.IsNullOrEmpty(newsLink))
                {
                    continue;
                }

                var news = new NewsArticle
                {
                    Title = newsTittle,
                    Font = font is not null ? font : "",
                    Link = newsLink,
                };

                newsList.Add(news);
            }

            if (newsList.Count == 0)
            {
                throw new Exception("Could not get News informations");
            }

            return newsList;
        }

        public async Task<List<NewsArticle>?> GetAllNewsArticlesWithCacheAsync()
        {

            var cacheKey = "googleFinanceNews";
            
            if (!_cache.TryGetValue(cacheKey, out List<NewsArticle>? newsList))
            {
                newsList = await GetAllNewsArticlesAsync();

                if (newsList is null || newsList.Count == 0)
                {
                    throw new Exception("Could not get News informations");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                };

                _cache.Set(cacheKey, newsList, cacheEntryOptions);
            }
            return newsList;
        }

        public async Task<List<NewsArticle>?> GetStockNewsInfoAsync(Stock stock)
        {
            string url = stock.Exchange switch
            {
                "BOVESPA" => $"https://www.google.com/finance/quote/{stock.Ticker}:BVMF?window=5D",
                "NASDAQ" => $"https://www.google.com/finance/quote/{stock.Ticker}:NASDAQ?window=5D",
                _ => throw new ArgumentException("Invalid market")
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
            var response = await client.GetStringAsync(url);

            await Task.Delay(3000);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var newsNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'yY3Lee')]");

            if (newsNodes is null)
            {
                Console.WriteLine($"Error on get news from {stock.Ticker}");
                return null;
            }

            var newsList = new List<NewsArticle>();

            foreach (var node in newsNodes)
            {
                var newsTittleNode = node.SelectSingleNode(".//div[@class='Yfwt5']");
                var newsTittle = newsTittleNode != null ? WebUtility.HtmlDecode(newsTittleNode.InnerText.Trim()) : string.Empty;

                var newsLinkNode = node.SelectSingleNode(".//div[1]//div[1]//a");
                var newsLink = newsLinkNode != null ? newsLinkNode.GetAttributeValue("href", string.Empty) : string.Empty;

                var fontNode = node.SelectSingleNode(".//div[@class='sfyJob']");
                var font = fontNode != null ? WebUtility.HtmlDecode(fontNode.InnerText.Trim()) : string.Empty;

                if (string.IsNullOrEmpty(newsTittle) || string.IsNullOrEmpty(newsLink))
                {
                    continue;
                }

                var news = new NewsArticle
                {
                    Title = newsTittle,
                    Font = font is not null ? font : "",
                    Link = newsLink,
                };

                newsList.Add(news);
            }

            if (newsList.Count == 0)
            {
                Console.WriteLine($"Could not get News informations from {stock.Ticker}");
                return null;
            }

            return newsList;

        }
    }
}