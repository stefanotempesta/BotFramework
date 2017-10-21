using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot02.Forms
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "I do not understand \"{0}\", try again.")]
    public class RootForm
    {
        public Destination Destination;

        [Optional]
        [Template(TemplateUsage.NoPreference, "Any")]
        [Prompt("Which {&} do you want to take? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
        public string Route;

        [Prompt("What kind of {&} do you want to do? You can choose more than one! {||}", ChoiceStyle = ChoiceStyleOptions.Buttons)]
        public List<Experience> Experiences;

        public GroupType GroupType;

        [Numeric(1, 10)]
        [Prompt("How many people? (1 to 10)")]
        [Template(TemplateUsage.NotUnderstood, "Invalid entry. You should enter a number between 1 and 10")]
        public int HowManyPeople;

        public static IForm<RootForm> BuildForm()
        {
            return new FormBuilder<RootForm>()
                .Message("Welcome")
                .Field(nameof(Destination))
                .Field(new FieldReflector<RootForm>(nameof(Route))
                    .SetType(null)  // null represents an enumeration field
                    .SetDefine(async (state, field) =>
                    {
                        field.RemoveValues();
                        await SetRoutes(state, field);
                        return true;
                    })
                    .SetDependencies(nameof(Destination))
                    .SetActive((state) => state.Destination != Destination.Andes)
                )
                .Field(nameof(Experiences))
                .Field(nameof(GroupType))
                .Field(nameof(HowManyPeople),
                    active: (state) =>
                    {
                        return state.GroupType == GroupType.Family;
                    },
                    validate: async (state, response) =>
                    {
                        return await ValidatePeople(state, response);
                    })
                .Confirm(async (state) =>
                {
                    decimal price = await CalculatePrice(state);
                    return new PromptAttribute($"Total price is {price}. Is that ok?");
                })
                .Build();
        }

        private static async Task SetRoutes(RootForm state, Field<RootForm> field)
        {
            switch (state.Destination)
            {
                case Destination.Kilimangiaro:
                    field
                        .AddDescription("Machame", "Machame Route")
                            .AddTerms("Machame", "machame route", "machame")
                        .AddDescription("Marangu", "Marangu Route")
                            .AddTerms("Marangu", "marangu route", "marangu")
                        .AddDescription("Lemosho", "Lemosho Route")
                            .AddTerms("Lemosho", "lemosho route", "lemosho");
                    break;

                case Destination.Himalaya:
                    field
                        .AddDescription("PoonHill", "Poon Hill and the Annapurna Base Camp")
                            .AddTerms("PoonHill", "poon hill", "poon", "annapurna")
                        .AddDescription("Everest", "Everest Base Camp")
                            .AddTerms("Everest", "everest base camp", "everest")
                        .AddDescription("UpperMustang", "Upper Mustang Trek")
                            .AddTerms("UpperMustang", "upper mustang trek", "upper mustang", "mustang");
                    break;
            }
        }

        private static async Task<ValidateResult> ValidatePeople(RootForm state, object response)
        {
            var result = new ValidateResult { IsValid = true, Value = response };

            if (Convert.ToInt32(response) < 1)
            {
                result.Feedback = "There should be at least 1 person.";
                result.IsValid = false;
            }
            else if (Convert.ToInt32(response) > 10)
            {
                result.Feedback = "I can accept maximum 10 people.";
                result.IsValid = false;
            }

            return result;
        }

        private static async Task<decimal> CalculatePrice(RootForm state)
        {
            decimal price = 0.0m;

            switch (state.Destination)
            {
                case Destination.Kilimangiaro: price = 1000.0m; break;
                case Destination.Himalaya: price = 1500.0m; break;
                case Destination.Andes: price = 1800.0m; break;
            }

            price += 200.0m * state.Experiences.Count;

            price *= state.GroupType == GroupType.SoloTraveler || state.GroupType == GroupType.Couple ?
                (int)state.GroupType : state.HowManyPeople;

            return price;
        }
    }
}