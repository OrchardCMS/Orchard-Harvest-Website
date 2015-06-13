using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace OrchardHarvest.Models {
    public class TwitterPart : ContentPart {
        [Required]
        public int FeedType {
            get { return this.Retrieve(x => x.FeedType); }
            set { this.Store(x => x.FeedType, value); }
        }

        [Required]
        public string Parameter1 {
            get { return this.Retrieve(x => x.Parameter1); }
            set { this.Store(x => x.Parameter1, value); }
        }

        public string Parameter2 {
            get { return this.Retrieve(x => x.Parameter2); }
            set { this.Store(x => x.Parameter2, value); }
        }

        [Required]
        [DefaultValue("10")]
        public int Count {
            get { return this.Retrieve(x => x.Count, 10); }
            set { this.Store(x => x.Count, value); }
        }

        [Required]
        [DefaultValue("15")]
        public int CacheMinutes {
            get { return this.Retrieve(x => x.CacheMinutes, 15); }
            set { this.Store(x => x.CacheMinutes, value); }
        }

        public bool DisplayProfilePhoto {
            get { return this.Retrieve(x => x.DisplayProfilePhoto); }
            set { this.Store(x => x.DisplayProfilePhoto, value); }
        }

        public bool DisplayName {
            get { return this.Retrieve(x => x.DisplayName); }
            set { this.Store(x => x.DisplayName, value); }
        }

        public bool DisplayScreenName {
            get { return this.Retrieve(x => x.DisplayScreenName); }
            set { this.Store(x => x.DisplayScreenName, value); }
        }

        public bool DisplayHashTags {
            get { return this.Retrieve(x => x.DisplayHashTags); }
            set { this.Store(x => x.DisplayHashTags, value); }
        }

        public bool DisplayTimestamp {
            get { return this.Retrieve(x => x.DisplayTimestamp); }
            set { this.Store(x => x.DisplayTimestamp, value); }
        }

        public bool DisplayNameAsLink {
            get { return this.Retrieve(x => x.DisplayNameAsLink); }
            set { this.Store(x => x.DisplayNameAsLink, value); }
        }

        public bool DisplayScreenNameAsLink {
            get { return this.Retrieve(x => x.DisplayScreenNameAsLink); }
            set { this.Store(x => x.DisplayScreenNameAsLink, value); }
        }

        public bool DisplayMentionAsLink {
            get { return this.Retrieve(x => x.DisplayMentionAsLink); }
            set { this.Store(x => x.DisplayMentionAsLink, value); }
        }

        public bool DisplayHashTagAsLink {
            get { return this.Retrieve(x => x.DisplayHashTagAsLink); }
            set { this.Store(x => x.DisplayHashTagAsLink, value); }
        }

        public bool DisplayUrlAsLink {
            get { return this.Retrieve(x => x.DisplayUrlAsLink); }
            set { this.Store(x => x.DisplayUrlAsLink, value); }
        }

        public bool DisplayTimestampAsLink {
            get { return this.Retrieve(x => x.DisplayTimestampAsLink); }
            set { this.Store(x => x.DisplayTimestampAsLink, value); }
        }
    }
}