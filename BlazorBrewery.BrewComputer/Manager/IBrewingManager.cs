using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public interface IBrewingManager
    {
        BrewingRecipe? CurrentBrewingRecipe { get; set; }
        Action? CurrentBrewingRecipeHasChanged { get; set; }
        BrewingStep? CurrentBrewingStep { get; set; }
        Action? CurrentBrewingStepHasChanged { get; set; }
    }
}