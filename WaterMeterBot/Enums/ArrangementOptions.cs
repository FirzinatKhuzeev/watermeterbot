namespace WaterMeterBot
{
    using Microsoft.Bot.Builder.FormFlow;

    public enum ArrangementOptions
    {
        [Terms(new string[] { "1", "Кухня" })]
        [Describe("Кухня")]
        Kitchen = 1,
        [Terms(new string[] { "2", "Ванная" })]
        [Describe("Ванная")]
        Bathroom = 2
    }
}