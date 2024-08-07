using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.NET.Data;
using Wallet.NET.Services.Report;

namespace Wallet.NET.Services.Monitor
{
    public class MonitorService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MonitorService(IServiceScopeFactory serviceScopeFactory)
        {

            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await AddJobHangFire();
        }

        private async Task AddJobHangFire()
        {
            RecurringJob.AddOrUpdate(
                "EmailReportWeeklyJob",
                () => SendReportAsync(),
                "0 5 * * 6");

            await Task.CompletedTask;
        }

        public async Task SendReportAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var users = await userManager.Users.ToListAsync();
                foreach (var user in users)
                {
                    await reportService.GenerateEmailReportAsync(user);
                }
            }
        }
    }
}