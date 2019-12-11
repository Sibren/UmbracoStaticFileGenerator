using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class UmbracoUrlModel
    {
        public int UmbracoId { get; set; }

        public string OldUrl { get; set; }

        public UmbracoUrlModel()
        {

        }

        public UmbracoUrlModel(int umbracoId, string oldUrl)
        {
            UmbracoId = umbracoId;
            OldUrl = oldUrl;
        }
    }
}