using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class StockInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ticker is required")]
        [MaxLength(ErrorMessage = "The maximum length of the ticker is 10 characters.")]
        public string Ticker { get; set; } = null!;

        [Required(ErrorMessage = "Exchange is required")]
        [MaxLength(ErrorMessage = "The maximum length of the exchange is 20 characters.")]
        public string Exchange { get; set; } = null!;
    }
}