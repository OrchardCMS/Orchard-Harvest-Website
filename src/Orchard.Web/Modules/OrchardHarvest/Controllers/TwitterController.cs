using System.Linq;
using System.Web.Mvc;
using LinqToTwitter;
using Orchard.Environment.Extensions;
using OrchardHarvest.Services;

namespace OrchardHarvest.Controllers {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterController : Controller {
        private readonly ITwitterService _twitterService;

        public TwitterController(ITwitterService twitterService) {
            _twitterService = twitterService;
        }

        [HttpPost]
        public JsonResult Search() {
            var tweets = _twitterService.FetchTweetsForSearch("#orchardcms", 5);
            var jQueryTweetData = new {
                response = new {
                    statuses = tweets.Select(MapStatus)
                }
            };
            return Json(jQueryTweetData);
        }

        private static object MapStatus(Status status) {
            if (status == null || status.StatusID == null)
                return null;

            return new {
                created_at = status.CreatedAt.ToString("F"),
                text = status.Text,
                id_str = status.StatusID,
                screen_name = status.User.Identifier.ScreenName,
                retweeted_status = MapStatus(status.RetweetedStatus),
                user = new {
                    name = status.User.Name,
                    screen_name = status.User.Identifier.ScreenName,
                    profile_image_url = status.User.ProfileImageUrl,
                    profile_image_url_https = status.User.ProfileImageUrlHttps,
                }
            };
        }
    }
}