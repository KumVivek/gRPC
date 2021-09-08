using Grpc.Net.Client;
using System;

namespace gRPC.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started Calling......");
            // Enable support for unencrypted HTTP2  
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Math.ArithmeticServices.ArithmeticService.ArithmeticServiceClient(channel);

            var response = client.PerformAddOperation(new Math.ArithmeticServices.InputRequest() 
            {
                A = 10,
                B = 10
            });
            Console.WriteLine(response.Output);
            Console.ReadKey();
        }
    }
}
