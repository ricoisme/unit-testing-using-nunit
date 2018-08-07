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

                    //sqlTransaction.Commit();
                    sqlTransaction.Rollback();
                }
            }
            return Ok();
        }

        private static void FailedMessagesProcess()
        {
            //if (returnFailedMessage == null)
            //    return;
            //try
            //{
            //    var warningMessage = $"Message string is too long. Topic:{returnFailedMessage.Topic}, Timestamp:{returnFailedMessage.Timestamp}, Length:{returnFailedMessage.Length}, Limit:{returnFailedMessage.Limit}, Published:{returnFailedMessage.Published}";
            //    Console.WriteLine(warningMessage);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(($"Error occurred when LargeMessagesProcess executed:{e.Message}"));
            //}
        }

        [CapSubscribe("rico.services.customer.check")]
        public Task CheckReceivedMessage(CustomersModule customer)
        {
            //return Task.FromException(new NullReferenceException());
            var tcs = new TaskCompletionSource<int>();
            tcs.TrySetCanceled();
            return tcs.Task;
            //return Task.FromResult(string.Join(",", customer));
        }
    }
}