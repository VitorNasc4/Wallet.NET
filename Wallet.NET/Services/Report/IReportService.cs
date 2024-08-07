using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Wallet.NET.Models;

namespace Wallet.NET.Services.Report
{
    public interface IReportService
    {
        Task GenerateEmailReportAsync(IdentityUser user);
    }
}