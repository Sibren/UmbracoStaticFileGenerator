using System.Configuration;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class StaticFileGeneratorConfig
    {
        public bool IsEnabled { get; }

        public StaticFileGeneratorConfig()
        {
            const string prefix = "Sib.UmbracoStaticFileGenerator.";

            IsEnabled = ConfigurationManager.AppSettings[prefix + "Enabled"] == "true";
        }
    }
}