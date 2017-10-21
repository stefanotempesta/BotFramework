using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Collections.Generic;

namespace Bot05.Dialogs
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

            StateClient state = activity.GetStateClient();
            BotData userData = await state.BotState.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            userData.SetProperty<string>("SelectedOptions", activity.Text);

            if (count++ == 0)
            {
                Activity reply = activity.CreateReply();
                reply.AddHeroCard("Choose any option:", new List<string> { "2", "3" }, new List<string> { "Step 2", "Step 3" });
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }
            else if (activity.Text == "2")
            {
                await context.Forward(new Step2Dialog(), ResumeAfter, activity, CancellationToken.None);
            }
            else if (activity.Text == "3")
            {
                await context.Forward(new Step3Dialog(), ResumeAfter, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync($"Root: {activity.Text}");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync($"Resume after Root: {message}");
        }

        private int count = 0;
    }
}