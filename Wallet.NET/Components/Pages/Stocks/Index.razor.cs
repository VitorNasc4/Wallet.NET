using Microsoft.AspNetCore.Components;
using MudBlazor;
using Wallet.NET.Models;
using Wallet.NET.Repositories.Stocks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class IndexPage : ComponentBase
    {
        [Inject]
        public IStockRepository repository { get; set; } = null!;

        [Inject]
        public IDialogService Dialog { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public List<Stock> Stocks { get; set; } = new List<Stock>();
        public async Task DeleteStock(Stock stock)
        {
            try
            {
                var result = await Dialog.ShowMessageBox
                    (
                        "Attention",
                        $"Do you want to delete the stock {stock.Ticker}?",
                        yesText: "Yes",
                        cancelText: "No"
                    );

                if (result is true)
                {
                    await repository.DeleteStockAsync(stock.Id);
                    Snackbar.Add($"Stock {stock} successfully deleted!", Severity.Success);
                    await OnInitializedAsync();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }

        public void GoToUpdate(int id)
        {
            NavigationManager.NavigateTo($"/stocks/update/{id}");
        }

        protected override async Task OnInitializedAsync()
        {
            Stocks = await repository.GetAllStocksAsync();
        }
    }
}
