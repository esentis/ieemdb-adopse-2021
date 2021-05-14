namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Abstractions;
  using Microsoft.AspNetCore.Mvc.ModelBinding;
  using Microsoft.AspNetCore.Mvc.Razor;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.AspNetCore.Mvc.ViewEngines;
  using Microsoft.AspNetCore.Mvc.ViewFeatures;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.Logging;

  public class RazorViewToStringRenderer
  {
    private const string CouldNotLocateView = "View {View} not located in any of the following paths {Locations}";

    private readonly IRazorViewEngine engine;
    private readonly ITempDataProvider tempDataProvider;
    private readonly IServiceProvider provider;
    private readonly ILogger<RazorViewToStringRenderer> logger;

    public RazorViewToStringRenderer(
      IRazorViewEngine engine,
      ITempDataProvider tempDataProvider,
      IServiceProvider provider,
      ILogger<RazorViewToStringRenderer> logger)
    {
      this.engine = engine;
      this.tempDataProvider = tempDataProvider;
      this.provider = provider;
      this.logger = logger;
    }

    public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
    {
      var context = new ActionContext(
        new DefaultHttpContext { RequestServices = provider },
        new RouteData(),
        new ActionDescriptor());
      var view = FindView(context, viewName);

      using var output = new StringWriter();
      var viewContext = new ViewContext(
        context,
        view,
        new ViewDataDictionary<TModel>(
          new EmptyModelMetadataProvider(),
          new ModelStateDictionary()) { Model = model },
        new TempDataDictionary(context.HttpContext, tempDataProvider),
        output,
        new HtmlHelperOptions());

      await view.RenderAsync(viewContext);

      return output.ToString();
    }

    private IView FindView(ActionContext context, string viewName)
    {
      var viewResult = engine.GetView(null, viewName, true);
      if (viewResult.Success)
      {
        return viewResult.View;
      }

      var contextViewResult = engine.FindView(context, viewName, true);
      if (contextViewResult.Success)
      {
        return contextViewResult.View;
      }

      var locations = viewResult.SearchedLocations.Concat(contextViewResult.SearchedLocations);

      logger.LogCritical(CouldNotLocateView, viewName, locations);

      throw new InvalidOperationException($"Could not locate {viewName} in {locations}");
    }
  }
}
