﻿using CoreProfiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyAPI.Modules;
using MyAPI.Repositorys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAPI.Filters
{
    public class CoreProfilerResultFilterAttribute : TypeFilterAttribute
    {
        public CoreProfilerResultFilterAttribute() : base(typeof(CoreProfilerResultFilterImpl))
        {
        }

        private class CoreProfilerResultFilterImpl : IAsyncResultFilter
        {
            private readonly ILogger<CoreProfilerResultFilterImpl> _logger;
            private readonly ICoreProfilerRepository _coreProfilerRepository;
            public CoreProfilerResultFilterImpl(ILogger<CoreProfilerResultFilterImpl> logger, ICoreProfilerRepository coreProfilerRepository)
            {
                _logger = logger;
                _coreProfilerRepository = coreProfilerRepository;
            }

            public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            {
                // do something before the action executes
                await next();
                //after the action executes
                var timingSession = ProfilingSession.Current.Profiler.GetTimingSession();
                if (timingSession != null)
                {
                    string sessionId = timingSession.Id.ToString();
                    List<CoreProfilerModulecs> coreProfilerModulecs = new List<CoreProfilerModulecs>();
                    foreach (var timing in timingSession.Timings)
                    {
                        long duration = 0;
                        if (timing.Name.ToLowerInvariant() == "root" && timing.DurationMilliseconds <= 0)
                            duration = timingSession.DurationMilliseconds;
                        coreProfilerModulecs.Add(new CoreProfilerModulecs
                        {
                            SessionId = sessionId,
                            ParentId = timing.ParentId.HasValue ? timing.ParentId.Value.ToString() : "",
                            Machine = timingSession.MachineName,
                            Type = timing.Type,
                            CurrentId = timing.Id.ToString(),
                            Name = timing.Name,
                            Start = timing.StartMilliseconds,
                            Duration = duration > 0 ? duration : timing.DurationMilliseconds,
                            Sort = timing.Sort,
                            Started = timing.Started
                        });
                    }
                    await _coreProfilerRepository.BulkInsertAsync(coreProfilerModulecs);
                }
            }
        }
    }
}
