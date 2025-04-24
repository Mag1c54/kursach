using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class Auth : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\New\Desktop\AES\AES\database.accdb;";

        public Auth()
        {
            InitializeComponent();
            this.Load += Auth_Load;
        }

        private void Auth_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            label1.ForeColor = Color.WhiteSmoke;
            label2.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.WhiteSmoke;
            label1.Font = new Font("Segoe UI", 20, FontStyle.Regular);
            label2.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            label3.Font = new Font("Segoe UI", 11, FontStyle.Regular);

            textBox1.Font = new Font("Segoe UI", 11);
            textBox2.Font = new Font("Segoe UI", 11);
            textBox1.BackColor = Color.FromArgb(50, 50, 50);
            textBox2.BackColor = Color.FromArgb(50, 50, 50);
            textBox1.ForeColor = Color.White;
            textBox2.ForeColor = Color.White;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox2.BorderStyle = BorderStyle.FixedSingle;

            textBox2.PasswordChar = '●';

            button1.Text = "Войти";
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.BackColor = Color.FromArgb(45, 45, 48);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Cursor = Cursors.Hand;
            button1.Click += button1_Click;

            
            Button registerButton = new Button
            {
                Text = "Регистрация",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Bottom,
                Height = 40
            };
            registerButton.FlatAppearance.BorderSize = 0;
            registerButton.Cursor = Cursors.Hand;
            registerButton.Click += RegisterButton_Click;

            this.Controls.Add(registerButton);

            button1.MouseEnter += (s, args) => button1.BackColor = Color.FromArgb(65, 65, 70);
            button1.MouseLeave += (s, args) => button1.BackColor = Color.FromArgb(45, 45, 48);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            
            if (login == "admin" && password == "1234")
            {
                Menu adminMenu = new Menu();
                adminMenu.Show();
                this.Hide();
                return;
            }

            
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID_Клиента FROM Клиенты WHERE Логин = ? AND Пароль = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientId", login);
                cmd.Parameters.AddWithValue("?", password);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    
                    CreateOrderDialog clientForm = new CreateOrderDialog(login);
                    clientForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();
            this.Hide();
        }
    }
}
