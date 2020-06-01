using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Farmacia_Imperial
{
    class Connection
    {
        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.Conectar);
            conn.Open();

            return conn;
        }
    }
}
