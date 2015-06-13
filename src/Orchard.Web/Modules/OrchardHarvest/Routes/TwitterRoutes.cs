using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace OrchardHarvest.Routes {
    [OrchardFeature("OrchardHarvest.Twitter")]
    public class TwitterRoutes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            yield return new RouteDescriptor {
                Route = new Route(
                    "Admin/OrchardHarvest/Twitter/FetchLists",
                    new RouteValueDictionary {
                        {"area", "OrchardHarvest"},
                        {"controller", "TwitterAdmin"},
                        {"action", "FetchLists"}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary {
                        {"area", "OrchardHarvest"}
                    },
                    new MvcRouteHandler())
            };
        }
    }
}