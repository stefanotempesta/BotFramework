using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot02.Forms;
using Microsoft.Bot.Builder.FormFlow;

namespace Bot02
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, BuildRootForm);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private static IDialog<RootForm> BuildRootForm()
        {
            return Chain
                .From(() => FormDialog.FromForm(RootForm.BuildForm))
                .Do(CompleteForm);
        }

        private static async Task CompleteForm(IBotContext context, IAwaitable<RootForm> result)
        {
            try
            {
                var completed = await result;

                await context.PostAsync($"Enjoy {completed.Destination}!");
            }
            catch (FormCanceledException<RootForm> ex)
            {
                await context.PostAsync($"Exception: {ex.Message}");
            }
        }
    }
}