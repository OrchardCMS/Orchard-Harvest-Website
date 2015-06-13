using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Title.Models;
using OrchardHarvest.Models;
using OrchardHarvest.Services;
using OrchardHarvest.ViewModels;

namespace OrchardHarvest.Drivers {
    public class SpeakerPartDriver : ContentPartDriver<SpeakerPart> {
        private readonly IContentManager _contentManager;
        private readonly IConferenceService _conferenceService;

        public SpeakerPartDriver(IContentManager contentManager, IConferenceService conferenceService) {
            _contentManager = contentManager;
            _conferenceService = conferenceService;
        }

        protected override DriverResult Editor(SpeakerPart part, dynamic shapeHelper) {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SpeakerPart part, IUpdateModel updater, dynamic shapeHelper) {
            return ContentShape("Parts_Speaker_Edit", () => {
                var allSessions = _contentManager.Query<SessionPart>("Session").List().ToArray();
                var currentSessions = _conferenceService.GetSessionsBySpeaker(part.Id);
                var viewModel = new EditSpeakerViewModel {
                    AllSessions = allSessions,
                    SelectedSessions = currentSessions.Select(x => new SessionViewModel { Id = x.Id, Name = x.As<TitlePart>().Title}).ToList()
                };

                if (updater != null) {
                    if (updater.TryUpdateModel(viewModel, Prefix, null, new[] {"AllSessions"})) {
                        var selectedSessionIds = viewModel.SelectedSessions.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                        _conferenceService.RemoveSessionsFromSpeaker(part.Id, selectedSessionIds);
                        _conferenceService.AddSessionsToSpeaker(part.Id, selectedSessionIds);
                    }
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts.Speaker", Model: viewModel, Prefix: Prefix);
            });
        }
    }
}