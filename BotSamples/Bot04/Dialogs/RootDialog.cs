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

            ShowAvailableRoutes(activity);

            if (IsAvailableRoute(activity.Text))
            {
                await context.Forward(new RouteDialog(), this.ResumeAfter, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync($"{activity.Text} is not an available route.");
                context.Wait(this.MessageReceivedAsync);
            }

            context.Wait(MessageReceivedAsync);
        }

        private void ShowAvailableRoutes(Activity activity)
        {
            activity.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction { Title = "Machame route", Type = ActionTypes.ImBack, Value = "machame" },
                    new CardAction { Title = "Marangu route", Type = ActionTypes.ImBack, Value = "marangu" },
                    new CardAction { Title = "Lemosho route", Type = ActionTypes.ImBack, Value = "lemosho" }
                }
            };
        }

        private bool IsAvailableRoute(string text)
        {
            return new List<string> { "machame", "marangu", "lemosho" }
                .Contains(text.ToLower());
        }

        private async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as IMessageActivity;
            await context.PostAsync($"Enjoy {message.Text}!");
        }
    }
}