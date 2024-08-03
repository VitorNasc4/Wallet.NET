using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.Models;

namespace Wallet.NET.Services.News
{
    public interface INewsService
    {
        Task<List<NewsArticle>?> GetAllNewsArticlesAsync();
        Task<List<NewsArticle>?> GetAllNewsArticlesWithCacheAsync();
    }
}