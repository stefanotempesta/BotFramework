using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Bot05.Dialogs
{
    [Serializable]
    public class Step2Dialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;
            await context.PostAsync($"Step 2: {activity.Text}");

            if (activity.Text == "exit")
            {
                context.Done("From 2");
            }
            else if (activity.Text == "3")
            {
                context.Call(new Step3Dialog(), this.ResumeAfter);
            }
            else
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("After 2");
        }
    }
}