using System.Collections.Generic;

namespace Common
{
    public interface IConfig
    {
        public string hostname { get;}
        public string queuename { get; }
        public bool durable { get; }
        public bool autoDelete { get; }
        public bool exclusive { get; }
        public IDictionary<string, object> arguments { get; }
        public string connString { get; }
    }
}