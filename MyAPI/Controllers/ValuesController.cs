using CoreProfiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Modules;
using MyAPI.Repositorys;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<string>> Get()
        {
            ProfilingSession.Current.AddTag("Get");
            using (ProfilingSession.Current.Step(() => "Handle Request - /"))
            {
                using (ProfilingSession.Current.Step(() => "write Log"))
                {
                    _logger.LogInformation("calling Values controller action");
                    //_logger.LogError("calling Values controller action");
                }
                using (ProfilingSession.Current.Step(() => "write Data to SQL Server"))
                {
                    await _eventLogRepository.InsertAsync(new EventLogModule
                    {
                        EventID = 1,
                        Exception = "",
                        LogLevel = "Information",
                        Message = "test by Rico"
                    });
                }
                using (ProfilingSession.Current.Step("Render Result"))
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

                        await _coreProfilerRepository.BulkInsertAsync(coreProfilerModulecs);
                    }

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
