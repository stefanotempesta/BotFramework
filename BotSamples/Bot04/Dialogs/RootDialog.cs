using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading;

namespace Bot04.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            if (count == 0)
            {
                await ShowAvailableDestinations(context, activity);
                count++;
            }
            else if (IsAvailableDestination(activity.Text))
            {
                await context.Forward(new DestinationDialog(), ResumeAfter, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync($"{activity.Text} is not an available destination.");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ShowAvailableDestinations(IDialogContext context, Activity activity)
        {
            Activity reply = activity.CreateReply();

            reply.Text = "Where would you like to go?";
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction { Title = "Kilimanjaro", Type = ActionTypes.ImBack, Value = "Kilimanjaro" },
                    new CardAction { Title = "Himalaya", Type = ActionTypes.ImBack, Value = "Himalaya" },
                    new CardAction { Title = "Andes", Type = ActionTypes.ImBack, Value = "Andes" }
                }
            };

            await context.PostAsync(reply);
            context.Wait(MessageReceivedAsync);
        }

        private bool IsAvailableDestination(string text)
        {
            return new List<string> { "kilimanjaro", "himalaya", "andes" }
                .Contains(text.ToLower());
        }

        private async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as string;
            await context.PostAsync(message);
        }

        private int count = 0;
    }
}