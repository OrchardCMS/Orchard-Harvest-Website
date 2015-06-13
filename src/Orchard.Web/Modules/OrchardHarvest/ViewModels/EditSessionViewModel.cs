using System.Collections.Generic;
using OrchardHarvest.Models;

namespace OrchardHarvest.ViewModels {
    public class EditSessionViewModel {
        public IEnumerable<SpeakerPart> AllSpeakers { get; set; }
        public IList<SpeakerViewModel> SelectedSpeakers { get; set; }
    }
}