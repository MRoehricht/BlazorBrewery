using BlazorBrewery.Core.Services;
using BlazorBrewery.Database.Context;
using BlazorBreweryDatabase.Context;
using BlazorBreweryServer;
using BlazorBreweryServer.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;



var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
//builder.Services.AddMudServices();
builder.Services.AddServiceDependencies();
builder.Services.AddSingleton<IConfigurationStoreService>(new ConfigurationStoreService(config["PinConfig:HeatPinId"], config["PinConfig:PumpPinId"]));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

var connection = config["ConnectionStrings:DefaultConnection"];
if (connection == null)
{
    throw new ApplicationException("ConnectionStrings:DefaultConnection konnte nicht aus den appsettings gelesen werden.");
}

builder.Services.AddDbContext<UserContext>(options => options.UseMySQL(connection));
builder.Services.AddDbContext<RecipeContext>(options => options.UseMySQL(connection));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
