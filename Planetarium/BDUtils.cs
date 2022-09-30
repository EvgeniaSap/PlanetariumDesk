using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Planetarium
{
    class BDUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            string database = "planetarium";
            string username = "root";
            string password = "";

            return BDmySQL.GetDBConnection(host, database, username, password);
        }
    }
}
