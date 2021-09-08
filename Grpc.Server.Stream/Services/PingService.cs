using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServerStreamExample;

namespace Grpc.Server.Stream
{
    public class PingService : ServerStreamExample.Ping.PingBase
    {
        private readonly ILogger<PingService> _logger;
        public PingService(ILogger<PingService> logger)
        {
            _logger = logger;
        }

        public async override Task DoReply(Message request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"Initial Message from Client: {request.Msg}");

            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    await responseStream.WriteAsync(new Message
                    {
                        Msg = $"Ping Response from the Server at {DateTime.UtcNow}"
                    });
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Operation Cancelled.");
            }

            Console.WriteLine("Processing Complete.");
        }
    }
}
