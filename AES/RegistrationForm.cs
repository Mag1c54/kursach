using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class RegistrationForm : Form
    {
        private TextBox nameTextBox;
        private TextBox phoneTextBox;
        private TextBox emailTextBox;
        private TextBox addressTextBox;
        private TextBox loginTextBox;
        private TextBox passwordTextBox;
        private Button registerButton;

        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";

        public RegistrationForm()
        {
            InitializeComponent();
            this.Load += RegistrationForm_Load;
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            this.Text = "Регистрация";
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(20),
            };

            Label label = new Label
            {
                Text = "Регистрация",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke,
                Height = 60,
                Width = 340,
                TextAlign = ContentAlignment.MiddleCenter
            };

            nameTextBox = CreateTextBox("ФИО");
            phoneTextBox = CreateTextBox("Телефон");
            emailTextBox = CreateTextBox("Email");
            addressTextBox = CreateTextBox("Адрес");
            loginTextBox = CreateTextBox("Логин");
            passwordTextBox = CreateTextBox("Пароль");

            registerButton = new Button
            {
                Text = "Зарегистрироваться",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Width = 340
            };
            registerButton.FlatAppearance.BorderSize = 0;
            registerButton.Click += RegisterButton_Click;

            panel.Controls.Add(label);
            panel.Controls.Add(nameTextBox);
            panel.Controls.Add(phoneTextBox);
            panel.Controls.Add(emailTextBox);
            panel.Controls.Add(addressTextBox);
            panel.Controls.Add(loginTextBox);
            panel.Controls.Add(passwordTextBox);
            panel.Controls.Add(registerButton);

            this.Controls.Add(panel);
        }

        private TextBox CreateTextBox(string placeholder)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Width = 340,
                Height = 35,
        
            };
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(phoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                string.IsNullOrWhiteSpace(addressTextBox.Text) ||
                string.IsNullOrWhiteSpace(loginTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Клиенты (ФИО, Телефон, Email, Адрес, Логин, Пароль) VALUES (?, ?, ?, ?, ?, ?)";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("?", phoneTextBox.Text);
                    cmd.Parameters.AddWithValue("?", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("?", addressTextBox.Text);
                    cmd.Parameters.AddWithValue("?", loginTextBox.Text);
                    cmd.Parameters.AddWithValue("?", passwordTextBox.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
