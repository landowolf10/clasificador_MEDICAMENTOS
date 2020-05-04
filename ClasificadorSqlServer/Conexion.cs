using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ClasificadorSqlServer
{
    class Conexion
    {
        public static SqlConnection obtenerConexion()
        {
            SqlConnection Conn = new SqlConnection(Properties.Settings.Default.Conectar);
            Conn.Open();

            return Conn;
        }
    }
}
