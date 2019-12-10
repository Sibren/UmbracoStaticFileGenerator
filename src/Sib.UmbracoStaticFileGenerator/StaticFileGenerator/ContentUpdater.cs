using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public static class ContentUpdater
    {

        private static readonly object lockObject = new object();


        public static void DoUponSavedActions(IEnumerable<IPublishedContent> entities, IUmbracoContextFactory contextFactory, string baseUrl)
        {
            Thread.Sleep(2000);
            lock (lockObject)
            {

                using (var umbracoContextReference = contextFactory.EnsureUmbracoContext())
                {

                    foreach (var entity in entities)
                    {
                        var oldModelShizzle = StandingData.OldModels.Where(x => x.UmbracoId == entity.Id);
                        if (oldModelShizzle != null && oldModelShizzle.Any())
                        {
                            foreach (var newItem in oldModelShizzle)
                            {
                                if (entity.Url != newItem.OldUrl)
                                {
                                    var newUrl = entity.Url;
                                    Create301(newItem.OldUrl, newUrl);
                                }
                            }

                            StandingData.OldModels.RemoveAll(x => x.UmbracoId == entity.Id);
                        }
                        // Todo: check if internalredirectID
                        if (entity == null || (entity.TemplateId == null || entity.TemplateId < 1) && !entity.Properties.Any(x => x.Alias == StandingData.UmbracoInternalRedirectIdName)) continue;
                        if (entity.Id == 0)
                        {
                            throw new ArgumentNullException("This should never happen");
                        }
                        else if (entity.Id > 0 && entity.IsPublished() == true)
                        {
                            var helper = umbracoContextReference.UmbracoContext.Content;
                            var node = helper.GetById(entity.Id);
                            if (node == null)
                                throw new ArgumentNullException("This has no node");

                            if (node.Children.Any())
                            {
                                DoUponSavedActions(node.Children, contextFactory, baseUrl);
                            }

                            // if it's a new page, it has no url yet. We have to move this to somewhere else, after saving
                            var niceUrl = baseUrl.TrimEnd('/') + node.Url;

                            var html = GetHtmlFromUrl(niceUrl);

                            CreateHtmlFile(node.Url, html);
                            Delete301(node.Url);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        public static void DeleteFilesAndFolders(string path, IUmbracoContextFactory contextFactory)
        {
            // is this always the last item or does it also use children?
            var trashedEntity = path.Split(',').Last();
            using (var umbracoContextReference = contextFactory.EnsureUmbracoContext())
            {
                var helper = umbracoContextReference.UmbracoContext.Content;
                var node = helper.GetById(Convert.ToInt32(trashedEntity));
                if (node != null)
                {
                    var folderLocation = GetFolderLocation(node.Url);
                    RecursiveDelete(new DirectoryInfo(folderLocation));
                }
            }
        }

        private static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }

        public static void Create301(string oldUrl, string url)
        {

            // Create a file so we can recognize it not to be indexed by crawlers etc
            CreateFile(oldUrl, StandingData.FileName301);
            var textFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\"  + StandingData.TemplatesFolder.RemoveAllSlashes() + "\\" + StandingData.HtmlFile301.RemoveAllSlashes();
            string text = File.ReadAllText(textFile);
            var htmlContents = text.Replace("{url}", url.Replace(StandingData.UmbracoRootFolderUrl.TrimEnd('/'), ""));
            CreateHtmlFile(oldUrl, htmlContents);
        }

        public static void Delete301(string folderName)
        {
            var folderLocation = GetFolderLocation(folderName);
            bool exists = Directory.Exists(folderLocation);
            if (exists)
                File.Delete(folderLocation.TrimEnd('\\') + "\\" + StandingData.FileName301.RemoveAllSlashes());
        }

        private static string GetFolderLocation(string url)
        {
            var replacedUrl = url.Replace(StandingData.UmbracoRootFolderUrl.RemoveAllSlashes(), "").TrimEnd('/');
            var folderLocation = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + StandingData.HtmlRootFolder.RemoveAllSlashes() + "\\" + replacedUrl.Replace("/", "\\");
            return folderLocation;
        }

        public static void CreateFile(string url, string fileName, string content = "")
        {
            var folderLocation = GetFolderLocation(url);
            bool exists = Directory.Exists(folderLocation);
            if (!exists)
                Directory.CreateDirectory(folderLocation);

            File.WriteAllText(folderLocation.TrimEnd('\\') + "\\" + fileName, content);
        }

        public static void CreateHtmlFile(string url, string html)
        {
            CreateFile(url, "index.html", html);
        }

        /// <summary>
        /// Gets the actual HTML from the given url
        /// </summary>
        /// <param name="pageUrl">The given URL</param>
        /// <returns>HTML</returns>
        public static string GetHtmlFromUrl(string pageUrl)
        {

            var originalHtml = "";
            var webRequestToGetFullHtml = System.Net.WebRequest.Create(pageUrl);
            var webRequestResponse = webRequestToGetFullHtml.GetResponse();
            using (var streamReader = new StreamReader(webRequestResponse.GetResponseStream()))
            {
                originalHtml = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var withFixedLinks = FixLinksInHtml(originalHtml);

            return withFixedLinks;
        }

        /// <summary>
        /// Replaces the links generated by Umbraco, removes the Root URL as set
        /// </summary>
        /// <param name="oldHtml"></param>
        /// <returns></returns>
        public static string FixLinksInHtml(string oldHtml)
        {
            var oldHtmlDocument = new HtmlDocument();
            oldHtmlDocument.LoadHtml(oldHtml);

            try
            {
                foreach (HtmlNode link in oldHtmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                {
                    // Just in case when a link is null (should not happen)
                    if (link == null) continue;

                    var linkAttribute = link.Attributes["href"];

                    // if there's no link or it has no value, move on
                    if (linkAttribute == null || string.IsNullOrEmpty(linkAttribute.Value)) continue;

                    linkAttribute.Value = linkAttribute.Value.Replace(StandingData.UmbracoRootFolderUrl.TrimEnd('/'), "");
                }

                return oldHtmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}