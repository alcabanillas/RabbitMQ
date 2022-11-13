using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    abstract class BaseConsumer
    {
        const int MAX_TIMESTAMP = 60;
        const string INSERT_CMD = "INSERT INTO MESSAGES(msg) VALUES({0})";

        internal readonly IConfig _cfg;
        internal readonly IDbConnectionFactory _connectionFactory;

        public BaseConsumer(IConfig cfg, IDbConnectionFactory dbConnectionFactory)
        {
            _cfg = cfg;
            _connectionFactory = dbConnectionFactory;
        }

        internal abstract void ListenEvents();

        internal bool MustBeWrittenToDB(string message)
        {
            DateTime msgDate = DateTime.Parse(message);
            return msgDate.Second % 2 == 0;
        }

        /// <summary>
        /// Processes the received message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>true if the message must be sended again, false otherwise</returns>
        internal async Task<bool> ProcessMessage(string message)
        {
            Console.WriteLine(" [x] Received {0}", message);

            DateTime msgDate = DateTime.Parse(message);
            DateTime currentDate = DateTime.Now;

            TimeSpan gap = currentDate - msgDate;
            if (gap.TotalSeconds > MAX_TIMESTAMP)
            {
                Console.WriteLine(" Message older than 1 minute");
                return false;
            }

            if (MustBeWrittenToDB(message))
            {
                try
                {
                    await InsertToDB(message);
                    Console.WriteLine(" Message even");
                    return false;
                }
                catch (Exception ex)
                {
                    //Log error message
                    return false;
                }
            }

            return true;
        }

        internal Task InsertToDB(string message)
        {
            Task t = new Task(() =>
            {
                IDbConnection conn = _connectionFactory.Open<IDbConnection>(_cfg.connString);
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = String.Format(INSERT_CMD, message);
                cmd.ExecuteNonQuery();
            });
            return t;
        }


    }
}
