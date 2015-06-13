using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Title.Models;
using OrchardHarvest.Models;
using OrchardHarvest.Services;
using OrchardHarvest.ViewModels;

namespace OrchardHarvest.Drivers {
    public class SessionPartDriver : ContentPartDriver<SessionPart> {
        private readonly IContentManager _contentManager;
        private readonly IConferenceService _conferenceService;

        public SessionPartDriver(IContentManager contentManager, IConferenceService conferenceService) {
            _contentManager = contentManager;
            _conferenceService = conferenceService;
        }

        protected override DriverResult Editor(SessionPart part, dynamic shapeHelper) {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SessionPart part, IUpdateModel updater, dynamic shapeHelper) {
            return ContentShape("Parts_Session_Edit", () => {
                var allSpeakers = _contentManager.Query<SpeakerPart>("Speaker").List().ToArray();
                var currentSpeakers = _conferenceService.GetSpeakersBySession(part.Id);
                var viewModel = new EditSessionViewModel {
                    AllSpeakers = allSpeakers,
                    SelectedSpeakers = currentSpeakers.Select(x => new SpeakerViewModel { Id = x.Id, Name = x.As<TitlePart>().Title }).ToList()
                };

                if (updater != null) {
                    if (updater.TryUpdateModel(viewModel, Prefix, null, new[] { "AllSpeakers" })) {
                        var selectedSpeakerIds = viewModel.SelectedSpeakers.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                        _conferenceService.RemoveSpeakersFromSession(part.Id, selectedSpeakerIds);
                        _conferenceService.AddSpeakersToSession(part.Id, selectedSpeakerIds);
                    }
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts.Session", Model: viewModel, Prefix: Prefix);
            });
        }
    }
}