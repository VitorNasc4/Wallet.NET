using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wallet.NET.Models;

namespace Wallet.NET.Services.Indices
{
    public class IndexService : IIndexService
    {
        public async Task<List<IndexInfo>> GetIndicesInfoAsync()
        {
            var url = "https://www.google.com/finance/markets/indexes";

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var indecesNodes = htmlDocument.DocumentNode.SelectSingleNode("//ul[contains(@class,'sbnBtf')]");

            if (indecesNodes is null)
            {
                throw new Exception("Could not get Indexes Nodes");
            }
            
            var indexesList = new List<IndexInfo>();

            foreach (var li in indecesNodes.SelectNodes(".//li"))
            {

                var indexNameNode = li.SelectSingleNode(".//a//div[1]//div[1]//div[1]//div[2]//div[1]");
                var indexName = indexNameNode != null ? WebUtility.HtmlDecode(indexNameNode.InnerText.Trim()) : string.Empty;

                var indexValueNode = li.SelectSingleNode(".//a//div[1]//div[1]//div[2]//span[1]//div[1]//div[1]");
                var indexValue = indexValueNode != null ? indexValueNode.InnerText.Trim() : string.Empty;

                var indexDailyChangeNode = li.SelectSingleNode(".//a//div[1]//div[1]//div[1]//span[1]");
                var indexDailyChange = indexDailyChangeNode != null ? indexDailyChangeNode.InnerText.Trim() : string.Empty;

                if (string.IsNullOrEmpty(indexName) || string.IsNullOrEmpty(indexValue) || string.IsNullOrEmpty(indexDailyChange))
                {
                    throw new Exception("Could not get Indexes informations");
                }

                var index = new IndexInfo
                {
                    Name = indexName,
                    Value = indexValue,
                    DailyChange = indexDailyChange,
                };

                indexesList.Add(index);
            }

            return indexesList;
        }
    }
}