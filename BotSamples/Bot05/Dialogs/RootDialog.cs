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

            if (count++ == 0)
            {
                Activity reply = activity.CreateReply();
                reply.AddHeroCard("Choose any option:", new List<string> { "Hiking", "Safari" }, new List<string> { "Hiking", "Safari" });
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                StateClient state = activity.GetStateClient();
                BotData userData = await state.BotState.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
                userData.SetProperty<string>("SelectedOption", activity.Text);

                await context.Forward(new NextStepDialog(), ResumeAfter, activity, CancellationToken.None);
            }
        }

        private async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync($"{message}");
        }

        private int count = 0;
    }
}