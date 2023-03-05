using BlazorBrewery.BrewComputer.Interfaces.Brewing;
using BlazorBrewery.BrewComputer.Manager;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Services.Brewing
{
    public class StepBrewService : IStepBrewService
    {
        private readonly ITemperatureManager _temperatureManager;
        private readonly IPumpManager _pumpManager;
        private DateTime _startTime;
        private IStepProcessesUpdater _updater;

        public StepBrewService(ITemperatureManager temperatureManager, IPumpManager pumpManager)
        {
            _temperatureManager = temperatureManager;
            _pumpManager = pumpManager;
        }

        public void Run(BrewingStep brewingStep, IStepProcessesUpdater updater)
        {
            _startTime = DateTime.Now;
            _updater = updater;
            if (brewingStep.Typ == BrewingStepTyp.Manually) return;

            //if (brewingStep.Pumpinterval != null)
            //{
            //    _pumpManager.Work(brewingStep.Pumpinterval, brewingStep.Typ);
            //}


            if (brewingStep.TargetTemperature != 0.0)
            {
                _temperatureManager.WorkDone += Stop;
                _temperatureManager.Work(brewingStep.TargetTemperature, brewingStep.DurationSeconds, brewingStep.Typ);
            }

        }

        public void Stop()
        {
            _updater.ICcompleted = true;
            _updater.PastTime = DateTime.Now - _startTime;
            _pumpManager.StopWork();
            _temperatureManager.StopWork();
        }
    }
}
