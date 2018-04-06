using CoreProfiler;
using CoreProfiler.Timings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Modules;
using MyAPI.Repositorys;
using System;
using System.Collections.Generic;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ICoreProfilerRepository _coreProfilerRepository;
        public ValuesController(ILogger<ValuesController> logger, IEventLogRepository eventLogRepository, ICoreProfilerRepository coreProfilerRepository)
        {
            _logger = logger;
            _eventLogRepository = eventLogRepository;
            _coreProfilerRepository = coreProfilerRepository;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            ITimingSession timingSession = null;
            using (ProfilingSession.Current.Step(() => "Handle Request - /"))
            {
                timingSession = ProfilingSession.Current.Profiler.GetTimingSession();
                _coreProfilerRepository.InsertAsync(new CoreProfilerModulecs
                {
                    SessionId = timingSession.ParentId.HasValue ? timingSession.ParentId.Value.ToString() :
                    Guid.NewGuid().ToString(),
                    Machine = timingSession.MachineName,
                    Type = timingSession.Type,
                    Id = timingSession.Id.ToString(),
                    Name = timingSession.Name,
                    Start = timingSession.StartMilliseconds,
                    Duration = timingSession.DurationMilliseconds,
                    Sort = timingSession.Sort,
                    Started = timingSession.Started
                }).GetAwaiter().GetResult();
                using (ProfilingSession.Current.Step(() => "write Log"))
                {
                    _logger.LogInformation("calling Values controller action");
                    //_logger.LogError("calling Values controller action");
                    timingSession = ProfilingSession.Current.Profiler.GetTimingSession();
                    _coreProfilerRepository.InsertAsync(new CoreProfilerModulecs
                    {
                        SessionId = timingSession.ParentId.HasValue ? timingSession.ParentId.Value.ToString() :
                  Guid.NewGuid().ToString(),
                        Machine = timingSession.MachineName,
                        Type = timingSession.Type,
                        Id = timingSession.Id.ToString(),
                        Name = timingSession.Name,
                        Start = timingSession.StartMilliseconds,
                        Duration = timingSession.DurationMilliseconds,
                        Sort = timingSession.Sort,
                        Started = timingSession.Started
                    }).GetAwaiter().GetResult();
                }
                using (ProfilingSession.Current.Step(() => "write Data to SQL Server"))
                {
                    _eventLogRepository.InsertAsync(new EventLogModule
                    {
                        EventID = 1,
                        Exception = "",
                        LogLevel = "Information",
                        Message = "test by Rico"
                    });
                    timingSession = ProfilingSession.Current.Profiler.GetTimingSession();
                    _coreProfilerRepository.InsertAsync(new CoreProfilerModulecs
                    {
                        SessionId = timingSession.ParentId.HasValue ? timingSession.ParentId.Value.ToString() :
                   Guid.NewGuid().ToString(),
                        Machine = timingSession.MachineName,
                        Type = timingSession.Type,
                        Id = timingSession.Id.ToString(),
                        Name = timingSession.Name,
                        Start = timingSession.StartMilliseconds,
                        Duration = timingSession.DurationMilliseconds,
                        Sort = timingSession.Sort,
                        Started = timingSession.Started
                    }).GetAwaiter().GetResult();
                }
                using (ProfilingSession.Current.Step(() => "return result"))
                {
                    timingSession = ProfilingSession.Current.Profiler.GetTimingSession();
                    _coreProfilerRepository.InsertAsync(new CoreProfilerModulecs
                    {
                        SessionId = timingSession.ParentId.HasValue ? timingSession.ParentId.Value.ToString() :
                   Guid.NewGuid().ToString(),
                        Machine = timingSession.MachineName,
                        Type = timingSession.Type,
                        Id = timingSession.Id.ToString(),
                        Name = timingSession.Name,
                        Start = timingSession.StartMilliseconds,
                        Duration = timingSession.DurationMilliseconds,
                        Sort = timingSession.Sort,
                        Started = timingSession.Started
                    }).GetAwaiter().GetResult();
                    return new string[] { "value1", "value2" };
                }
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
