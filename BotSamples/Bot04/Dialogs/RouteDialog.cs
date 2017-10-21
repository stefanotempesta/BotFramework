using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Bot04.Dialogs
{
    [Serializable]
    public class RouteDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            await context.PostAsync($"Your selected route is {activity.Text}.");

            PromptDialog.Confirm(
                context,
                ConfirmAsync,
                "Do you confirm?",
                "Try again (Yes/No)",
                promptStyle: PromptStyle.Auto);

            context.Wait(MessageReceivedAsync);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync("Route confirmed.");
            }
            else
            {
                context.Call(new RootDialog(), null);
            }
        }
    }
}