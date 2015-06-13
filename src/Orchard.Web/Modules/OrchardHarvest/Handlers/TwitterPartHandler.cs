using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OrchardHarvest.Caching;
using OrchardHarvest.Models;

namespace OrchardHarvest.Handlers {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterPartHandler : ContentHandler {
        private readonly ISignals _signals;

        public TwitterPartHandler(ISignals signals) {
            _signals = signals;
        }

        protected override void UpdateEditorShape(UpdateEditorContext context) {
            var part = context.ContentItem.As<TwitterPart>();

            if (part != null)
                _signals.Trigger(WellKnownSignals.TwitterPartChanged);

            base.UpdateEditorShape(context);
        }
    }
}