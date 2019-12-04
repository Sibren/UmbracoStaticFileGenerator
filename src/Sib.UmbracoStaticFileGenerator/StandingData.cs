using Sib.UmbracoStaticFileGenerator.Models;
using System.Collections.Generic;

namespace Sib.UmbracoStaticFileGenerator
{
    public static class StandingData
    {
        public static List<UmbracoUrlModel> OldModels { get; set; } = new List<UmbracoUrlModel>();
        public static List<UmbracoUrlModel> NewModels { get; set; } = new List<UmbracoUrlModel>();
    }
}