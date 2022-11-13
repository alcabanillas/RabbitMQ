using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IConfig cfg = Config.Instance;

            BaseProducer server = new RabbitMQProducer(cfg);
            
            var cts = new CancellationTokenSource();
            var task = server.DoWork(cts.Token);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadKey(intercept : true);

            cts.Cancel();
            // We wait for the task to gracefully quit
            await task;
        }

    }
}
