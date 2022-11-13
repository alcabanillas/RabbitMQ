using System;
using System.Collections.Generic;
using System.Configuration;

namespace Common
{
    public class Config : IConfig
    {
        private static Config _instance;
        private static readonly object _lock = new object();

        public string hostname { get; private set; }
        public string queuename { get ; private set; }
        public bool durable { get; private set; }
        public bool autoDelete { get; private set; }
        public bool exclusive { get; private set; }
        public IDictionary<string, object> arguments { get; private set; }
        public string connString { get; private set; }

        public static Config Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Config();
                    }
                    return _instance;
                }
            }
        }

        protected Config()
        {
            hostname = ConfigurationManager.AppSettings["hostname"];
            queuename = ConfigurationManager.AppSettings["queuename"];
            durable = bool.Parse(ConfigurationManager.AppSettings["durable"]);
            durable = bool.Parse(ConfigurationManager.AppSettings["autoDelete"]);
            durable = bool.Parse(ConfigurationManager.AppSettings["exclusive"]);
            connString = ConfigurationManager.AppSettings["connString"];
    }
    }
}
