using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bot06.Forms
{
    [Serializable]
    public class RootJForm
    {
        public static IForm<JObject> BuildForm()
        {
            string filename = $"{AppContext.BaseDirectory}/Forms/RootJForm.json";
            string testJForm = File.ReadAllText(filename);

            var schema = JObject.Parse(testJForm);
            return new FormBuilderJson(schema)
                .AddRemainingFields()
                .Build();
        }
    }
}