using System.Collections.Generic;
using LinqToTwitter;
using Orchard;
using OrchardHarvest.Models;

namespace OrchardHarvest.Services {
    public interface ITwitterService : IDependency {
        IEnumerable<Status> FetchTweetsForPart(TwitterPart part);
        IEnumerable<Status> FetchTweetsForScreenName(TwitterPart part);
        IEnumerable<Status> FetchTweetsForList(TwitterPart part);
        IEnumerable<Status> FetchTweetsForFavorites(TwitterPart part);
        IEnumerable<Status> FetchTweetsForSearch(TwitterPart part);
        IEnumerable<Status> FetchTweetsForSearch(string search, int count);
        IEnumerable<TwitterList> FetchListsForScreenName(string screenName);
    }
}