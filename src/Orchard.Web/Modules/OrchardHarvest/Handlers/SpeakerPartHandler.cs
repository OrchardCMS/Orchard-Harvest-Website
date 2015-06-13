using Orchard.ContentManagement.Handlers;
using OrchardHarvest.Models;
using OrchardHarvest.Services;

namespace OrchardHarvest.Handlers {
    public class SpeakerPartHandler : ContentHandler {
        private readonly IConferenceService _conferenceService;

        public SpeakerPartHandler(IConferenceService conferenceService) {
            _conferenceService = conferenceService;
            OnActivated<SpeakerPart>(SetupLazyFields);
        }

        private void SetupLazyFields(ActivatedContentContext context, SpeakerPart part) {
            part._sessionsField.Loader(() => _conferenceService.GetSessionsBySpeaker(part.Id));
        }
    }
}