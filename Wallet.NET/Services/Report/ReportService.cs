using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Wallet.NET.DTOs;
using Wallet.NET.Models;
using Wallet.NET.Services.Email;
using Wallet.NET.Services.News;
using Wallet.NET.Services.Stocks;

namespace Wallet.NET.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IStockService _stockService;
        private readonly INewsService _newsService;
        private readonly IEmailService _emailService;
        public ReportService(IStockService stockService, INewsService newsService, IEmailService emailService)
        {
            _stockService = stockService;
            _newsService = newsService;
            _emailService = emailService;
        }

        public Task DownloadReportAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task GenerateEmailReportAsync(IdentityUser user)
        {
            var notificationInfo = new NotificationInfoDTO
            {
                Email =  user.Email!
            };

            var stocks = await _stockService.GetUserStocksAsync(user.Id);

            var stockReportList = new List<StockReportInfoDTO>();
            foreach (var stock in stocks)
            {
                var priceInfo = await _stockService.GetStockPriceInfoAsync(stock);

                if (priceInfo is null)
                {
                    continue;
                }

                var newsInfo = await _newsService.GetStockNewsInfoAsync(stock);

                if (newsInfo is null)
                {
                    continue;
                }

                var stockReportInfoDTO = new StockReportInfoDTO
                {
                    Ticker = stock.Ticker,
                    PriceInfo = priceInfo,
                    NewsArticles = newsInfo
                };

                stockReportList.Add(stockReportInfoDTO);
            }

            if (!stockReportList.Any())
            {
                Console.WriteLine("Error on get report for user");
                return;
            }

            notificationInfo.StockReports = stockReportList;

            await _emailService.SendEmail(notificationInfo);
        }
    }
}