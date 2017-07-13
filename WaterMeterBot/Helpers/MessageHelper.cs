namespace WaterMeterBot.Helpers
{
    using System.Text;
    using System.Linq;
    using Models;

    public static class MessageHelper
    {
        public static string GetEmailBodyText(AccountDetails accountDetails)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<b>Лицевой счет:</b> {accountDetails.PersonalAccount}<br/>");
            sb.AppendLine($"<b>ФИО:</b> {accountDetails.FullName}<br/>");
            sb.AppendLine($"<b>Номер телефона:</b> {accountDetails.PhoneNumber}<br/>");
            sb.AppendLine($"<b>Адрес:</b> г. Казань, ул. Рауиса Гареева, д. {(int)accountDetails.House}, кв. {accountDetails.Appartment}<br/>");
            sb.AppendLine("<i>Показания приборов учета:</i><br/>");

            var kitchenMeters = accountDetails.WaterMeters.Where(m => m.Arrangement == ArrangementOptions.Kitchen).ToList();
            var bathroomMeters = accountDetails.WaterMeters.Where(m => m.Arrangement == ArrangementOptions.Bathroom).ToList();

            if (kitchenMeters.Any())
            {
                sb.AppendLine("<b>Кухня:</b><br/>");
                foreach (var meter in kitchenMeters)
                {
                    var type = meter.WaterType == WaterTypeOptions.HeatWaterSupply ? "ГВС" : "ХВС";
                    sb.AppendLine($"* {meter.MeterName} [{type}]. Значение: {meter.MeterReading}.<br/>");
                }
            }

            if (bathroomMeters.Any())
            {
                sb.AppendLine("<b>Ванная комната:</b>");
                foreach (var meter in bathroomMeters)
                {
                    var type = meter.WaterType == WaterTypeOptions.HeatWaterSupply ? "ГВС" : "ХВС";
                    sb.AppendLine($"* {meter.MeterName} [{type}]. Значение: {meter.MeterReading}.<br/>");
                }
            }

            return sb.ToString();
        }

        public static string GetHelpText(string userName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Добро пожаловать, {userName}!");
            sb.AppendLine("С помощью данного бота Вы можете передать показания приборов учета воды,");
            sb.AppendLine("предоставляемые управляющей компании «Центр-МК».");
            sb.AppendLine("Прием показаний приборов учета воды осуществляется с 23 по 25 число.\n");
            sb.AppendLine("<b>Доступные команды:</b>");
            sb.AppendLine("/старт - Начать передачу показаний приборов учета воды.");
            sb.AppendLine("/сброс - Сбросить текущий процесс.");
            sb.AppendLine("/помощь - Вывод справочной информации.");

            return sb.ToString();
        }
    }
}