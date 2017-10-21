using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot02.Forms
{
    public enum Destination
    {
        [Terms("kili", "kilimangiaro")]
        Kilimangiaro = 1,

        Himalaya,

        Andes
    }

    public enum Experience
    {
        [Terms("cultural and tribal", "cultural", "culture", "tribal", "tribe")]
        CulturalAndTribal = 1,

        Hiking,

        [Terms("wild safari", "wild", "safari")]
        WildSafari
    }

    public enum GroupType
    {
        [Terms("solo traveler", "solo")]
        SoloTraveler = 1,

        Couple = 2,

        Family,
    }
}