using System.ComponentModel.DataAnnotations;

namespace BlazorBrewery.Core.Models.Brewing
{
    public enum BrewingStepTyp
    {
        [Display(Name = "Manuell")]
        Manually,
        [Display(Name = "Heizen")]
        Heat,
        [Display(Name = "Halten")]
        HoldTemperature,
        //[Display(Name = "Abkühlen")]
        //CoolDown
    }
}
