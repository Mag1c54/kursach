using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class EditClient : Form
    {
        private string clientId; 
        private string clientLogin; 
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";

        private TextBox txtName, txtPhone, txtEmail, txtAddress, txtLogin, txtPassword;
        private Button saveButton;

        public EditClient(string login)
        {
            clientLogin = login;
            InitializeComponent();
            InitializeLayout();

            LoadClientIdByLogin();
            if (clientId != null)
                LoadClientData();
        }

        private void LoadClientIdByLogin()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID_Клиента FROM Клиенты WHERE Логин = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", clientLogin);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        clientId = reader["ID_Клиента"].ToString();
                    }
                }
            }
        }

        private void InitializeLayout()
        {
            this.Text = clientId == null ? "Добавить клиента" : "Редактировать клиента";
            this.Size = new Size(400, 420);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            int top = 20;

            txtName = CreateField("ФИО:", ref top);
            txtPhone = CreateField("Телефон:", ref top);
            txtEmail = CreateField("Email:", ref top);
            txtAddress = CreateField("Адрес:", ref top);
            txtLogin = CreateField("Логин:", ref top);
            txtPassword = CreateField("Пароль:", ref top);

            saveButton = new Button
            {
                Text = "Сохранить",
                Top = top + 10,
                Left = 110,
                Width = 240,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);
        }

        private TextBox CreateField(string labelText, ref int top)
        {
            Label label = new Label
            {
                Text = labelText,
                ForeColor = Color.White,
                Top = top,
                Left = 20,
                Width = 80
            };
            TextBox textbox = new TextBox
            {
                Top = top,
                Left = 110,
                Width = 240
            };
            this.Controls.Add(label);
            this.Controls.Add(textbox);
            top += 40;
            return textbox;
        }

        private void LoadClientData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ФИО, Телефон, Email, Адрес, Логин, Пароль FROM Клиенты WHERE ID_Клиента = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", clientId);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtName.Text = reader["ФИО"].ToString();
                        txtPhone.Text = reader["Телефон"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtAddress.Text = reader["Адрес"].ToString();
                        txtLogin.Text = reader["Логин"].ToString();
                        txtPassword.Text = reader["Пароль"].ToString();
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd;

                if (clientId == null)
                {
                    cmd = new OleDbCommand("INSERT INTO Клиенты (ФИО, Телефон, Email, Адрес, Логин, Пароль) VALUES (?, ?, ?, ?, ?, ?)", conn);
                    cmd.Parameters.AddWithValue("?", txtName.Text);
                    cmd.Parameters.AddWithValue("?", txtPhone.Text);
                    cmd.Parameters.AddWithValue("?", txtEmail.Text);
                    cmd.Parameters.AddWithValue("?", txtAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtLogin.Text);
                    cmd.Parameters.AddWithValue("?", txtPassword.Text);
                }
                else
                {
                    cmd = new OleDbCommand("UPDATE Клиенты SET ФИО = ?, Телефон = ?, Email = ?, Адрес = ?, Логин = ?, Пароль = ? WHERE ID_Клиента = ?", conn);
                    cmd.Parameters.AddWithValue("?", txtName.Text);
                    cmd.Parameters.AddWithValue("?", txtPhone.Text);
                    cmd.Parameters.AddWithValue("?", txtEmail.Text);
                    cmd.Parameters.AddWithValue("?", txtAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtLogin.Text);
                    cmd.Parameters.AddWithValue("?", txtPassword.Text);
                    cmd.Parameters.AddWithValue("?", clientId);
                }

                cmd.ExecuteNonQuery();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
