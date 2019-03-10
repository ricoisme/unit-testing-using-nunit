using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using MyAPI.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAPI.Filters
{
    public class CacheResultFilter : ResultFilterAttribute
    {
        private readonly int _expiration;
        public CacheResultFilter(int expirationMs)
        {
            _expiration = expirationMs;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.RouteData.Values["Controller"].ToString();
            var action = context.RouteData.Values["Action"].ToString();
            var method = context.HttpContext.Request.Method;
            var queryStrings = new List<string>();
            if (context.HttpContext.Request.Query.Count > 0)
            {
                context.HttpContext.Request.Query.ToList()
                   .ForEach(kv =>
                   {
                       queryStrings.Add(kv.Key);
                       queryStrings.Add(kv.Value);
                   });
            }
            var key = controller.GenerateCacheKey(action, method, queryStrings.ToArray());
            var cache = context.HttpContext.RequestServices.GetService(typeof(IDistributedCache));
            if (cache == null)
            {
                base.OnResultExecuting(context);
                return;
            }
            var cacheValue = (cache as IDistributedCache)?.GetString(key);
            if (cacheValue != null)
            {
                context.Result =
                    new ObjectResult(JsonConvert.DeserializeObject(cacheValue));
            }
            else
            {
                var objResult = context.Result as ObjectResult;
                var json = JsonConvert.SerializeObject(objResult.Value);
                //var o = JObject.Parse(json);
                //var obj = o.SelectToken("Value").Select(s => (string)s);
                //var jArray = JArray.Parse(json);
                //var obj = jArray[0]["Value"].Value<string>();             
                (cache as IDistributedCache)?.SetString(key, json,
               new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(_expiration) });
            }
            base.OnResultExecuting(context);
        }

        //public async override Task OnResultExecutionAsync(
        //    ResultExecutingContext context,
        //    ResultExecutionDelegate next)
        //{
        //    var controller = context.RouteData.Values["Controller"].ToString();
        //    var action = context.RouteData.Values["Action"].ToString();
        //    var method = context.HttpContext.Request.Method;
        //    var key = controller.GenerateCacheKey(action, method);
        //    var cache = context.HttpContext.RequestServices.GetService(typeof(IDistributedCache));
        //    if (cache == null)
        //    {
        //        await base.OnResultExecutionAsync(context, next).ConfigureAwait(false);
        //        return;
        //    }
        //    var cacheValue = await (cache as IDistributedCache).GetStringAsync(key).ConfigureAwait(false);
        //    if (cacheValue != null)
        //    {
        //        context.Result =
        //          new ObjectResult(JsonConvert.DeserializeObject(cacheValue));
        //    }
        //    else
        //    {
        //        await next().ConfigureAwait(false);
        //        var objResult = context.Result as ObjectResult;
        //        var json = JsonConvert.SerializeObject(objResult.Value);
        //        await (cache as IDistributedCache).SetStringAsync(key, json,
        //            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(_expiration) }).ConfigureAwait(false);
        //    }
        //    await base.OnResultExecutionAsync(context, next).ConfigureAwait(false);
        //}


    }
}
