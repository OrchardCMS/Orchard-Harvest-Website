using Orchard.ContentManagement.Handlers;
using OrchardHarvest.Models;
using OrchardHarvest.Services;

namespace OrchardHarvest.Handlers {
    public class SessionPartHandler : ContentHandler {
        private readonly IConferenceService _conferenceService;

        public SessionPartHandler(IConferenceService conferenceService) {
            _conferenceService = conferenceService;
            OnActivated<SessionPart>(SetupLazyFields);
        }

        private void SetupLazyFields(ActivatedContentContext context, SessionPart part) {
            part._speakersField.Loader(() => _conferenceService.GetSpeakersBySession(part.Id));
        }
    }
}