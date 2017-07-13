namespace WaterMeterBot
{
    using Microsoft.Bot.Builder.FormFlow;

    public enum MeterCountOptions
    {
        [Describe("Два")]
        [Terms(new string[] { "2", "Два" })]
        Two = 2,
        [Describe("Четыре")]
        [Terms(new string[] { "4", "Четыре" })]
        Four = 4
    }
}