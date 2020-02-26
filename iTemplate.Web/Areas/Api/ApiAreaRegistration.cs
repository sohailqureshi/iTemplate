using System.Web.Http;
using System.Web.Mvc;

namespace iTemplate.Web.Areas
{
  public class ApiAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Api";
      }
    }

    //public override void RegisterArea(AreaRegistrationContext context)
    //{
    //  context.MapRoute(
    //       "DefaultApiWithAction",
    //       "Api/{controller}/{action}/{id}",
    //      new { action = "Index", id = UrlParameter.Optional }
    //  );
    //}

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.Routes.MapMvcAttributeRoutes();
      context.Routes.MapHttpRoute(
          name: this.AreaName,
          routeTemplate: this.AreaName + "/{controller}/{action}/{id}",
          defaults: new { action = "Index", id = RouteParameter.Optional }
      );
    }
  }
}