using BlazorBrewery.BrewComputer.Interfaces.Brewing;
using BlazorBrewery.BrewComputer.Manager;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;
using Microsoft.Extensions.Logging;

namespace BlazorBrewery.BrewComputer.Services.Brewing
{
    public class StepBrewService : IStepBrewService
    {
        private readonly ITemperatureManager _temperatureManager;
        private readonly IPumpManager _pumpManager;
        private readonly ILogger<StepBrewService> _logger;
        private DateTime _startTime;
        private IStepProcessesUpdater _updater;
        private BrewingStep _brewingStep;
        public TimeSpan BrewTime { get; private set; }

        public Action WorkIsDone { get; set; }

        public StepBrewService(ITemperatureManager temperatureManager, IPumpManager pumpManager, ILogger<StepBrewService> logger)
        {
            _temperatureManager = temperatureManager;
            _pumpManager = pumpManager;
            _logger = logger;
        }

        public void Run(BrewingStep brewingStep, IStepProcessesUpdater updater, IProgress<int> progress)
        {
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} Stufe Start -" + brewingStep.Name);
            _startTime = DateTime.Now;
            _updater = updater;
            _brewingStep = brewingStep;
            progress.Report(0);
            if (brewingStep.Typ == BrewingStepTyp.Manually) return;

            if (brewingStep.Pumpinterval != null)
            {
                _pumpManager.Work(brewingStep.Pumpinterval, brewingStep.Typ);
            }

            if (brewingStep.TargetTemperature != 0.0)
            {
                if (_temperatureManager.WorkDone == null)
                {
                    _temperatureManager.WorkDone += WorkDone;
                }

                _temperatureManager.Work(brewingStep.TargetTemperature, brewingStep.DurationMinutes, brewingStep.Typ, progress);
            }
        }

        public void Clear()
        {
            if (_temperatureManager == null || _pumpManager == null) return;

            if (_temperatureManager.WorkDone != null)
            {
                _temperatureManager.WorkDone -= WorkDone;
            }
            _temperatureManager.StopWork();
            _pumpManager.StopWork();
            BrewTime = TimeSpan.Zero;
        }

        private void WorkDone()
        {
            _updater.ICcompleted = true;
            _updater.PastTime = DateTime.Now - _startTime;
            BrewTime += _updater.PastTime;
            _pumpManager.StopWork();
            _temperatureManager.StopWork();
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} Stufe Beendet -" + _brewingStep.Name);
            WorkIsDone?.Invoke();
        }

        public void Stop()
        {
            _updater.ICcompleted = false;
            _updater.PastTime = DateTime.Now - _startTime;
            BrewTime += _updater.PastTime;
            _pumpManager.StopWork();
            _temperatureManager.StopWork();
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} Stufe Gesoppt -" + _brewingStep.Name);
        }
    }
}
