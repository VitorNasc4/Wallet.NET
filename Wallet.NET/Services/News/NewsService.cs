using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wallet.NET.Models;

namespace Wallet.NET.Services.News
{
    public class NewsService : INewsService
    {
        public async Task<List<NewsArticle>?> GetAllNewsArticlesAsync()
        {
            var url = "https://www.google.com/finance/?hl=en";

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);

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


                if (string.IsNullOrEmpty(newsTittle) || string.IsNullOrEmpty(newsLink))
                {
                    continue;
                }

                var news = new NewsArticle
                {
                    Title = newsTittle,
                    Link = newsLink,
                };

                newsList.Add(news);
            }

            if(newsList.Count == 0)
            {
                throw new Exception("Could not get News informations");
            }

            return newsList;
        }
    }
}