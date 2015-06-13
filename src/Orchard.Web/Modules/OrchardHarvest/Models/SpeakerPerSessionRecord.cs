using System;

namespace OrchardHarvest.Models {
    public class SpeakerPerSessionRecord {
        public virtual int Id { get; set; }
        public virtual int SpeakerId { get; set; }
        public virtual int SessionId { get; set; }
    }
}