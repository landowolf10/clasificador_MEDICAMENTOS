using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ClasificadorSqlServer
{
    public partial class frmClasificador : Form
    {
        string mensaje = "";
        int idMedicamento = 0;

        public frmClasificador()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) && string.IsNullOrWhiteSpace(txtDistribuidor.Text) &&
                string.IsNullOrWhiteSpace(txtPrecio.Text) || string.IsNullOrEmpty(txtPrecio.Text))
                MessageBox.Show("Favor de capturar todos los datos.");
            else
            {
                if (btnGuardar.Text == "Actualizar")
                {
                    mensaje = "actualizado";
                    updateData();
                }
                else
                {
                    mensaje = "insertado";
                    insertData();

                    MessageBox.Show("Medicamento " + mensaje + " correctamente");

                    Clear();
                    gridFill();
                }
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

        void insertData()
        {
            SqlConnection conexion = Conexion.obtenerConexion();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAgregarMedicamento";
            cmd.Parameters.Add("@nombre_medicamento", SqlDbType.VarChar).Value = txtNombre.Text;
            cmd.Parameters.Add("@distribuidor_medicamento", SqlDbType.VarChar).Value = txtDistribuidor.Text;
            cmd.Parameters.Add("@precio_medicamento", SqlDbType.Decimal).Value = txtPrecio.Text;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            conexion.Close();
        }

        void updateData()
        {
            SqlConnection conexion = Conexion.obtenerConexion();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spModificarMedicamento";
            cmd.Parameters.Add("@id_medicamento", SqlDbType.Int).Value = idMedicamento;
            cmd.Parameters.Add("@nombre_medicamento", SqlDbType.VarChar).Value = txtNombre.Text;
            cmd.Parameters.Add("@distribuidor_medicamento", SqlDbType.VarChar).Value = txtDistribuidor.Text;
            cmd.Parameters.Add("@precio_medicamento", SqlDbType.Decimal).Value = txtPrecio.Text;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            Clear();
            gridFill();

            conexion.Close();
        }

        void deleteData()
        {
            SqlConnection conexion = Conexion.obtenerConexion();
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
            SqlConnection conexion = Conexion.obtenerConexion();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spBuscarMedicamento";
            cmd.Parameters.Add("@nombre_medicamento", SqlDbType.VarChar).Value = txtBuscar.Text;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dataTable);
                dgvMedicamentos.DataSource = dataTable;
                dgvMedicamentos.Columns[0].Visible = false;
            }
        }

        void gridFill()
        {
            SqlConnection conexion = Conexion.obtenerConexion();
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
            txtNombre.Text = "";
            txtDistribuidor.Text = "";
            txtPrecio.Text = "";
            btnGuardar.Text = "Guardar";
            btnEliminar.Enabled = false;
        }

        private void frmClasificador_Load(object sender, EventArgs e)
        {
            btnEliminar.Enabled = false;

            Clear();
            gridFill();
        }

        private void dgvMedicamentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMedicamentos.CurrentRow.Index != -1)
            {
                idMedicamento = int.Parse(dgvMedicamentos.CurrentRow.Cells[0].Value.ToString());
                txtNombre.Text = dgvMedicamentos.CurrentRow.Cells[1].Value.ToString();
                txtDistribuidor.Text = dgvMedicamentos.CurrentRow.Cells[2].Value.ToString();
                txtPrecio.Text = dgvMedicamentos.CurrentRow.Cells[3].Value.ToString();

                btnGuardar.Text = "Actualizar";
                btnEliminar.Enabled = true;
            }
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
    }
}
