using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Wallet.NET.Models;
using Wallet.NET.Repositories.Stocks;
using Wallet.NET.Services.Stocks;

namespace Wallet.NET.Components.Pages.Stocks
{
    public class CreateStockPage : ComponentBase
    {
        [Inject]
        public IStockRepository repository { get; set; } = null!;

        [Inject]
        public IStockService service { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public StockInputModel InputModel { get; set; } = new StockInputModel();
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; } =null!;


        public async Task OnValidSubmitAsync(EditContext editContext)
        {
            try
            {
                if (editContext.Model is StockInputModel model)
                {
                    var newStock = new Stock
                    {
                        Ticker = model.Ticker,
                        Exchange = model.Exchange
                    };

                    var stock = await service.ValidateAndCreateStockAsync(newStock);

                    if (stock is null)
                    {
                        throw new Exception("There was a problem during the stock registration");
                    }

                    var auth = await AuthenticationState;
                    var user = auth.User;
                    var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (user.Identity is null || string.IsNullOrEmpty(userId))
                    {
                        throw new Exception("User Id not found");
                    
                    }
                    await service.AddStockToUserAsync(userId!, stock.Id);

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