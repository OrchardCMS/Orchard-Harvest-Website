using System.Collections.Generic;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;
using OrchardHarvest.Models;

namespace OrchardHarvest.Services {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class MissingTwitterSettingsBanner : Component, INotificationProvider {
        private readonly IOrchardServices _orchardServices;

        public MissingTwitterSettingsBanner(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
        }

        public IEnumerable<NotifyEntry> GetNotifications() {
            var workContext = _orchardServices.WorkContext;
            var settings = workContext.CurrentSite.As<TwitterSettingsPart>();

            if (settings == null || !settings.IsValid()) {
                var urlHelper = new UrlHelper(workContext.HttpContext.Request.RequestContext);
                var url = urlHelper.Action("Twitter", "Admin", new { Area = "Settings" });
                yield return new NotifyEntry { Message = T("The <a href=\"{0}\">Twitter API settings</a> need to be configured.", url), Type = NotifyType.Warning };
            }
        }
    }
}
