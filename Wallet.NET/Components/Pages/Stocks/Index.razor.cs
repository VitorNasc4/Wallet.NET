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

        public bool IsLoading { get; set; } = true;

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
                    var auth = await AuthenticationState;
                    var user = auth.User;
                    var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (user.Identity is null || string.IsNullOrEmpty(userId))
                    {
                        throw new Exception("User Id not found");
                    }

                    await service.RemoveStockToUserAsync(userId!, stock.Id);
                    // await repository.DeleteStockAsync(stock.Id);
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
        private Task<AuthenticationState> AuthenticationState { get; set; } =null!;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var auth = await AuthenticationState;

                HideButtons = !auth.User.IsInRole("User");

                var user = auth.User;
                var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (user.Identity is null || string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User Id not found");
                }
                
                var stocks = await service.GetUserStocksAsync(userId!);
                var stockViewModelList = new List<StockViewModel>();
                foreach (var stock in stocks)
                {
                    var stockDTO = await service.GetStockInfoAsync(stock.Ticker, stock.Exchange);
                    var stockViewModel = new StockViewModel
                    {
                        Id = stock.Id,
                        Ticker = stock.Ticker,
                        Exchange = stock.Exchange,
                        OpeningValue = stockDTO is not null && !string.IsNullOrEmpty(stockDTO.OpeningValue) ? stockDTO.OpeningValue : "Error on get Opening Value",
                        CurrentValue = stockDTO is not null && !string.IsNullOrEmpty(stockDTO.CurrentValue) ? stockDTO.CurrentValue : "Error on get Current Value",
                        DailyChange = stockDTO is not null ? stockDTO.DailyChange.ToString() : "Error on get Daily Change",
                        Variation = stockDTO is not null && !string.IsNullOrEmpty(stockDTO.Variation) ? stockDTO.Variation : "Error on get Variation",
                    };
                    stockViewModelList.Add(stockViewModel);
                }

                Stocks = stockViewModelList;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }

        public string GetColor(string variation)
        {
            if (variation.StartsWith("-"))
            {
                return "red";
            }
            return "green";
        }
    }
}
