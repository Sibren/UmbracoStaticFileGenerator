using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Umbraco.Core.Security;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class ContentFinderForCheckingWebsiteUrl : IContentFinder
    {

        public bool TryFindContent(PublishedRequest contentRequest)
        {
            // handle all requests beginning /website...
            var path = contentRequest.Uri.AbsolutePath;
            HttpContextBase wrapper = new HttpContextWrapper(HttpContext.Current);
            UmbracoBackOfficeIdentity user = wrapper.GetCurrentIdentity(true);
            bool isLoggedIn = user != null;
            if (!isLoggedIn)
            {
                if (path.StartsWith("/" + StandingData.UmbracoRootFolderUrl))
                {
                    contentRequest.Is404 = true;
                    HttpContext.Current.Response.Redirect(path.Replace("/" + StandingData.UmbracoRootFolderUrl, ""));
                }
            }
            
            return true;
        }

    }
}