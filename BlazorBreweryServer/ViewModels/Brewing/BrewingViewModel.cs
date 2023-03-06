using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBreweryServer.ViewModels.Brewing
{
    public class BrewingViewModel
    {
        public List<BrewingRecipe> Recipes { get; set; } = new List<BrewingRecipe>();

        public BrewingRecipe? SelectedRecipe { get; set; }

        public Queue<BrewingStep> QueueBrewingSteps { get; } = new Queue<BrewingStep>();

        public void BuildQueueBrewingSteps()
        {
            if (SelectedRecipe == null) return;

            foreach (var step in SelectedRecipe.BrewingSteps.OrderBy(_ => _.Position))
            {
                QueueBrewingSteps.Enqueue(step);
            }
        }
    }
}
