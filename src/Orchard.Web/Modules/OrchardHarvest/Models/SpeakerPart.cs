using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;

namespace OrchardHarvest.Models {
    public class SpeakerPart : ContentPart {
        public LazyField<IEnumerable<SessionPart>> _sessionsField = new LazyField<IEnumerable<SessionPart>>();

        public IEnumerable<SessionPart> Sessions {
            get { return _sessionsField.Value; }
            set { _sessionsField.Value = value; }
        }
    }
}