using System.Collections.Generic;
using OrchardHarvest.Models;

namespace OrchardHarvest.ViewModels {
    public class EditSpeakerViewModel {
        public IEnumerable<SessionPart> AllSessions { get; set; }
        public IList<SessionViewModel> SelectedSessions { get; set; }
    }
}