using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wallet.NET.Models;

namespace Wallet.NET.Services.Indices
{
    public class CryptoService : ICryptoService
    {
        public async Task<List<Crypto>> GetCryptoInfoAsync()
        {
            var url = "https://www.google.com/finance/markets/cryptocurrencies";

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var cryptosNodes = htmlDocument.DocumentNode.SelectSingleNode("//ul[contains(@class,'sbnBtf')]");

            if (cryptosNodes is null)
            {
                throw new Exception("Could not get Crypto Nodes");
            }
            
            var cryptosList = new List<Crypto>();

            foreach (var li in cryptosNodes.SelectNodes(".//li"))
            {

                // var cryptoNameNode = li.SelectSingleNode("//div[contains(@class,'ZvmM7')]");
                var cryptoNameNode = li.SelectSingleNode(".//a//div[1]//div[1]//div[1]//div[2]//div[1]");
                var cryptoName = cryptoNameNode != null ? WebUtility.HtmlDecode(cryptoNameNode.InnerText.Trim()) : string.Empty;

                // var cryptoValueNode = li.SelectSingleNode("//div[contains(@class,'YMlKec')]");
                var cryptoValueNode = li.SelectSingleNode(".//a//div[1]//div[1]//div[2]//span[1]//div[1]//div[1]");
                var cryptoValue = cryptoValueNode != null ? cryptoValueNode.InnerText.Trim() : string.Empty;

                // var cryptoDailyChangeNode = li.SelectSingleNode("//span[contains(@class,'P2Luy Ebnabc')]");
                var cryptoDailyChangeNode = li.SelectSingleNode(".//a//div[1]//div[1]//div[3]//div[1]//div[1]//span[1]");
                var cryptoDailyChange = cryptoDailyChangeNode != null ? cryptoDailyChangeNode.InnerText.Trim() : string.Empty;

                if (string.IsNullOrEmpty(cryptoName) || string.IsNullOrEmpty(cryptoValue) || string.IsNullOrEmpty(cryptoDailyChange))
                {
                    continue;
                }

                var crypto = new Crypto
                {
                    Name = cryptoName,
                    Value = cryptoValue,
                    DailyChange = cryptoDailyChange,
                };

                cryptosList.Add(crypto);
                if (cryptosList.Count == 6) return cryptosList;
            }

            if (cryptosList.Count == 0) throw new Exception("Could not get Crypto informations");
            return cryptosList;
        }
    }
}