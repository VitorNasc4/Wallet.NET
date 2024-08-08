using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = null!;
        public string Exchange { get; set; } = null!;
        public string OpeningValue { get; set; } = null!;
        public string CurrentValue { get; set; } = null!;
        public string DailyChange { get; set; } = null!;
        public string Variation { get; set; } = null!;
    }
}