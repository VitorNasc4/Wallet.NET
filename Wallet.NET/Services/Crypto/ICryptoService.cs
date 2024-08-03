using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.NET.Models;

namespace Wallet.NET.Services.Indices
{
    public interface ICryptoService
    {
        Task<List<Crypto>> GetCryptoInfoAsync();
    }
}