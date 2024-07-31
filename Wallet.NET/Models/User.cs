using Wallet.NET.Data;
using Wallet.NET.Models;

namespace Wallet.NET.Models
{
    public class User : ApplicationUser
    {
        public ICollection<UserStock> UserStocks { get; set; } = new List<UserStock>();
    }
}
