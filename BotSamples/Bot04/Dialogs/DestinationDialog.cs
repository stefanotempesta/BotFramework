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
    public class DestinationDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            PromptDialog.Confirm(
                context,
                ConfirmAsync,
                $"Your selected destination is {activity.Text}. Do you confirm?",
                "Try again (Yes/No)",
                promptStyle: PromptStyle.Auto);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync("Destination confirmed.");
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                context.Done("Goodbye.");
            }
        }
    }
}