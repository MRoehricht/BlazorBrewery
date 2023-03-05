namespace BlazorBrewery.Core.Models.Brewing
{
    public class BrewingStep
    {
        public Guid Id { get; set; }
        public Guid BrewingRecipeId { get; set; }
        public string? Name { get; set; }
        public int Position { get; set; }
        public int DurationSeconds { get; set; }
        public double TargetTemperature { get; set; }
        public BrewingStepTyp Typ { get; set; }
        public Guid? PumpintervalId { get; set; }
        public Pumpinterval? Pumpinterval { get; set; }
    }
}
