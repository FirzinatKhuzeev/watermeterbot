namespace WaterMeterBot
{
    using Microsoft.Bot.Builder.FormFlow;

    public enum HouseOptions
    {
        [Terms(new string[] { "1", "первый", "92", "Первый дом (92)" })]
        [Describe("92")]
        Fist = 92,
        [Terms(new string[] { "2", "второй", "94", "Первый дом (94)" })]
        [Describe("94")]
        Second = 94,
        [Terms(new string[] { "3", "третий", "96", "Первый дом (96)" })]
        [Describe("96")]
        Third = 96,
        [Terms(new string[] { "4", "четвертый", "98", "Первый дом (98)" })]
        [Describe("98")]
        Fourth = 98,
        [Terms(new string[] { "5", "пятый", "100", "Первый дом (100)" })]
        [Describe("100")]
        Fifth = 100
    }
}