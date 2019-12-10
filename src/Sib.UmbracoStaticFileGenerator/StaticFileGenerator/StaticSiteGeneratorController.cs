using Sib.UmbracoStaticFileGenerator.StaticFileGenerator;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using Umbraco.Web.Editors;

namespace Sib.UmbracoStaticFileGenerator.Controllers.StaticFileGenerator
{
    public class StaticSiteGeneratorController : UmbracoAuthorizedJsonController
    {
        private readonly StaticFileGeneratorConfig config;
        
        public StaticSiteGeneratorController(StaticFileGeneratorConfig config)
        {
            this.config = config;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveConfig(ImportConfigModel model)
        {
            StandingData.SaveStandingData(model);
            return Request.CreateResponse(HttpStatusCode.OK, Configuration.Formatters.JsonFormatter);
        }

        // invoked by the back-office
        // requires that the user is logged into the backoffice and has access to the settings section
        // beware! the name of the method appears in modelsbuilder.controller.js
        [System.Web.Http.HttpGet] // use the http one, not mvc, with api controllers!
        public HttpResponseMessage GetDashboard()
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetDashboardResult(), Configuration.Formatters.JsonFormatter);
        }

        // invoked by the dashboard
        // requires that the user is logged into the backoffice and has access to the settings section
        // beware! the name of the method appears in modelsbuilder.controller.js
        [System.Web.Http.HttpPost] // use the http one, not mvc, with api controllers!
        public HttpResponseMessage Startup()
        {
            try
            {
                // todo
            }
            catch (Exception e)
            {
                // Todo
            }

            return Request.CreateResponse(HttpStatusCode.OK, GetDashboardResult(), Configuration.Formatters.JsonFormatter);
        }

        private Dashboard GetDashboardResult()
        {
            return new Dashboard
            {
                IsEnabled = config.IsEnabled,
                Text = GetText(),
                Config = Config()
            };
        }

        private ImportConfigModel Config()
        {
            var configModel = new ImportConfigModel();

            configModel.FileName301 = StandingData.FileName301;
            configModel.HtmlFile301 = StandingData.HtmlFile301;
            configModel.HtmlRootFolder = StandingData.HtmlRootFolder;
            configModel.TemplatesFolder = StandingData.TemplatesFolder;
            configModel.UmbracoRootFolderUrl = StandingData.UmbracoRootFolderUrl;

            return configModel;
        }

        private string GetText()
        {
            if (!config.IsEnabled)
            {
                return "Not enabled";
            } else
            {
                return "Welcome to a new episode of Jackass";
            }
        }

        [DataContract]
        internal class Dashboard
        {
            [DataMember(Name = "enable")]
            public bool IsEnabled;

            [DataMember(Name = "text")]
            public string Text;

            [DataMember(Name = "config")]
            public ImportConfigModel Config;
        }
    }
}
