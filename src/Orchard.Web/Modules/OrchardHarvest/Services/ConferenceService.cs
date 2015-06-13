using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Data;
using OrchardHarvest.Models;

namespace OrchardHarvest.Services {
    public class ConferenceService : IConferenceService {
        private readonly IRepository<SpeakerPerSessionRecord> _speakerPerSessionRepository;
        private readonly IContentManager _contentManager;

        public ConferenceService(IRepository<SpeakerPerSessionRecord> speakerPerSessionRepository, IContentManager contentManager) {
            _speakerPerSessionRepository = speakerPerSessionRepository;
            _contentManager = contentManager;
        }

        public IEnumerable<SessionPart> GetSessionsBySpeaker(int speakerId, VersionOptions versionOptions = null) {
            var ids = from record in _speakerPerSessionRepository.Table
                      where record.SpeakerId == speakerId
                      select record.SessionId;

            versionOptions = versionOptions ?? VersionOptions.Published;
            return _contentManager.GetMany<SessionPart>(ids, versionOptions, QueryHints.Empty);
        }

        public IEnumerable<SpeakerPart> GetSpeakersBySession(int sessionId, VersionOptions versionOptions = null) {
            var ids = from record in _speakerPerSessionRepository.Table
                      where record.SessionId == sessionId
                      select record.SpeakerId;

            versionOptions = versionOptions ?? VersionOptions.Published;
            return _contentManager.GetMany<SpeakerPart>(ids, versionOptions, QueryHints.Empty);
        }

        public void RemoveSessionsFromSpeaker(int speakerId, IEnumerable<int> except = null) {
            except = except ?? Enumerable.Empty<int>();

            foreach (var record in _speakerPerSessionRepository.Table.Where(x => x.SpeakerId == speakerId && !except.Contains(x.SessionId))) {
                _speakerPerSessionRepository.Delete(record);
            }
        }

        public void AddSessionsToSpeaker(int speakerId, IEnumerable<int> sessionIds) {
            var existingSessionIdsQuery = from record in _speakerPerSessionRepository.Table
                                          where record.SpeakerId == speakerId && sessionIds.Contains(record.SessionId)
                                          select record.SessionId;

            var existingSessionIds = existingSessionIdsQuery.ToArray();

            foreach (var sessionId in from sessionId in sessionIds where !existingSessionIds.Contains(sessionId) select sessionId) {

                _speakerPerSessionRepository.Create(new SpeakerPerSessionRecord {
                    SpeakerId = speakerId,
                    SessionId = sessionId
                });
            }
        }

        public void RemoveSpeakersFromSession(int sessionId, IEnumerable<int> except = null) {
            except = except ?? Enumerable.Empty<int>();

            foreach (var record in _speakerPerSessionRepository.Table.Where(x => x.SessionId == sessionId && !except.Contains(x.SpeakerId))) {
                _speakerPerSessionRepository.Delete(record);
            }
        }

        public void AddSpeakersToSession(int sessionId, IEnumerable<int> speakerIds) {
            var existingSpeakerIdsQuery = from record in _speakerPerSessionRepository.Table
                                          where record.SessionId == sessionId && speakerIds.Contains(record.SpeakerId)
                                          select record.SpeakerId;

            var existingSpeakerIds = existingSpeakerIdsQuery.ToArray();

            foreach (var speakerId in from speakerId in speakerIds where !existingSpeakerIds.Contains(speakerId) select speakerId) {

                _speakerPerSessionRepository.Create(new SpeakerPerSessionRecord {
                    SessionId = sessionId,
                    SpeakerId = speakerId
                });
            }
        }
    }
}