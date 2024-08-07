using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Wallet.NET.DTOs;

namespace Wallet.NET.Services.Email
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(NotificationInfoDTO notificationInfoDTO)
        {
            var email = Environment.GetEnvironmentVariable("OUT_EMAIL");
            var pass = Environment.GetEnvironmentVariable("OUT_PASS");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                throw new Exception("Erron on sending email");
            }
            
            var subject = $"Wallet.NET Report";

            var pdfBytes = GeneratePdf(notificationInfoDTO);
            var pdfFileName = "StockReport.pdf";

            var message = new MailMessage();
            message.From = new MailAddress(email);
            message.To.Add(new MailAddress(notificationInfoDTO.Email));
            message.Subject = subject;
            message.Body = "Please find the attached stock report PDF.";
            message.IsBodyHtml = false;

            using (var stream = new MemoryStream(pdfBytes))
            {
                var attachment = new Attachment(stream, pdfFileName, "application/pdf");
                message.Attachments.Add(attachment);

                using (var client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(email, pass);

                    await Task.Run(() => client.Send(message));
                }
            }

            Console.WriteLine($"Email enviado com sucesso para {notificationInfoDTO.Email}");
        }

        private byte[] GeneratePdf(NotificationInfoDTO notificationInfoDTO)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Wallet.NET Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Text($"Stock Report").FontSize(16);

                            foreach (var report in notificationInfoDTO.StockReports)
                            {
                                column.Item().Text($"Ticker: {report.Ticker}").Bold();
                                column.Item().Text($"Variation during the last 5 days: {report.PriceInfo.Variation}");
                                column.Item().Text($"Current Price: {report.PriceInfo.CurrentPrice}");
                                column.Item().Text("News Articles:").Bold();

                                if (report.NewsArticles != null && report.NewsArticles.Any())
                                {
                                    foreach (var article in report.NewsArticles)
                                    {
                                        column.Item().Text(text =>
                                        {
                                            text.Span("- ").FontSize(12);
                                            text.Hyperlink(article.Title, article.Link).FontSize(12).FontColor(Colors.Blue.Medium);
                                        });
                                    }
                                }
                                else
                                {
                                    column.Item().Text("- No news articles available.");
                                }

                                column.Item().Text(""); 
                            }
                        });

                    page.Footer()
                        .Text("Click on the titles to read the full articles.")
                        .FontSize(10).FontColor(Colors.Grey.Medium);
                });
            });

            using var memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }
    }
}