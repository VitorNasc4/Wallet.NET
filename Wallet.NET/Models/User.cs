using Wallet.NET.Data;
using Wallet.NET.Models;

namespace Wallet.NET.Models
{
    public class User : ApplicationUser
    {
        public string Name { get; set; } = null!;
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
