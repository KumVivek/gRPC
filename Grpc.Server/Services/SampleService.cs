using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Message;
using Microsoft.Extensions.Logging;

namespace Grpc.Server
{
    public class SampleService : Sample.SampleBase
    {
        private readonly ILogger<SampleService> _logger;
        public SampleService(ILogger<SampleService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
