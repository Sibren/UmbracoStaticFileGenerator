using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Web;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public class UponSaveContent : IUserComposer
    {
        public void Compose(Composition composition)
        {
            // Append our component to the collection of Components
            // It will be the last one to be run
            composition.Components().Append<SubscribeToSaving>();
        }
    }

    public class SubscribeToSaving : IComponent
    {
        //Todo: dependency injection
        private readonly IUmbracoContextFactory umbracoContextFactory;
        private readonly StaticFileGeneratorConfig config;

        public SubscribeToSaving(IUmbracoContextFactory umbracoContextFactory, StaticFileGeneratorConfig config)
        {
            this.umbracoContextFactory = umbracoContextFactory;
            this.config = config;
        }

        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            if (config.IsEnabled)
            {
                ContentService.Saving += ContentService_Saving;
                ContentService.Published += ContentService_Published;
                ContentService.Trashing += ContentService_Trashing;
            }

        }

        private void ContentService_Trashing(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var deletedEntity in e.MoveInfoCollection)
            {
                ContentUpdater.DeleteFilesAndFolders(deletedEntity.OriginalPath, umbracoContextFactory);
            }
        }

        private void ContentService_Published(IContentService sender, ContentPublishedEventArgs e)
        {
            try
            {
                var list = new List<IPublishedContent>();
                // don't listen to items without a template or a redirect
                foreach (var savedEntity in e.PublishedEntities)
                {
                    if (savedEntity.Properties.Any(x => x.Alias == StandingData.UmbracoInternalRedirectIdName))
                    {
                        var value = savedEntity.Properties.FirstOrDefault(x => x.Alias == StandingData.UmbracoInternalRedirectIdName);
                        var currentRedirectValue = value.GetValue();
                        if (currentRedirectValue != null)
                        {
                            var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                            var node = helper.Content(savedEntity.Id);
                            list.Add(node);
                        }

                    }
                    else if (savedEntity.TemplateId > 0)
                    {
                        var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                        var node = helper.Content(savedEntity.Id);
                        list.Add(node);
                    }
                }



#pragma warning disable 4014
                if (list.Any())
                {

                    var baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
                    Task.Factory.StartNew(() => ContentUpdater.DoUponSavedActionsWithWait(list, umbracoContextFactory, baseUrl));
                }
#pragma warning restore 4014

            }
            catch (Exception ex)
            {
                // Todo: add logging
                e.Messages.Add(new EventMessage("Error", ex.Message, EventMessageType.Error));
            }
        }

        // terminate: runs once when Umbraco stops
        public void Terminate()
        { }

        private void ContentService_Saving(IContentService sender, ContentSavingEventArgs e)
        {
            try
            {
                foreach (var savedEntity in e.SavedEntities)
                {
                    if (savedEntity.Id == 0 || savedEntity.TemplateId == 0)
                    {
                        // this is a new node or it doesn't have a template, don't add it
                        continue;
                    }
                    var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                    var node = helper.Content(savedEntity.Id);
                    var oldLink = node.Url;

                    StandingData.OldModels.Add(new UmbracoUrlModel(savedEntity.Id, node.Url()));
                }
            }
            catch (Exception ex)
            {
                // Todo: add logging
                e.Cancel = true;
                e.Messages.Add(new EventMessage("Error", ex.Message, EventMessageType.Error));
            }
        }
    }
}