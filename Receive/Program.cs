using Common;
using Consumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data.Common;
using System.Text;

class Program
{
    public static void Main()
    {
        IConfig cfg = Config.Instance;

        IDbConnectionFactory db = new SqlDbConnectionFactory();

        BaseConsumer client = new RabbitMQConsumer(cfg, db);

        client.ListenEvents();


        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}