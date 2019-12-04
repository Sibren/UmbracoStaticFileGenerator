﻿using Sib.UmbracoStaticFileGenerator.Models;
using Sib.UmbracoStaticFileGenerator.Services;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

namespace Sib.UmbracoStaticFileGenerator
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
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        public SubscribeToSaving(IUmbracoContextFactory umbracoContextFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
        }



        private List<UmbracoUrlModel> OldUrls = new List<UmbracoUrlModel>();

        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            ContentService.Saving += ContentService_Saving;
            ContentService.Published += ContentService_Published;
            ContentService.Trashing += ContentService_Trashing;
            //ContentService.Trashed += ContentService_Trashed;
        }

        private void ContentService_Trashing(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var deletedEntity in e.MoveInfoCollection)
            {
                ContentUpdater.Create404(deletedEntity.OriginalPath, _umbracoContextFactory);
            }
        }

        //private void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        //{
            
        //    foreach (var deletedEntity in e.MoveInfoCollection)
        //    {
        //        ContentUpdater.Create404(deletedEntity.OriginalPath, _umbracoContextFactory);
        //    }
        //}

        private void ContentService_Published(IContentService sender, ContentPublishedEventArgs e)
        {
            try
            {
                var list = new List<IPublishedContent>();
                foreach (var savedEntity in e.PublishedEntities)
                {
                    var helper = Umbraco.Web.Composing.Current.UmbracoHelper;
                    var node = helper.Content(savedEntity.Id);
                    list.Add(node);
                }

#pragma warning disable 4014
                var baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
            Task.Factory.StartNew(() => ContentUpdater.DoUponSavedActions(list, _umbracoContextFactory, baseUrl));
#pragma warning restore 4014
                
            }
            catch (Exception ex)
            {
                // Todo: add logging
                e.Messages.Add(new EventMessage("Foutmelding", ex.Message, EventMessageType.Error));
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
                    if (savedEntity.Id == 0)
                    {
                        // this is a new node, don't add it
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
                e.Messages.Add(new EventMessage("Foutmelding", ex.Message, EventMessageType.Error));
            }
        } 
    }
}