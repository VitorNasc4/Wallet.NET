using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.NET.Models
{
    public class Crypto
    {
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string DailyChange { get; set; } = null!;
    }
}