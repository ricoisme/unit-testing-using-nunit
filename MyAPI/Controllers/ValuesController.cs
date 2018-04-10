using CoreProfiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Modules;
using MyAPI.Repositorys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAPI.Controllers
{
    //[CoreProfilerResultFilterAttribute]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IEventLogRepository _eventLogRepository;

        public ValuesController(ILogger<ValuesController> logger, IEventLogRepository eventLogRepository)
        {
            _logger = logger;
            _eventLogRepository = eventLogRepository;

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
