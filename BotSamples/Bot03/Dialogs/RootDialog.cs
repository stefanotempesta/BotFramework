using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using AdaptiveCards;

namespace Bot03.Dialogs
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
            Activity reply = activity.CreateReply();

            AddSuggestedActions(reply);
            //AddAttachment(reply);
            //AddHeroCard(reply);   // reply.AddHeroCard();
            //AddThumbnailCard(reply);
            //AddVideoCard(reply);
            //AddReceiptCard(reply);
            //AddAdaptiveCard(reply);

            await context.PostAsync(reply);
            context.Wait(MessageReceivedAsync);
        }

        private void AddSuggestedActions(Activity reply)
        {
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction { Title = "Machame route", Type = ActionTypes.ImBack, Value = "route-machame" },
                    new CardAction { Title = "Marangu route", Type = ActionTypes.ImBack, Value = "route-marangu" },
                    new CardAction { Title = "Lemosho route", Type = ActionTypes.ImBack, Value = "route-lemosho" }
                }
            };
        }

        private void AddAttachment(Activity reply)
        {
            reply.Attachments.Add(new Attachment
            {
                ContentUrl = "https://vignette.wikia.nocookie.net/39cluesidea/images/2/26/Smile.png",
                ContentType = "image/png",
                Name = "Smile.png"
            });
        }

        private void AddHeroCard(Activity reply)
        {
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(new HeroCard
            {
                Title = "Welcome to Kiligate",
                Subtitle = "What experience would you like to do?",
                Images = new List<CardImage>
                {
                    new CardImage { Url = "https://kiligate.com/wp-content/uploads/2017/06/maasai-tribe-thumbnail-315x150.jpg" }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction { Title = "Hiking", Type = ActionTypes.PostBack, Value = "hiking" },
                    new CardAction { Title = "Wildlife Safari", Type = ActionTypes.PostBack, Value = "safari" },
                    new CardAction { Title = "Cultural and Tribal Excursions", Type = ActionTypes.PostBack, Value = "excursion" }
                }
            }.ToAttachment());
        }

        private void AddThumbnailCard(Activity reply)
        {
            reply.AttachmentLayout = AttachmentLayoutTypes.List;
            reply.Attachments.Add(new ThumbnailCard
            {
                Title = "Welcome to Kiligate",
                Subtitle = "www.kiligate.com",
                Images = new List<CardImage>
                {
                    new CardImage { Url = "https://kiligate.com/wp-content/uploads/2017/06/hiking-photo-by-phil-coffman-thumbnail-1-315x150-1-315x150.jpg" },
                },
                Buttons = new List<CardAction>
                {
                    new CardAction { Title = "Write to the bot", Type = ActionTypes.PostBack, Value = "Write this back to the bot" },
                    new CardAction { Title = "Show video", Type = ActionTypes.PlayVideo, Value = "https://www.youtube.com/watch?v=BEnl-4ee4Fw" },
                    new CardAction { Title = "Show image", Type = ActionTypes.ShowImage, Value = "https://kiligate.com/product-category/culture-tribes/" },
                    new CardAction { Title = "Download file", Type = ActionTypes.DownloadFile, Value = "http://www.latteseditori.it/Portals/0/carte-identita/kilimangiaro-plus.pdf" }
                }
            }.ToAttachment());
        }

        private void AddVideoCard(Activity reply)
        {
            reply.Attachments.Add(new VideoCard
            {
                Title = "Video card",
                Subtitle = "Climbing Kilimanjaro",
                Aspect = "16:9",
                Media = new List<MediaUrl>
                {
                    new MediaUrl { Url = "https://www.youtube.com/watch?v=BEnl-4ee4Fw" }
                }
            }.ToAttachment());
        }

        private void AddReceiptCard(Activity reply)
        {
            reply.Attachments.Add(new ReceiptCard
            {
                Title = "Order confirmed",
                Buttons = new List<CardAction>
                {
                    new CardAction { Title = "Confirm", Type = ActionTypes.PostBack, Value = "order-confirm" },
                    new CardAction { Title = "Cancel", Type = ActionTypes.PostBack, Value = "order-cancel" },
                },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem { Title = "Wildlife Safari in Africa", Subtitle = "Watch animals in their natural habitat", Image = new CardImage { Url = "https://kiligate.com/wp-content/uploads/2017/06/safari-giraffe-nairobi-thumbnail-315x150-1-315x150.jpg" }, Price = "100", Quantity = "1" },
                    new ReceiptItem { Title = "Mt. Kenya: Chogoria – Sirimon Route", Subtitle = "Hiking, Sightseeing, Walking Safari", Image = new CardImage { Url = "https://kiligate.com/wp-content/uploads/2017/07/Kenya-4-550x358.jpg" }, Price = "20", Quantity = "1" },
                    new ReceiptItem { Title = "Airport Transfer", Subtitle = "Kilimanjaro International Airport transfer", Image = new CardImage { Url = "https://kiligate.com/wp-content/uploads/2017/08/car-1-550x358.jpg" }, Price = "10", Quantity = "1" }
                },
                Tax = "N/A",
                Total = "USD 130.00"
            }.ToAttachment());
        }

        private void AddAdaptiveCard(Activity reply)
        {
            reply.Attachments.Add(new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = new AdaptiveCard
                {
                    Speak = "<s>Welcome to \"Kiligate\" <break strength='weak'/> your portal to fair and responsible tourism.</s>" +
                            "<s>Where do you want to travel?</s>",
                    Body = new List<CardElement>
                    {
                        new TextBlock { Text = "Welcome to Fair Voyage", Size = TextSize.Large, Weight = TextWeight.Bolder, Color = TextColor.Accent },
                        new TextBlock { Text = "Where do you want to travel?" }
                    },
                    Actions = new List<ActionBase>
                    {
                        new HttpAction { Title = "Kilimangiaro", Url = "https://kiligate.com/destination/kilimanjaro/" },
                        new HttpAction { Title = "Himalaya", Url = "https://www.himalayatours.ch" }
                    }
                }
            });
        }
    }
}