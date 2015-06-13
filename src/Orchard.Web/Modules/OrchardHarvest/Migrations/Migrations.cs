using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace OrchardHarvest.Migrations {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("SpeakerPerSessionRecord", table => table
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("SpeakerId")
                .Column<int>("SessionId"));

            ContentDefinitionManager.AlterPartDefinition("SpeakerPart", part => part
                .WithField("Photo", f => f
                    .OfType("MediaLibraryPickerField")
                    .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "false")
                    .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Image"))
                .WithField("Subtitle", f => f
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Wide"))
                .WithField("LinkedIn", f => f
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Large"))
                .WithField("Google", f => f
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Large"))
                .WithField("Facebook", f => f
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Large"))
                .WithField("Twitter", f => f
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Twitter", "Large"))
                .WithField("Website", f => f
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Website", "Large"))
                .WithDescription("Turns your type into an Orchard Harvest Speaker"));

            ContentDefinitionManager.AlterPartDefinition("SessionPart", part => part
                .WithField("Start", f => f
                    .OfType("DateTimeField")
                    .WithSetting("DateTimeFieldSettings.Required", "true")
                    .WithSetting("DateTimeFieldSettings.Display", "DateAndTime"))
                .WithField("End", f => f
                    .OfType("DateTimeField")
                    .WithSetting("DateTimeFieldSettings.Required", "true")
                    .WithSetting("DateTimeFieldSettings.Display", "DateAndTime"))
                .WithField("IconCssClass", f => f
                    .OfType("TextField")
                    .WithDisplayName("Icon Css Class")
                    .WithSetting("TextFieldSettings.Website", "Wide")
                    .WithSetting("TextFieldSettings.Hint", "Optional. Specify a Font Awesome class, e.g. fa-star."))
                .WithDescription("Turns your type into an Orchard Harvest Session"));

            ContentDefinitionManager.AlterTypeDefinition("Speaker", type => type
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                .WithPart("AutoroutePart", builder => builder
                    .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                    .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                    .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: 'speakers/{Content.Slug}', Description: 'john-doe'}]")
                    .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                .WithPart("BodyPart")
                .WithPart("SpeakerPart")
                .Draftable()
                .Creatable());

            ContentDefinitionManager.AlterTypeDefinition("Session", type => type
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                .WithPart("AutoroutePart", builder => builder
                    .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                    .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                    .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: 'sessions/{Content.Slug}', Description: 'my-session'}]")
                    .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                .WithPart("BodyPart")
                .WithPart("SessionPart")
                .Draftable()
                .Creatable());

            return 1;
        }
    }
}