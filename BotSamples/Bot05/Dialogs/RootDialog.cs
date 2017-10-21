using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;

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
            userData.SetProperty<string>("SelectedRoute", "route");

            if (activity.Text == "2")
            {
                await context.Forward(new Step2Dialog(), this.ResumeAfter, activity, CancellationToken.None);
            }
            else if (activity.Text == "3")
            {
                await context.Forward(new Step3Dialog(), this.ResumeAfter, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync($"Root: {activity.Text}");
                context.Wait(this.MessageReceivedAsync);
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync($"ResumeAfter: {message}");
        }
    }
}