using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Farmacia_Imperial
{
    public class dbMethods
    {
        Form1 form = new Form1();
        public void insertData(string descripcionMedicamento, string nombreDist1, string precioDist1, string nombreDist2, 
            string precioDist2, string precioV, string inventarioInicial, string inventarioFinal)
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            string nombreD1, nombreD2, precioD1, precioD2, precioVenta, invInicial, invFinal;

            if ((string.IsNullOrWhiteSpace(nombreDist1)) || string.IsNullOrEmpty(nombreDist1))
                nombreD1 = "";
            else
                nombreD1 = nombreDist1;

            if ((string.IsNullOrWhiteSpace(nombreDist2)) || string.IsNullOrEmpty(nombreDist2))
                nombreD2 = "";
            else
                nombreD2 = nombreDist2;

            if ((string.IsNullOrWhiteSpace(precioDist1)) || string.IsNullOrEmpty(precioDist1))
                precioD1 = "0";
            else
                precioD1 = precioDist1;

            if ((string.IsNullOrWhiteSpace(precioDist2)) || string.IsNullOrEmpty(precioDist2))
                precioD2 = "0";
            else
                precioD2 = precioDist2;

            if ((string.IsNullOrWhiteSpace(precioV)) || string.IsNullOrEmpty(precioV))
                precioVenta = "0";
            else
                precioVenta = precioV;

            if ((string.IsNullOrWhiteSpace(inventarioInicial)) || string.IsNullOrEmpty(inventarioInicial))
                invInicial = "0";
            else
                invInicial = inventarioInicial;

            if ((string.IsNullOrWhiteSpace(inventarioFinal)) || string.IsNullOrEmpty(inventarioFinal))
                invFinal = "0";
            else
                invFinal = inventarioFinal;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAgregarMedicamento";
            cmd.Parameters.Add("@descripcion_medicamento", SqlDbType.VarChar).Value = descripcionMedicamento;
            cmd.Parameters.Add("@nombre_d1", SqlDbType.VarChar).Value = nombreD1;
            cmd.Parameters.Add("@precio_d1", SqlDbType.Decimal).Value = precioD1;
            cmd.Parameters.Add("@nombre_d2", SqlDbType.VarChar).Value = nombreD2;
            cmd.Parameters.Add("@precio_d2", SqlDbType.Decimal).Value = precioD2;
            cmd.Parameters.Add("@precio_v", SqlDbType.Decimal).Value = precioVenta;
            cmd.Parameters.Add("@inv_inicial", SqlDbType.Int).Value = invInicial;
            cmd.Parameters.Add("@inv_final", SqlDbType.Int).Value = invFinal;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            conexion.Close();
        }
    }
}
