using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Runtime.Caching;

namespace Viq.AccessPoint.TestHarness.Services.Helpers
{
    public class CacheHandlerFilter: ActionFilterAttribute
    {  
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            try
            { 
                base.OnActionExecuting(ctx);
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.SlidingExpiration = DateTime.Now.AddDays(1).TimeOfDay;
                var appSettings = ctx.ActionArguments["setup"] as AppSettingsDto; 

                ObjectCache cache = MemoryCache.Default;
                cache.Set(CacheType.AppSettings.ToString(), appSettings, policy);
            }
            catch (Exception)
            { 
                throw;
            } 
        }
    }
}
