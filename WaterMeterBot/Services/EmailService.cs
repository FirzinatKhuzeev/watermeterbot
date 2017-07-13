namespace WaterMeterBot.Services
{
    using System.Configuration;
    using System.Threading.Tasks;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Net;
    
    public class EmailService
    {
        public async Task<string> SendEmail(string message)
        {
            var apiKey = ConfigurationManager.AppSettings["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = ConfigurationManager.AppSettings["Email:From"];
            var to = ConfigurationManager.AppSettings["Email:To"];

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from),
                Subject = "Показания приборов учета воды",
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(to));

            var response = await client.SendEmailAsync(msg);

            return response.StatusCode == HttpStatusCode.Accepted
                ? "Данные успешно отправлены. Благодарим вас за использование данного бота."
                : "Возникала ошибка при отправке данных.";
        }
    }
}