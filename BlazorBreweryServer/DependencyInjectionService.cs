using BlazorBrewery.BrewComputer.Interfaces.Brewing;
using BlazorBrewery.BrewComputer.Manager;
using BlazorBrewery.BrewComputer.Services.Brewing;
using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBrewery.Database.Repositories;
using BlazorBreweryInterface.Controller;
using BlazorBreweryInterface.Fake.Controller;
using BlazorBreweryInterface.Interfaces;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Brewing;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Recipes;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Settings;
using BlazorBreweryServer.Services.ViewModels.Brewing;
using BlazorBreweryServer.Services.ViewModels.Recipes;
using BlazorBreweryServer.Services.ViewModels.Settings;
using BlazorBreweryServer.Services.ViewModels.SingletonTest;

namespace BlazorBreweryServer
{
    public static class DependencyInjectionService
    {
        public static void AddServiceDependencies(this IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env != null && env == "Development")
            {
                services.AddScoped<IOneWireDeviceController, FakeOneWireDeviceController>();
                services.AddScoped<IThermometerController, FakeThermometerController>();
            }
            else
            {
                services.AddScoped<IOneWireDeviceController, OneWireDeviceController>();
                services.AddScoped<IThermometerController, ThermometerController>();
            }


            services.AddScoped<IRecipeRepository, RecipeRepository>();


            services.AddScoped<IPumpIntervalViewModelService, PumpIntervalViewModelService>();
            services.AddScoped<IRecipesViewModelService, RecipesViewModelService>();
            services.AddScoped<ISettingsViewModelService, SettingsViewModelService>();
            services.AddScoped<IBrewingStepViewModelService, BrewingStepViewModelService>();
            services.AddScoped<IBrewingViewModelService, BrewingViewModelService>();
            services.AddScoped<ITemperatureManager, TemperatureManager>();
            services.AddScoped<IPumpManager, PumpManager>();
            services.AddSingleton<IRelayManager, RelayManager>();

            services.AddScoped<IStepBrewService, StepBrewService>();

            services.AddSingleton<ISingletonTestViewModelService, SingletonTestViewModelService>();
            services.AddSingleton<IBrewingManager, BrewingManager>();
        }
    }
}
