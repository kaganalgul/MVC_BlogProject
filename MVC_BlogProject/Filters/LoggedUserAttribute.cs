﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_BlogProject.Filters
{
    public class LoggedUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userId = context.HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                string routePath = context.HttpContext.Request.Path;
                context.Result = new RedirectToActionResult("Login", "Auth", new { yonlen = routePath });
            }
        }
    }
}
