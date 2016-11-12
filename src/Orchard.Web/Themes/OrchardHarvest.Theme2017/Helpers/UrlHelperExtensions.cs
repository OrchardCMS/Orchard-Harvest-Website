using System;
using System.Web.Mvc;

namespace OrchardHarvest.Theme2017.Helpers {
    public static class UrlHelperExtensions {
        public static string HomePageUrl(this UrlHelper urlHelper, string anchor) {
            var homePath = urlHelper.Content("~/");
            var isHomePage = urlHelper.RequestContext.HttpContext.Request.Path == homePath;
            return isHomePage ? anchor : String.Format("{0}{1}", homePath, anchor);
        }
    }
}