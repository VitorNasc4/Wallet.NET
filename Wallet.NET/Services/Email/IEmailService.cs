using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.DTOs;

namespace Wallet.NET.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(NotificationInfoDTO notificationInfoDTO);
    }
}