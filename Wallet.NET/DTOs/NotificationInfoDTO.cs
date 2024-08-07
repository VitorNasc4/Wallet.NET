using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.NET.DTOs
{
    public class NotificationInfoDTO
    {
        public string Email { get; set; } = null!;
        public List<StockReportInfoDTO> StockReports { get; set; } = new List<StockReportInfoDTO>();
    }
}