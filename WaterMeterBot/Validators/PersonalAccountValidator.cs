namespace WaterMeterBot.Validators
{
    using System.Text.RegularExpressions;
    using Microsoft.Bot.Builder.FormFlow;
    using Models;

    public static class PersonalAccountValidator
    {
#pragma warning disable 1998
        public static ValidateAsyncDelegate<AccountDetails> ValidateAsync =
            async (state, response) =>
            {
                var result = new ValidateResult { IsValid = true, Value = response };
                var personalAccount = (response as string).Trim();
                var regex = new Regex("^\\d{10}$");
                if (!regex.IsMatch(personalAccount))
                {
                    result.Feedback = "Лицевой счет должно содержать 10 цифр.";
                    result.IsValid = false;
                }

                return result;
            };
#pragma warning restore 1998
    }
}