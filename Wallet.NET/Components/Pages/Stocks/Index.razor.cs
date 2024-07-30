using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Wallet.NET.Models;
using Wallet.NET.Repositories.Stocks;
using Wallet.NET.Services.Stocks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class IndexPage : ComponentBase
    {
        [Inject]
        public IStockRepository repository { get; set; } = null!;

        [Inject]
        public IStockService service { get; set; } = null!;

        [Inject]
        public IDialogService Dialog { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public List<StockViewModel> Stocks { get; set; } = new List<StockViewModel>();
        public async Task DeleteStock(StockViewModel stock)
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
                    Snackbar.Add($"Stock {stock.Ticker} successfully deleted!", Severity.Success);
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

        public bool HideButtons { get; set; } = false;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            HideButtons = !auth.User.IsInRole("User");

            try
            {
                var stocks = await repository.GetAllStocksAsync();
                var stockViewModelList = new List<StockViewModel>();
                foreach (var stock in stocks)
                {
                    var stockDTO = await service.GetStockInfoAsync(stock.Ticker, stock.Exchange);
                    var stockViewModel = new StockViewModel
                    {
                        Id = stock.Id,
                        Ticker = stock.Ticker,
                        Exchange = stock.Exchange,
                        CurrentValue = stockDTO is not null ? stockDTO.CurrentValue.ToString() : "Error on get Current Value",
                        DailyChange = stockDTO is not null ? stockDTO.DailyChange.ToString() : "Error on get Daily Change",
                    };
                    stockViewModelList.Add(stockViewModel);
                }

                Stocks = stockViewModelList;
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }
}
