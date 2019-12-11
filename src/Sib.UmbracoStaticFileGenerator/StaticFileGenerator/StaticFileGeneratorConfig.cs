using System.Configuration;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class StaticFileGeneratorConfig
    {
        /// <summary>
        /// Is this enabled in the web.config?
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        /// We need Umbraco.Core.HideTopLevelNodeFromPath to be set to false in the web.config
        /// </summary>
        public bool HideTopNodeSettingNeedsToBeFalse { get; }

        public StaticFileGeneratorConfig()
        {
            const string prefix = "Sib.UmbracoStaticFileGenerator.";

            IsEnabled = ConfigurationManager.AppSettings[prefix + "Enabled"] == "true";
            HideTopNodeSettingNeedsToBeFalse = ConfigurationManager.AppSettings["Umbraco.Core.HideTopLevelNodeFromPath"] == "true";
        }
    }
}