using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VATMVCAPPFramework.Data.ConnectionFactory
{
    public static class DBConnection
    {
        public static Func<DbConnection> ConnectionFactory = () => new SqlConnection(ConnectionString.Connection);

        public static class ConnectionString
        {
            public static string Connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

    }
}
