using Newtonsoft.Json;
using Sib.UmbracoStaticFileGenerator.Models;
using System;
using System.Collections.Generic;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public static class StandingData
    {
        public static List<UmbracoUrlModel> OldModels { get; set; } = new List<UmbracoUrlModel>();

        public static string UmbracoInternalRedirectIdName = "umbracoInternalRedirectId";

        public static string ApplicationName = "StaticFileGenerator";

        public static string ConfigFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\config\\Sib.StaticFileGenerator.js";

        /// <summary>
        /// The Folder where the HTML-files should be stored
        /// </summary>
        public static string HtmlRootFolder = "www";

        /// <summary>
        /// The URL Umbraco uses but we need to skip for creating our files and links
        /// </summary>
        public static string UmbracoRootFolderUrl = "website";

        /// <summary>
        /// The folder containing 301 templates etc
        /// </summary>
        public static string TemplatesFolder = "Templates";

        /// <summary>
        /// The 301-file in the TemplatesFolder
        /// </summary>
        public static string HtmlFile301 = "301.html";

        /// <summary>
        /// The 301-filename recognizing for sitemap etc
        /// </summary>
        public static string FileName301 = ".301";

        public static void SaveStandingData(ImportConfigModel configModel)
        {
            if (!string.IsNullOrEmpty(configModel.FileName301)) StandingData.FileName301 = configModel.FileName301;
            if (!string.IsNullOrEmpty(configModel.HtmlFile301)) StandingData.HtmlFile301 = configModel.HtmlFile301;
            if (!string.IsNullOrEmpty(configModel.HtmlRootFolder)) StandingData.HtmlRootFolder = configModel.HtmlRootFolder;
            if (!string.IsNullOrEmpty(configModel.TemplatesFolder)) StandingData.TemplatesFolder = configModel.TemplatesFolder;
            if (!string.IsNullOrEmpty(configModel.UmbracoRootFolderUrl)) StandingData.UmbracoRootFolderUrl = configModel.UmbracoRootFolderUrl;

            // todo: save to config
            ContentUpdater.CreateFile(StandingData.ConfigFile, JsonConvert.SerializeObject(configModel));
        }
    }
}