using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using Sib.UmbracoStaticFileGenerator.Controllers.StaticFileGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;
using PhysicalFileSystem = Microsoft.Owin.FileSystems.PhysicalFileSystem;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    /// <summary>
    /// Leave as is unless you know what you're doing
    /// </summary>
    public class MyComposer : ComponentComposer<StartupCustomRoutingComponent>, ICoreComposer
    {
        public override void Compose(Composition composition)
        {
            base.Compose(composition);

            composition.Configs.Add(() => new StaticFileGeneratorConfig());
        }
    }

    public class StartupCustomRoutingComponent : IComponent
    {
        public void Initialize()
        {
            try
            {
                if (File.Exists(StandingData.ConfigFile))
                {
                    string jsonConfig = File.ReadAllText(StandingData.ConfigFile);
                    var configModel = JsonConvert.DeserializeObject<ImportConfigModel>(jsonConfig);

                    if (!string.IsNullOrEmpty(configModel.FileName301)) StandingData.FileName301 = configModel.FileName301;
                    if (!string.IsNullOrEmpty(configModel.HtmlFile301)) StandingData.HtmlFile301 = configModel.HtmlFile301;
                    if (!string.IsNullOrEmpty(configModel.HtmlRootFolder)) StandingData.HtmlRootFolder = configModel.HtmlRootFolder;
                    if (!string.IsNullOrEmpty(configModel.TemplatesFolder)) StandingData.TemplatesFolder = configModel.TemplatesFolder;
                    if (!string.IsNullOrEmpty(configModel.UmbracoRootFolderUrl)) StandingData.UmbracoRootFolderUrl = configModel.UmbracoRootFolderUrl;
                }

            }
            catch (Exception)
            {

            }
            UmbracoDefaultOwinStartup.MiddlewareConfigured += (_, e) => ConfigureMiddleware(e.AppBuilder);
            InstallServerVars();
        }

        private void InstallServerVars()
        {
            // register our url - for the backoffice api
            ServerVariablesParser.Parsing += (sender, serverVars) =>
            {
                if (!serverVars.ContainsKey("umbracoUrls"))
                    throw new Exception("Missing umbracoUrls.");
                var umbracoUrlsObject = serverVars["umbracoUrls"];
                if (umbracoUrlsObject == null)
                    throw new Exception("Null umbracoUrls");
                if (!(umbracoUrlsObject is Dictionary<string, object> umbracoUrls))
                    throw new Exception("Invalid umbracoUrls");

                if (!serverVars.ContainsKey("umbracoPlugins"))
                    throw new Exception("Missing umbracoPlugins.");
                if (!(serverVars["umbracoPlugins"] is Dictionary<string, object> umbracoPlugins))
                    throw new Exception("Invalid umbracoPlugins");

                if (HttpContext.Current == null) throw new InvalidOperationException("HttpContext is null");
                var urlHelper = new System.Web.Mvc.UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

                umbracoUrls[StandingData.ApplicationName] = urlHelper.GetUmbracoApiServiceBaseUrl<StaticSiteGeneratorController>(controller => controller.Startup());
            };
        }

        private void ConfigureMiddleware(IAppBuilder app)
        {
            var physicalFileSystem = new PhysicalFileSystem(@"./" + StandingData.HtmlRootFolder);

            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[]
            {
                "index.html"
            };

            app.UseFileServer(options);
        }

        public void Terminate()
        {

        }
    }
}