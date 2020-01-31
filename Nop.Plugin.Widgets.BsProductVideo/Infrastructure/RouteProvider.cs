
using Nop.Web.Framework.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Nop.Plugin.Widgets.BsProductVideo.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
           
           
        }

        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //throw new System.NotImplementedException();
        }

        public int Priority
        {
            get
            {
                return 2;
            }
        }
    }
}
