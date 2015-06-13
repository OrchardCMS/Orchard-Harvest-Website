using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using LinqToTwitter;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Services;
using OrchardHarvest.Caching;
using OrchardHarvest.Models;

namespace OrchardHarvest.Services {
    [UsedImplicitly]
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterService : Component, ITwitterService {
        private const string CacheKeyPrefix = "OrchardHarvest.TwitterApi";
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;
        private readonly ISignals _signals;
        private readonly IOrchardServices _services;

        public TwitterService(ICacheManager cacheManager, IClock clock, ISignals signals, IOrchardServices services) {
            _cacheManager = cacheManager;
            _clock = clock;
            _signals = signals;
            _services = services;
        }

        public IEnumerable<Status> FetchTweetsForPart(TwitterPart part) {
            var cacheKey = CacheKeyPrefix + part.Id;

            return _cacheManager.Get(cacheKey, ctx => {
                ctx.Monitor(_signals.When(WellKnownSignals.TwitterPartChanged));
                ctx.Monitor(_clock.When(TimeSpan.FromMinutes(part.CacheMinutes)));
                return FetchTweets(part).ToList();
            });
        }

        protected IEnumerable<Status> FetchTweets(TwitterPart part) {
            IEnumerable<Status> tweets;

            switch (part.FeedType) {
                case (int)FeedType.Profile:
                    tweets = FetchTweetsForScreenName(part);
                    break;
                case (int)FeedType.List:
                    tweets = FetchTweetsForList(part);
                    break;
                case (int)FeedType.Favorites:
                    tweets = FetchTweetsForFavorites(part);
                    break;
                default:
                    tweets = FetchTweetsForSearch(part);
                    break;
            }

            return tweets;
        }

        public IEnumerable<Status> FetchTweetsForScreenName(TwitterPart part) {
            var screenName = part.Parameter1.Trim();
            var count = part.Count;
            var context = CreateContext();
            return context.Status.Where(x =>
                x.Type == StatusType.User &&
                x.ScreenName == screenName &&
                x.Count == count);
        }

        public IEnumerable<Status> FetchTweetsForList(TwitterPart part) {
            var listId = part.Parameter2.Trim();
            var count = part.Count;
            var context = CreateContext();
            return context.List.Where(x =>
                x.ListID == listId &&
                x.Count == count)
                .SelectMany(x => x.Statuses);
        }

        public IEnumerable<Status> FetchTweetsForFavorites(TwitterPart part) {
            var screenName = part.Parameter1.Trim();
            var count = part.Count;
            var context = CreateContext();
            return context.Favorites.Where(x =>
                x.ScreenName == screenName &&
                x.Count == count);
        }

        public IEnumerable<Status> FetchTweetsForSearch(TwitterPart part) {
            return FetchTweetsForSearch(part.Parameter1, part.Count);
        }

        public IEnumerable<Status> FetchTweetsForSearch(string search, int count) {
            var searchTerms = HttpUtility.UrlEncode(search.Trim());
            var context = CreateContext();

            return context.Search.Where(x =>
                x.Type == SearchType.Search &&
                x.Query == searchTerms &&
                x.Count == count)
            .SelectMany(x => x.Statuses);
        }

        public IEnumerable<TwitterList> FetchListsForScreenName(string screenName) {
            if (String.IsNullOrWhiteSpace(screenName))
                return Enumerable.Empty<TwitterList>();

            var context = CreateContext();
            return context.List.Where(x =>
                x.ScreenName == screenName)
                .Select(x => new TwitterList {
                    Id = x.ListID,
                    Name = x.Name
                });
        }

        private TwitterContext CreateContext() {
            var settings = _services.WorkContext.CurrentSite.As<TwitterSettingsPart>();
            return new TwitterContext(new SingleUserAuthorizer {
                Credentials = new SingleUserInMemoryCredentials {
                    ConsumerKey = settings.ConsumerKey,
                    ConsumerSecret = settings.ConsumerSecret,
                    TwitterAccessToken = settings.AccessToken,
                    TwitterAccessTokenSecret = settings.AccessTokenSecret
                },
                AuthAccessType = AuthAccessType.Read
            });
        }
    }
}
