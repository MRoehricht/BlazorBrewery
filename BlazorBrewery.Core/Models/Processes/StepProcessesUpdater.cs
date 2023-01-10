namespace BlazorBrewery.Core.Models.Processes
{
    public class StepProcessesUpdater : IStepProcessesUpdater
    {
        public TimeSpan PastTime { get; set; }
        public string? InfoText { get; set; }
        public bool ICcompleted { get; set; }
    }
}
