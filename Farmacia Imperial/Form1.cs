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

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) || string.IsNullOrEmpty(txtPrecio.Text) &&
                string.IsNullOrWhiteSpace(txtInvInicial.Text) || string.IsNullOrEmpty(txtInvInicial.Text))
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

        void insertData()
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAgregarMedicamento";
            cmd.Parameters.Add("@descripcion_medicamento", SqlDbType.VarChar).Value = txtDescripcion.Text;
            cmd.Parameters.Add("@precio_medicamento", SqlDbType.Decimal).Value = txtPrecio.Text;
            cmd.Parameters.Add("@inv_inicial", SqlDbType.Int).Value = txtInvInicial.Text;

            cmd.Connection = conexion;

            cmd.ExecuteNonQuery();

            conexion.Close();
        }

        void updateData()
        {
            SqlConnection conexion = Connection.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spModificarMedicamento";
            cmd.Parameters.Add("@id_medicamento", SqlDbType.Int).Value = idMedicamento;
            cmd.Parameters.Add("@descripcion_medicamento", SqlDbType.VarChar).Value = txtDescripcion.Text;
            cmd.Parameters.Add("@precio_medicamento", SqlDbType.Decimal).Value = txtPrecio.Text;
            cmd.Parameters.Add("@inv_inicial", SqlDbType.Int).Value = txtInvInicial.Text;

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
            txtPrecio.Text = "";
            txtInvInicial.Text = "";
            btnGuardar.Text = "Guardar";
            btnEliminar.Enabled = false;
        }

        private void dgvMedicamentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMedicamentos.CurrentRow.Index != -1)
            {
                idMedicamento = int.Parse(dgvMedicamentos.CurrentRow.Cells[0].Value.ToString());
                txtDescripcion.Text = dgvMedicamentos.CurrentRow.Cells[1].Value.ToString();
                txtPrecio.Text = dgvMedicamentos.CurrentRow.Cells[2].Value.ToString();
                txtInvInicial.Text = dgvMedicamentos.CurrentRow.Cells[3].Value.ToString();

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
    }
}