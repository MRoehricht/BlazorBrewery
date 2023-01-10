using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBrewery.Database.Repositories;
using BlazorBreweryInterface.Controller;
using BlazorBreweryInterface.Debug.Controller;
using BlazorBreweryInterface.Interfaces;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Recipes;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Settings;
using BlazorBreweryServer.Services.ViewModels.Recipes;
using BlazorBreweryServer.Services.ViewModels.Settings;

namespace BlazorBreweryServer
{
    public static class DependencyInjectionService
    {
        public static void AddServiceDependencies(this IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env != null && env == "Development")
            {
                services.AddScoped<IOneWireDeviceController, DebugOneWireDeviceController>();
                services.AddScoped<IPinController, DebugPinController>();
                services.AddScoped<IThermometerController, DebugThermometerController>();
            }
            else
            {
                services.AddScoped<IOneWireDeviceController, OneWireDeviceController>();
                services.AddScoped<IPinController, PinController>();
                services.AddScoped<IThermometerController, ThermometerController>();
            }


            services.AddScoped<IRecipeRepository, RecipeRepository>();


            services.AddScoped<IRecipesViewModelService, RecipesViewModelService>();
            services.AddScoped<ISettingsViewModelService, SettingsViewModelService>();


        }
    }
}
