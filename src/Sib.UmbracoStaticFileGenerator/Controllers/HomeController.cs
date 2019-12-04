using Sib.UmbracoStaticFileGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Sib.UmbracoStaticFileGenerator.Controllers
{
    public class HomeController : RenderMvcController
    {
        public ActionResult Index(ContentModel model)
        {
            var viewModel = new HomeViewModel(model.Content);
            return CurrentTemplate(viewModel);
        }
    }
}