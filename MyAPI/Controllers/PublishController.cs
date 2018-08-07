using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyAPI.Modules;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PublishController : Controller
    {
        private readonly ICapPublisher _publisher;
        public PublishController(ICapPublisher publisher)
        {
            _publisher = publisher;
        }

        [Route("~/checkCustomerWithTrans")]
        public async Task<IActionResult> PublishMessageWithTransactionUsingAdonet([FromServices] IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LoggerDatabase");
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    // your business code
                    //topic=rico.services.customer.check
                    _publisher.Publish("rico.services.customer.check",
                        new CustomersModule { ID = "1", Name = "ricoisme", Email = "rico@aa.com", MobilePhone = "123456789" }, sqlTransaction);

                    sqlTransaction.Commit();
                }
            }
            return Ok();
        }

        [CapSubscribe("rico.services.customer.check")]
        public Task<string> CheckReceivedMessage(CustomersModule customer)
        {
            return Task.FromResult(string.Join(",", customer));
        }
    }
}