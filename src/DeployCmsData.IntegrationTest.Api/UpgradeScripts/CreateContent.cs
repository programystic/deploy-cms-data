﻿using DeployCmsData.Core.ActionFilters;
using DeployCmsData.Core.Interfaces;
using DeployCmsData.UmbracoCms.Services;

namespace DeployCmsData.IntegrationTest.Api.UpgradeScripts
{
    [RunScriptEveryTime]
    public class CreateContent : UmbracoUpgradeScript
    {
        public override bool RunScript(IUpgradeLogRepository upgradeLog)
        {
            var website = ContentService.CreateContent("My Website", -1, "websiteRoot");
            ContentService.SaveAndPublishWithStatus(website);

            var homePage = ContentService.CreateContent("Home", website.Id, "homePage");
            homePage.SetValue("pageTitle", "Hello World");
            ContentService.SaveAndPublishWithStatus(homePage);

            return true;
        }
    }
}