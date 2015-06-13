using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using OrchardHarvest.Models;

namespace OrchardHarvest.Migrations {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterMigrations : DataMigrationImpl {
        public int Create() {

            ContentDefinitionManager.AlterPartDefinition(typeof(TwitterPart).Name,
                builder => builder.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("TwitterWidget", cfg => cfg
                .WithPart("TwitterPart")
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            return 1;
        }
    }
}