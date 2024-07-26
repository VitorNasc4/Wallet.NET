using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Wallet.NET.Models;
using Wallet.NET.Repositories.Stocks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class CreateStockPage : ComponentBase
    {
        [Inject]
        public IStockRepository repository { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public StockInputModel InputModel { get; set; } = new StockInputModel();

        public async Task OnValidSubmitAsync(EditContext editContext)
        {
            try
            {
                if (editContext.Model is StockInputModel model)
                {
                    var stock = new Stock
                    {
                        Ticker = model.Ticker,
                        Exchange = model.Exchange
                    };
                    await repository.CreateStockAsync(stock);
                    Snackbar.Add("Stock successfully registered", Severity.Success);
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