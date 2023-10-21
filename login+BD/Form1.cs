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
using BCrypt.Net;


namespace login_BD
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=DESKTOP-R4TG502;Initial Catalog=DB_login_DB1;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            string nombre = NombreTextBox.Text;
            string contraseña = ContraseñaTextBox.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, contraseña, bloqueada, salt FROM tbl_user WHERE nombre = @Nombre";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            string hashedPassword = reader.GetString(1);
                            bool bloqueada = reader.GetBoolean(2);
                            string salt = reader.GetString(3);

                            if (bloqueada)
                            {
                                MessageBox.Show("La cuenta está bloqueada. Comunícate con el soporte.");
                            }
                            else
                            {
                                if (BCrypt.Net.BCrypt.Verify(contraseña, hashedPassword))
                                {
                                    MessageBox.Show("Inicio de sesión exitoso");
                                    // Realiza acciones adicionales tras un inicio de sesión exitoso.
                                }
                                else
                                {
                                    MessageBox.Show("Contraseña incorrecta.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Usuario no encontrado");
                        }
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}