using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Builder.FormFlow;
using Bot06.Forms;

namespace Bot06
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
                await Conversation.SendAsync(activity, BuildJsonForm);
            }
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private static IDialog<JObject> BuildJsonForm()
        {
            return Chain
                .From(() => FormDialog.FromForm(RootJForm.BuildForm));
        }
    }
}