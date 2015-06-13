using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OrchardHarvest.Models;

namespace OrchardHarvest.Handlers {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterSettingsPartHandler : ContentHandler {
        public Localizer T { get; set; }

        public TwitterSettingsPartHandler() {
            Filters.Add(new ActivatingFilter<TwitterSettingsPart>("Site"));
            T = NullLocalizer.Instance;
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Twitter")) {
                Id = "Twitter",
                Position = "16"
            });
        }
    }
}