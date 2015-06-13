using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OrchardHarvest.Models;
using OrchardHarvest.Services;

namespace OrchardHarvest.Drivers
{
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterPartDriver : ContentPartDriver<TwitterPart> {
        private readonly ITwitterService _twitterService;

        public TwitterPartDriver(ITwitterService twitterService) {
            _twitterService = twitterService;
        }

        protected override DriverResult Display(TwitterPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Twitter", () => shapeHelper.Parts_Twitter(
                Part: part, 
                Tweets: _twitterService.FetchTweetsForPart(part).ToList()));
        }

        protected override DriverResult Editor(TwitterPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Twitter_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "Parts.Twitter", 
                Model: part, 
                Prefix: Prefix));
        }

        protected override DriverResult Editor( TwitterPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}