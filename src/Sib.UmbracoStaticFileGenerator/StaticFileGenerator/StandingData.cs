using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public static class StandingData
    {
        /// <summary>
        /// Used after saving, to check for 301-changes
        /// </summary>
        public static List<UmbracoUrlModel> OldModels { get; set; } = new List<UmbracoUrlModel>();

        /// <summary>
        /// Don't want to make typo's
        /// </summary>
        public static string UmbracoInternalRedirectIdName = "umbracoInternalRedirectId";

        /// <summary>
        /// The name of this plugin
        /// </summary>
        public static string ApplicationName = "StaticFileGenerator";

        /// <summary>
        /// The location of the configfile
        /// </summary>
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

        /// <summary>
        /// Update the standing data and save to file
        /// </summary>
        /// <param name="configModel"></param>
        public static void SaveStandingData(ImportConfigModel configModel)
        {
            if (!string.IsNullOrEmpty(configModel.FileName301)) StandingData.FileName301 = configModel.FileName301;
            if (!string.IsNullOrEmpty(configModel.HtmlFile301)) StandingData.HtmlFile301 = configModel.HtmlFile301;
            if (!string.IsNullOrEmpty(configModel.HtmlRootFolder)) StandingData.HtmlRootFolder = configModel.HtmlRootFolder;
            if (!string.IsNullOrEmpty(configModel.TemplatesFolder)) StandingData.TemplatesFolder = configModel.TemplatesFolder;
            if (!string.IsNullOrEmpty(configModel.UmbracoRootFolderUrl)) StandingData.UmbracoRootFolderUrl = configModel.UmbracoRootFolderUrl;

            ContentUpdater.CreateFile(StandingData.ConfigFile, JsonConvert.SerializeObject(configModel, Formatting.Indented));
        }
    }
}