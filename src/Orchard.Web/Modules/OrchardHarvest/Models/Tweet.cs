using System;

namespace OrchardHarvest.Models {
    public class Tweet {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string Message { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}