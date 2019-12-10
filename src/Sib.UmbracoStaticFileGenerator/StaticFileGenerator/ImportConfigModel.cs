using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class ImportConfigModel
    {
        public string UmbracoRootFolderUrl { get; set; }

        public string TemplatesFolder { get; set; }

        public string HtmlFile301 { get; set; }

        public string FileName301 { get; set; }

        public string HtmlRootFolder { get; set; }

    }
}