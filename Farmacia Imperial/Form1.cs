using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Farmacia_Imperial
{
    public partial class Form1 : Form
    {
        string mensaje = "";
        int idMedicamento = 0;
        string descripcionMedicamento = "", nombreDist1 = "", precioDist1 = "", nombreDist2 = "", precioDist2 = "", precioV = "";
        string inventarioInicial = "", inventarioFinal = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                MessageBox.Show("Favor de capturar la descripción del medicamento.");
            else
            {
                if (txtPrecioD1.Text == "." || txtPrecioD1.Text == "." || txtPrecioVenta.Text == ".")
                    MessageBox.Show("Favor de ingresar un precio correcto");
                else
                {
                    if (btnGuardar.Text == "Actualizar")
                    {
                        mensaje = "actualizado";
                        updateData();
                    }
                    else
                    {
                        descripcionMedicamento = txtDescripcion.Text;
                        nombreDist1 = txtNombreD1.Text;
                        precioDist1 = txtPrecioD1.Text;
                        nombreDist2 = txtNombreD2.Text;
                        precioDist2 = txtPrecioD2.Text;
                        precioV = txtPrecioVenta.Text;
                        inventarioInicial = txtInvInicial.Text;
                        inventarioFinal = txtInvFinal.Text;

                        dbMethods metodosDB = new dbMethods();

                        mensaje = "insertado";

                        metodosDB.insertData(descripcionMedicamento, nombreDist1, precioDist1, nombreDist2, precioDist2, precioV, inventarioInicial, inventarioFinal);

                        Clear();
                        gridFill();
                    }

                    MessageBox.Show("Medicamento " + mensaje + " correctamente");
                }
            }
        }

        void updateData()
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spModificarMedicamento";
            cmd.Parameters.Add("@id_medicamento", SqlDbType.Int).Value = idMedicamento;
            cmd.Parameters.Add("@descripcion_medicamento", SqlDbType.VarChar).Value = txtDescripcion.Text;
            cmd.Parameters.Add("@nombre_d1", SqlDbType.VarChar).Value = txtNombreD1.Text;
            cmd.Parameters.Add("@precio_d1", SqlDbType.Decimal).Value = txtPrecioD1.Text;
            cmd.Parameters.Add("@nombre_d2", SqlDbType.VarChar).Value = txtNombreD2.Text;
            cmd.Parameters.Add("@precio_d2", SqlDbType.Decimal).Value = txtPrecioD2.Text;
            cmd.Parameters.Add("@precio_v", SqlDbType.Decimal).Value = txtPrecioVenta.Text;
            cmd.Parameters.Add("@inv_inicial", SqlDbType.Int).Value = txtInvInicial.Text;
            cmd.Parameters.Add("@inv_final", SqlDbType.Int).Value = txtInvFinal.Text;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            Clear();
            gridFill();

            conexion.Close();
        }

        void deleteData()
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spEliminarMedicamento";
            cmd.Parameters.Add("@id_medicamento", SqlDbType.Int).Value = idMedicamento;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            MessageBox.Show("Medicamento eliminado correctamente");

            Clear();
            gridFill();

            conexion.Close();
        }

        void search()
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spBuscarMedicamento";
            cmd.Parameters.Add("@descripcion_medicamento", SqlDbType.VarChar).Value = txtBuscar.Text;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dataTable);
                dgvMedicamentos.DataSource = dataTable;
                dgvMedicamentos.Columns[0].Visible = false;
            }

            txtBuscar.Text = "";
        }

        void gridFill()
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            string query = cmd.CommandText = "spMostrarTodo";

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dgvMedicamentos.DataSource = dataTable;

            dgvMedicamentos.Columns[0].Visible = false;
        }

        void Clear()
        {
            txtDescripcion.Text = "";
            txtNombreD1.Text = "";
            txtPrecioD1.Text = "";
            txtNombreD2.Text = "";
            txtPrecioD2.Text = "";
            txtPrecioVenta.Text = "";
            txtInvInicial.Text = "";
            txtInvFinal.Text = "";
            btnGuardar.Text = "Guardar";
            btnEliminar.Enabled = false;
        }

        private void dgvMedicamentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMedicamentos.CurrentRow.Index != -1)
            {
                idMedicamento = int.Parse(dgvMedicamentos.CurrentRow.Cells[0].Value.ToString());
                txtDescripcion.Text = dgvMedicamentos.CurrentRow.Cells[1].Value.ToString();
                txtNombreD1.Text = dgvMedicamentos.CurrentRow.Cells[2].Value.ToString();
                txtPrecioD1.Text = dgvMedicamentos.CurrentRow.Cells[3].Value.ToString();
                txtNombreD2.Text = dgvMedicamentos.CurrentRow.Cells[4].Value.ToString();
                txtPrecioD2.Text = dgvMedicamentos.CurrentRow.Cells[5].Value.ToString();
                txtPrecioVenta.Text = dgvMedicamentos.CurrentRow.Cells[6].Value.ToString();
                txtInvInicial.Text = dgvMedicamentos.CurrentRow.Cells[7].Value.ToString();
                txtInvFinal.Text = dgvMedicamentos.CurrentRow.Cells[8].Value.ToString();

                btnGuardar.Text = "Actualizar";
                btnEliminar.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnEliminar.Enabled = false;

            Clear();
            gridFill();
        }

        private void txtPrecioD1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            //Solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtPrecioD2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            //Solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            gridFill();
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            //Solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            deleteData();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            search();
        }

        private void txtInvInicial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtInvFinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}