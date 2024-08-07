using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.Models;

namespace Wallet.NET.DTOs
{
    public class StockReportInfoDTO
    {
        public string Ticker { get; set; } = null!;
        public StockReportPriceInfoDTO PriceInfo { get; set; } =null!;
        public  List<NewsArticle>? NewsArticles { get; set; } = new List<NewsArticle>();   
    }
    public class StockReportPriceInfoDTO
    {
        public string? Variation { get; set; }
        public string? CurrentPrice { get; set; }
    }
}