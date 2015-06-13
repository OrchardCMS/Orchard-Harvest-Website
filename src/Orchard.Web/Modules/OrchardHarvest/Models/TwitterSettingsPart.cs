using System;
using System.Linq;
using Orchard.ContentManagement;

namespace OrchardHarvest.Models {
    public class TwitterSettingsPart : ContentPart {
        public string ConsumerKey {
            get { return this.Retrieve(x => x.ConsumerKey); }
            set { this.Store(x => x.ConsumerKey, value); }
        }

        public string ConsumerSecret {
            get { return this.Retrieve(x => x.ConsumerSecret); }
            set { this.Store(x => x.ConsumerSecret, value); }
        }

        public string AccessToken {
            get { return this.Retrieve(x => x.AccessToken); }
            set { this.Store(x => x.AccessToken, value); }
        }

        public string AccessTokenSecret {
            get { return this.Retrieve(x => x.AccessTokenSecret); }
            set { this.Store(x => x.AccessTokenSecret, value); }
        }

        public bool IsValid() {
            return !new[] {ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret}.Any(String.IsNullOrWhiteSpace);
        }
    }
}