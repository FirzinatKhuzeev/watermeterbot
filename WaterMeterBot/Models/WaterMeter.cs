namespace WaterMeterBot.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Threading;
    using System.Configuration;
    using Microsoft.Bot.Builder.FormFlow;
    
    [Serializable]
    public class WaterMeter
    {
        private static readonly ConcurrentDictionary<CultureInfo, IForm<WaterMeter>> Forms = new ConcurrentDictionary<CultureInfo, IForm<WaterMeter>>();

        [Prompt("Введите {&}:")]
        [Describe("Серийный номер")]
        public string MeterSerialNumber { get; set; }
        
        [Prompt("Какой {&} вы хотите ввести? {||}")]
        [Describe("Тип водоснабжения")]
        public WaterTypeOptions WaterType { get; set; }

        [Prompt("Введите {&} прибора учета:")]
        [Describe("Наименование")]
        public string MeterName { get; set; }

        [Prompt("Введите {&}:")]
        [Describe("Показание прибора учета воды")]
        public string MeterReading { get; set; }

        [Prompt("Выберите {&} прибора учета воды: {||}")]
        [Describe("Расположение")]
        public ArrangementOptions Arrangement { get; set; }

        public static IForm<WaterMeter> BuildWaterMeterForm()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(ConfigurationManager.AppSettings["CultureInfo"]);
            var culture = Thread.CurrentThread.CurrentUICulture;
            IForm<WaterMeter> form;
            if (!Forms.TryGetValue(culture, out form))
            {
                var builder = new FormBuilder<WaterMeter>()
                    .Field(nameof(Arrangement))
                    .Field(nameof(WaterType))
                    .Field(nameof(MeterReading));

                builder.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Auto;
                form = builder.Build();
                Forms[culture] = form;
            }

            return form;
        }
    }
}