namespace WaterMeterBot.Dialogs
{
    using System.Collections.Generic;
    using Services;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Builder.FormFlow;
    using Models;
    using Helpers;

    [Serializable]
    public class MainDialog : IDialog<object>
    {
        private AccountDetails accountDetails;

        private int currentMeterCount = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            switch (message.Text)
            {
                case "/старт":
                case "/start":
                case "/сброс":
                case "/reset":
                    await this.StartFillingAsync(context);
                    break;
                case "/помощь":
                case "/help":
                    await this.HelpMessageAsync(context);
                    break;
            }
        }

        public async Task HelpMessageAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.Text = MessageHelper.GetHelpText(context.Activity.From.Name);
            reply.TextFormat = "xml";

            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }

        public async Task StartFillingAsync(IDialogContext context)
        {
            this.accountDetails = new AccountDetails();
            var accountDetailsDialog = new FormDialog<AccountDetails>(this.accountDetails, AccountDetails.BuildAccountDetailsForm, FormOptions.PromptInStart);

            context.Call(accountDetailsDialog, this.AfterAccountReceivedAsync);
        }

        public async Task AfterAccountReceivedAsync(IDialogContext context, IAwaitable<AccountDetails> result)
        {
            try
            {
                this.accountDetails = await result;
                this.accountDetails.WaterMeters = new List<WaterMeter>();
                this.currentMeterCount = 1;

                var waterMeter = new WaterMeter();
                var waterMeterDialog = new FormDialog<WaterMeter>(waterMeter, WaterMeter.BuildWaterMeterForm, FormOptions.PromptInStart);

                context.Call(waterMeterDialog, this.AfterWaterMeterReceivedAsync);
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("Вы отменили ввод данных.");
                context.Wait(MessageReceivedAsync);
            }
        }


        public async Task AfterWaterMeterReceivedAsync(IDialogContext context, IAwaitable<WaterMeter> result)
        {
            try
            {
                var waterMeter = await result;

                waterMeter.MeterName = $"Прибор учета воды №{this.currentMeterCount}";

                this.accountDetails.WaterMeters.Add(waterMeter);

                if ((int)this.accountDetails.MeterCount > this.currentMeterCount)
                {
                    var waterMeterDialog = new FormDialog<WaterMeter>(new WaterMeter(), WaterMeter.BuildWaterMeterForm, FormOptions.PromptInStart);
                    context.Call(waterMeterDialog, this.AfterWaterMeterReceivedAsync);
                }
                else
                {
                    PromptDialog.Choice(
                        context,
                        this.AfterAceptedAsync,
                        new List<string>() { "Да", "Нет" },
                        "Я даю свое согласие на обработку моих персональных данных, в соответствии с Федеральным законом от 27.07.2006 года №152-ФЗ 'О персональных данных'",
                        "Ошибка. Пожалуйста повторите.");
                }

                this.currentMeterCount++;
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("Вы отменили ввод данных.");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterAceptedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var isAccept = await result;
            var reply = context.MakeMessage();

            if (isAccept.Equals("да", StringComparison.OrdinalIgnoreCase))
            {
                accountDetails.Message = MessageHelper.GetEmailBodyText(accountDetails);
                reply.Text = accountDetails.Message;
                reply.TextFormat = "xml";
                await context.PostAsync(reply);

                PromptDialog.Choice(
                    context,
                    this.ResumeAndSendEmailAsync,
                    new List<string>() { "Да", "Нет" },
                    "Все данные корректны, отправить данные?",
                    "Ошибка. Пожалуйста повторите.");
            }
            else
            {
                reply.Text = "Вы отклонили пользовательское соглашение.";
                await context.PostAsync(reply);
                context.Wait(this.MessageReceivedAsync);
            }
        }

        public async Task ResumeAndSendEmailAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var isAccept = await argument;
            var reply = context.MakeMessage();
            if (isAccept.Equals("да", StringComparison.OrdinalIgnoreCase))
            {
                var email = new EmailService();
                var result = await email.SendEmail(accountDetails.Message);
                reply.Text = result;
                reply.TextFormat = "xml";
                await context.PostAsync(reply);
            }
            else
            {
                reply.Text = "Вы отменили отправку данных.";
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}