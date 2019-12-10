using System.Configuration;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class StaticFileGeneratorConfig
    {
        public bool Enable { get; }

        public StaticFileGeneratorConfig()
        {
            const string prefix = "Sib.UmbracoStaticFileGenerator.";

            Enable = ConfigurationManager.AppSettings[prefix + "Enabled"] == "true";
        }
    }
}