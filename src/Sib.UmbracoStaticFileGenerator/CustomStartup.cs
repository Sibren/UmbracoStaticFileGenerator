using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Web;

namespace Sib.UmbracoStaticFileGenerator
{
    //public class MyComposer : ComponentComposer<MyComponent>, IUserComposer
    //{

    //}​

    public class MyComposer : ComponentComposer<StartupCustomRoutingComponent>, IUserComposer
    {

    }

    public class StartupCustomRoutingComponent : IComponent
    {
        public void Initialize()
        {
            UmbracoDefaultOwinStartup.MiddlewareConfigured += (_, e) => ConfigureMiddleware(e.AppBuilder);
        }

        private void ConfigureMiddleware(IAppBuilder app)
        {
            var physicalFileSystem = new PhysicalFileSystem(@"./www");

            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[]
            {
                "index.html"
            };

            app.UseFileServer(options);
        }

        public void Terminate()
        {
            
        }
    }

    public class SubscribeToContentServiceSavingComponent : IComponent
    {
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            ContentService.Saving += ContentService_Saving;
        }

        // terminate: runs once when Umbraco stops
        public void Terminate()
        { }

        private void ContentService_Saving(IContentService sender, ContentSavingEventArgs e)
        {
            foreach (var content in e.SavedEntities
                // Check if the content item type has a specific alias
                .Where(c => c.ContentType.Alias.InvariantEquals("MyContentType")))
            {
                // Do something if the content is using the MyContentType doctype
            }
        }
    }
}