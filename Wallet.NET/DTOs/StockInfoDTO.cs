using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.NET.DTOs
{
    public class StockInfoDTO
    {
        public string? OpeningValue { get; set; }
        public string? CurrentValue { get; set; }
        public decimal DailyChange { get; set; }
        public string? Variation { get; set; }
    }
}