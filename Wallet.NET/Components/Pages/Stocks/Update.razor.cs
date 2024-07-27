using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Wallet.NET.Models;
using Wallet.NET.Repositories.Stocks;
using Wallet.NET.Services.Stocks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class UpdateStockPage : ComponentBase
    {
        [Parameter]
        public int StockId { get; set; }

        [Inject]
        public IStockRepository repository { get; set; } = null!;

        [Inject]
        public IStockService service { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public StockInputModel InputModel { get; set; } = new StockInputModel();

        private Stock? CurrentStock { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentStock = await repository.GetStockByIdAsync(StockId);

            if (CurrentStock is null)
            {
                return;
            }

            InputModel = new StockInputModel
            {
                Ticker = CurrentStock.Ticker,
                Exchange = CurrentStock.Exchange,
            };
        }

        public async Task OnValidSubmitAsync(EditContext editContext)
        {
            try
            {
                if (editContext.Model is StockInputModel model)
                {
                    var isValidExchange = Stock.IsValidExchange(model.Exchange);
                    if (!isValidExchange)
                    {
                        throw new Exception("Exchange not valid");
                    }

                    var isValidStock = await service.IsValidStock(model.Ticker, model.Exchange);
                    if (!isValidStock)
                    {
                        throw new Exception("Ticker not found for this Exchange");
                    }

                    CurrentStock!.Ticker = model.Ticker;
                    CurrentStock.Exchange = model.Exchange;

                    await repository.UpdateStockAsync(CurrentStock);
                    Snackbar.Add("Stock successfully updated", Severity.Success);
                    NavigationManager.NavigateTo("/stocks");
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }

    }
}