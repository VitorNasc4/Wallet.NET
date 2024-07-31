using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.NET.Models
{
    public class UserStock
    {
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public int StockId { get; set; }
        public Stock Stock { get; set; } = null!;
    }
}