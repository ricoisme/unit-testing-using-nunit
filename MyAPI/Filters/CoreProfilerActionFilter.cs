using CoreProfiler;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyAPI.Modules;
using MyAPI.Repositorys;
using System.Collections.Generic;

namespace MyAPI.Filters
{
    public class CoreProfilerActionFilter : ActionFilterAttribute
    {
        private readonly ICoreProfilerRepository _coreProfilerRepository;
        private readonly ILogger<CoreProfilerResultFilter> _logger;
        public CoreProfilerActionFilter(ICoreProfilerRepository coreProfilerRepository, ILogger<CoreProfilerResultFilter> logger)
        {
            _coreProfilerRepository = coreProfilerRepository;
            _logger = logger;
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var timingSession = ProfilingSession.Current.Profiler.GetTimingSession();
            if (timingSession != null)
            {
                string sessionId = timingSession.Id.ToString();
                List<CoreProfilerModulecs> coreProfilerModulecs = new List<CoreProfilerModulecs>();
                foreach (var timing in timingSession.Timings)
                {
                    coreProfilerModulecs.Add(new CoreProfilerModulecs
                    {
                        SessionId = sessionId,
                        ParentId = timing.ParentId.HasValue ? timing.ParentId.Value.ToString() : "",
                        Machine = timingSession.MachineName,
                        Type = timing.Type,
                        CurrentId = timing.Id.ToString(),
                        Name = timing.Name,
                        Start = timing.StartMilliseconds,
                        Duration = timing.DurationMilliseconds,
                        Sort = timing.Sort,
                        Started = timing.Started
                    });
                }
                _coreProfilerRepository.BulkInsert(coreProfilerModulecs);
            }
            base.OnResultExecuted(context);
        }
    }
}
