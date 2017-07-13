namespace WaterMeterBot.Models
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Configuration;
    using Microsoft.Bot.Builder.FormFlow;
    using Validators;

    [Serializable]
    public class AccountDetails
    {
        private static readonly ConcurrentDictionary<CultureInfo, IForm<AccountDetails>> Forms = new ConcurrentDictionary<CultureInfo, IForm<AccountDetails>>();

        [Prompt("Введите {&}:")]
        [Describe("Номер лицевого счета")]
        public string PersonalAccount;

        [Prompt("Введите {&}:")]
        [Describe("Фамилия Имя Отчество")]
        public string FullName;

        [Prompt("Выберите {&}: {||}")]
        [Describe("Номер дома")]
        public HouseOptions House;

        [Prompt("Введите {&}:")]
        [Describe("Номер квартиры")]
        public string Appartment;

        [Prompt("Введите {&}:")]
        [Describe("Номер телефона")]
        [Pattern(@"(\d)? ?\(?(\d\d\d)?\)? *?(\d\d\d) *?-? *?(\d\d) *?-? *?(\d\d)")]
        public string PhoneNumber;

        [Prompt("Выберите {&}: {||}")]
        [Describe("Количество приборов учета воды")]
        public MeterCountOptions MeterCount;

        public string Message;

        public bool IsAccepted;

        public List<WaterMeter> WaterMeters { get; set; }

        public static IForm<AccountDetails> BuildAccountDetailsForm()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(ConfigurationManager.AppSettings["CultureInfo"]);
            var culture = Thread.CurrentThread.CurrentUICulture;
            IForm<AccountDetails> form;
            if (!Forms.TryGetValue(culture, out form))
            {
                var builder = new FormBuilder<AccountDetails>()
                    .Field(nameof(PersonalAccount))
                    .Field(nameof(PersonalAccount),
                        validate: PersonalAccountValidator.ValidateAsync)
                    .Field(nameof(FullName))
                    .Field(nameof(House))
                    .Field(nameof(Appartment),
                        validate: AppartmentValidator.ValidateAsync)
                    .Field(nameof(PhoneNumber))
                    .Field(nameof(MeterCount));

                builder.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Auto;
                form = builder.Build();
                Forms[culture] = form;
            }

            return form;
        }
    }
}