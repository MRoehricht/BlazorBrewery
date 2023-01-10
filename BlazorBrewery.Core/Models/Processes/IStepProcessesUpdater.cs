namespace BlazorBrewery.Core.Models.Processes
{
    public interface IStepProcessesUpdater
    {
        bool ICcompleted { get; set; }
        string? InfoText { get; set; }
        TimeSpan PastTime { get; set; }
    }
}