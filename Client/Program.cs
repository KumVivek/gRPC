using Grpc.Core;
using Grpc.Net.Client;
using ServerStreamExample;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            Console.WriteLine("Started Calling......");
            // Enable support for unencrypted HTTP2  
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            // call the PingService
            CallPingReply(new Ping.PingClient(channel)).GetAwaiter().GetResult();
           
        }

        private async static Task CallPingReply(Ping.PingClient pingClient)
        {
            // some random request from client
            var request = new Message { Msg = "Hello Ping!" };

            // a cancellationToken to cancel the operation after sometime
            // in this case the operation (reading from the server) is cancelled
            // after 60 seconds from the invocation
            var cancellationToken = new CancellationTokenSource(
              TimeSpan.FromSeconds(60));

            try
            {
                // call the server method which returns a stream 
                // pass the cancellationToken along side so that the operation gets
                // cancelled when needed
                AsyncServerStreamingCall<Message> response =
                    pingClient.DoReply(request,
                      cancellationToken: cancellationToken.Token);

                // loop through each object from the ResponseStream
                while (await response.ResponseStream.MoveNext())
                {
                    // fetch the object currently pointed
                    var current = response.ResponseStream.Current;

                    // print it
                    Console.WriteLine($"{current.Msg}");
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Operation Cancelled.");
            }

            Console.ReadLine();
        }
    }
}
