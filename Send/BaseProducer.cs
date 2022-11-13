using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    abstract class BaseProducer
    {
        internal readonly IConfig _cfg;

        internal abstract Task Publish();
        
        public BaseProducer(IConfig cfg)
        {
            _cfg = cfg;
        }

        public async Task DoWork(CancellationToken cancel)
        {
            foreach (var task in ProduceForever(cancel))
            {
                await task;
            }
        }

        internal IEnumerable<Task> ProduceForever(CancellationToken cancel)
        {
            do
            {
                var t = Task.Run(async delegate
                {
                    await Task.Delay(3000);
                });
                t.Wait();

                yield return Publish();

            } while (!cancel.IsCancellationRequested);

        }
    }
}
