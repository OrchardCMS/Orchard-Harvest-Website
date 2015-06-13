using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;
using OrchardHarvest.Models;

namespace OrchardHarvest.Services {
    public interface IConferenceService : IDependency {
        IEnumerable<SessionPart> GetSessionsBySpeaker(int speakerId, VersionOptions versionOptions = null);
        IEnumerable<SpeakerPart> GetSpeakersBySession(int sessionId, VersionOptions versionOptions = null);
        void RemoveSessionsFromSpeaker(int speakerId, IEnumerable<int> except = null);
        void AddSessionsToSpeaker(int speakerId, IEnumerable<int> sessionIds);
        void RemoveSpeakersFromSession(int sessionId, IEnumerable<int> except = null);
        void AddSpeakersToSession(int sessionId, IEnumerable<int> speakerIds);
    }
}