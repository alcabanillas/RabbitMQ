using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Consumer
{
    public interface IDbConnectionFactory
    {
        IDbConnection Get(string connectionString);
    }

    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Get(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}

