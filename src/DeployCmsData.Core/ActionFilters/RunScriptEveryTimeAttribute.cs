﻿using System;

namespace DeployCmsData.Core.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RunScriptEveryTimeAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
    }
}
