using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot01.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (count == 0)
            {
                await context.PostAsync("Hi there, what's your name?");
                count++;
            }
            else
            {
                await context.PostAsync($"Hello {activity.Text}, nice to meet you!");
            }

            context.Wait(MessageReceivedAsync);
        }

        private int count = 0;
    }
}