using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Ingredients;

namespace BlazorBrewery.Database.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        Task<BrewingRecipe> Add(BrewingRecipe brewingRecipe);
        Task<List<BrewingRecipe>> AllBrewingRecipes();
        Task Delete(BrewingRecipe brewingRecipe);
        BrewingRecipe GetBrewingRecipe(Guid Id);
        Task<BrewingRecipe> Save(BrewingRecipe brewingRecipe);
        Task<List<Unit>> GetUnits();
        Task<BrewingStep?> GetBrewingStep(Guid id);
        Task<List<Pumpinterval>> GetAllPumpintervals();
        Task<Pumpinterval> CreateEmptyPumpInterval();
        Task Delete(Pumpinterval pumpinterval);

        Task Save(Pumpinterval interval);
    }
}