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
    public class Step3Dialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;
            await context.PostAsync($"Step 3: {activity.Text}");

            StateClient state = activity.GetStateClient();
            BotData userData = await state.BotState.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            string selectedRoute = userData.GetProperty<string>("SelectedRoute");

            context.Done($"From 3: {selectedRoute}");
        }
    }
}