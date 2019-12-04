using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Sib.UmbracoStaticFileGenerator.ViewModels
{
    public class HomeViewModel : ContentModel
    {
        public string Title = "Mijn awesome title";

        public HomeViewModel(IPublishedContent content) : base(content)
        {
        }
    }
}