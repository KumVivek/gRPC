using Grpc.Core;
using Math.ArithmeticServices;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Grpc.Server
{
    public class SampleService : Math.ArithmeticServices.ArithmeticService.ArithmeticServiceBase
    {
        private readonly ILogger<SampleService> _logger;
        public SampleService(ILogger<SampleService> logger)
        {
            _logger = logger;
        }

        public async override Task<InputResponse> PerformAddOperation(InputRequest request, ServerCallContext context)
        {
           return new InputResponse()
            {
                Output = request.A + request.B
            };
        }
    }
}
