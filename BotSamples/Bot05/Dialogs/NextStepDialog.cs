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
    public class NextStepDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> argument)
        {
            var activity = await argument as Activity;

            StateClient state = activity.GetStateClient();
            BotData userData = await state.BotState.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            string selectedOption = userData.GetProperty<string>("SelectedOption");

            context.Done($"Selection: {selectedOption}");
        }
    }
}