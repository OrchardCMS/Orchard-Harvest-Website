using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OrchardHarvest.Models;

namespace OrchardHarvest.Drivers {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterSettingsPartDriver : ContentPartDriver<TwitterSettingsPart> {
        protected override string Prefix {
            get { return "TwitterSettingsPart"; }
        }

        protected override DriverResult Editor(TwitterSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_TwitterSettings_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts.TwitterSettings", Model: part, Prefix: Prefix)).OnGroup("Twitter");
        }

        protected override DriverResult Editor(TwitterSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(TwitterSettingsPart part, ExportContentContext context) {
            var partName = part.PartDefinition.Name;

            context.Element(partName).SetAttributeValue("ConsumerKey", part.ConsumerKey);
            context.Element(partName).SetAttributeValue("ConsumerSecret", part.ConsumerSecret);
            context.Element(partName).SetAttributeValue("AccessToken", part.AccessToken);
            context.Element(partName).SetAttributeValue("AccessTokenSecret", part.AccessTokenSecret);
        }

        protected override void Importing(TwitterSettingsPart part, ImportContentContext context) {
            var partName = part.PartDefinition.Name;

            context.ImportAttribute(partName, "ConsumerKey", value => part.ConsumerKey = value);
            context.ImportAttribute(partName, "ConsumerSecret", value => part.ConsumerSecret = value);
            context.ImportAttribute(partName, "AccessToken", value => part.AccessToken = value);
            context.ImportAttribute(partName, "AccessTokenSecret", value => part.AccessTokenSecret = value);
        }
    }
}