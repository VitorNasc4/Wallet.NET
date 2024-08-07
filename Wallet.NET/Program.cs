using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using StackExchange.Redis;
using Wallet.NET.Components;
using Wallet.NET.Components.Account;
using Wallet.NET.Data;
using Wallet.NET.Repositories.Stocks;
using Wallet.NET.Services.Email;
using Wallet.NET.Services.Indices;
using Wallet.NET.Services.Monitor;
using Wallet.NET.Services.News;
using Wallet.NET.Services.Report;
using Wallet.NET.Services.Stocks;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddMemoryCache();;
builder.Services.AddScoped<IIndexService, IndexService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHangfire(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("RedisConnection");
    var redis = ConnectionMultiplexer.Connect(connectionString!);

    options.UseRedisStorage(redis, options: new RedisStorageOptions { Prefix = $"HANG_FIRE"});
});
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<MonitorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapHangfireDashboard("/hangfire");
app.Run();
