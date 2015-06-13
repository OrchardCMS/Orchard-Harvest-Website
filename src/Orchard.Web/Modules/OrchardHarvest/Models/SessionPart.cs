using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;

namespace OrchardHarvest.Models {
    public class SessionPart : ContentPart {
        public LazyField<IEnumerable<SpeakerPart>> _speakersField = new LazyField<IEnumerable<SpeakerPart>>();

        public IEnumerable<SpeakerPart> Speakers {
            get { return _speakersField.Value; }
            set { _speakersField.Value = value; }
        }
    }
}