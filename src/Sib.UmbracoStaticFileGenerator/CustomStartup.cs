using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace Sib.UmbracoStaticFileGenerator
{
    /// <summary>
    /// Leave as is unless you know what you're doing
    /// </summary>
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
}