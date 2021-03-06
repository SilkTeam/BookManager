﻿using System;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Filter
{
    public class IdentityAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Convert.ToInt32(httpContext.Session["Identity"]) != 0;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ContentResult() { Content = "权限不足" };
        }
    }
}