using CoreProfiler;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Extension;
using MyAPI.Modules;
using MyAPI.Repositorys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        [HttpGet("ReadName/{serial}")]
        public async Task<string> ReadName(int serial)
        {
            var inputpara = new DynamicParameters();
            inputpara.Add("c1", serial, DbType.Int16, ParameterDirection.Input, null);

            var outputpara = new DynamicParameters();
            outputpara.Add("c2", "", DbType.String, ParameterDirection.Output, 10);

            await _eventLogRepository.ExecuteSPAsync("usp_test", inputpara, outputpara);
            string result = outputpara.Get<string>("c2");

            return string.IsNullOrEmpty(result) ? "no content" : result;
        }

        [HttpGet("BulkTest/{max}")]
        public async Task<string> BulkTest(int max)
        {
            var datas = new List<EventLogModule>();
            var dt = new DataTable();
            ProfilingSession.Current.AddTag("BulkTest");
            using (ProfilingSession.Current.Step(() => "Handle Request - /"))
            {
                using (ProfilingSession.Current.Step(() => "generate datas"))
                {
                    datas = generate(max);
                }
                using (ProfilingSession.Current.Step(() => "Convert datatable"))
                {
                    dt = datas.ToDataTable(new string[] { "EventID", "LogLevel", "Message", "Exception" });
                }
                using (ProfilingSession.Current.Step(() => "bulk insert to sql server"))
                {
                    //await _eventLogRepository.BulkInsertAsync(datas);
                    var mytvp = new DynamicParameters();
                    mytvp.AddDynamicParams(new { DataList = dt.AsTableValuedParameter("UT_EventLog") });
                    await _eventLogRepository.ExecuteSPAsync("dbo.usp_InsertEventLogWithTvp", mytvp);
                }
                return await Task.FromResult($"number of record:{datas.Count}");
            }
        }

        private List<EventLogModule> generate(int max)
        {
            var result = new List<EventLogModule>();
            var objlock = new object();
            Parallel.For(0, max, (index) =>
                 {
                     lock (objlock)
                     {
                         result.Add(new EventLogModule
                         {
                             EventID = index,
                             Exception = "",
                             LogLevel = "Information",
                             Message = $"bulk insert test => {index}"
                         });
                     }
                 });
            return result;
        }


        private SqlMapper.ICustomQueryParameter ToTvp<T>(
   IEnumerable<T> enumerable,
   params Func<T, object>[] columnSelectors)
        {
            if (columnSelectors.Length == 0)
            {
                Func<T, object> getSelf = x => x;
                columnSelectors = new[] { getSelf };
            }
            var result = new DataTable();
            foreach (var selector in columnSelectors)
            {
                result.Columns.Add();
            }
            foreach (var item in enumerable)
            {
                var colValues = columnSelectors.Select(selector => selector(item)).ToArray();
                result.Rows.Add(colValues);
            }
            return result.AsTableValuedParameter();
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
