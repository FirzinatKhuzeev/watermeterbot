namespace WaterMeterBot
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Bot.Builder.FormFlow;

    public enum WaterTypeOptions
    {
        [Describe("ГВС")]
        [Terms(new string[] {"1", "Горячая", "ГВС" })]
        [Display(Name = "ГВС")]
        HeatWaterSupply = 1,
        [Describe("ХВС")]
        [Terms(new string[] { "2", "Холодная", "ХВС" })]
        [Display(Name = "ХВС")]
        ColdWaterSupply = 2
    }
}