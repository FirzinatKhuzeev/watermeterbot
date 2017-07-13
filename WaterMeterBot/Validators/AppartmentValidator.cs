namespace WaterMeterBot.Validators
{
    using System.Text.RegularExpressions;
    using Microsoft.Bot.Builder.FormFlow;
    using Models;

    public static class AppartmentValidator
    {
#pragma warning disable 1998
        public static ValidateAsyncDelegate<AccountDetails> ValidateAsync =
            async (state, response) =>
            {
                var result = new ValidateResult { IsValid = true, Value = response };
                var appartment = (response as string).Trim();
                Regex regex = new Regex("^\\d{1,3}$");
                if (!regex.IsMatch(appartment))
                {
                    result.Feedback = "Номер квартиры может содержать только цифры (не более 3 цифр).";
                    result.IsValid = false;
                }
                return result;
            };
#pragma warning restore 1998
    }
}