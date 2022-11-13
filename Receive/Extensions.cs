using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Consumer
{
    public static class Extensions
    {
        public static IDbConnection Open<T>(this IDbConnectionFactory factory, string cnDB)
        {
            var connection = factory.Get(cnDB);
            connection.Open();
            return connection;
        }
    }
}
