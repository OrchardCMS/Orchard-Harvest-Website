using System.Linq;
using System.Web.Mvc;
using Orchard.Environment.Extensions;
using OrchardHarvest.Services;

namespace OrchardHarvest.Controllers {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterAdminController : Controller {
        private readonly ITwitterService _twitterService;

        public TwitterAdminController(ITwitterService twitterService) {
            _twitterService = twitterService;
        }

        [HttpPost]
        public JsonResult FetchLists(string screenName) {
            var lists = _twitterService.FetchListsForScreenName(screenName).ToArray();
            return Json(lists);
        }
    }
}