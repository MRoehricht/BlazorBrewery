using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public class BrewingManager : IBrewingManager
    {
        private BrewingRecipe? _currentBrewingRecipe;

        public BrewingRecipe? CurrentBrewingRecipe
        {
            get { return _currentBrewingRecipe; }
            set { _currentBrewingRecipe = value; CurrentBrewingRecipeHasChanged?.Invoke(); }
        }

        private BrewingStep? _currentBrewingStep;

        public BrewingStep? CurrentBrewingStep
        {
            get { return _currentBrewingStep; }
            set { _currentBrewingStep = value; CurrentBrewingStepHasChanged?.Invoke(); }
        }

        public Action? CurrentBrewingRecipeHasChanged { get; set; }
        public Action? CurrentBrewingStepHasChanged { get; set; }
        public Action? StopAllBewings { get; set; }

        public void RunStopAllBewings()
        {
            StopAllBewings?.Invoke();
        }
    }
}
