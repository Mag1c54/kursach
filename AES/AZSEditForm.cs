using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class AZSEditForm : Form
    {
        private string azsId; 
        private TextBox txtAddress, txtName;
        private Button saveButton;
       
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public AZSEditForm(string id)
        {
            azsId = id;
            InitializeComponent();
            InitializeLayout();

            if (azsId != null)
                LoadAZSData();
        }

        private void InitializeLayout()
        {
            this.Text = azsId == null ? "Добавить АЗС" : "Редактировать АЗС";
            this.Size = new Size(350, 250);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblAddress = new Label { Text = "Адрес:", ForeColor = Color.White, Top = 20, Left = 20, Width = 80 };
            txtAddress = new TextBox { Top = 20, Left = 110, Width = 180 };

            Label lblName = new Label { Text = "Название:", ForeColor = Color.White, Top = 60, Left = 20, Width = 80 };
            txtName = new TextBox { Top = 60, Left = 110, Width = 180 };

            saveButton = new Button
            {
                Text = "Сохранить",
                Top = 110,
                Left = 110,
                Width = 180,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            this.Controls.Add(lblAddress);
            this.Controls.Add(txtAddress);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(saveButton);
        }

        private void LoadAZSData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Адрес, Название FROM АЗС WHERE ID_АЗС = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", azsId);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtAddress.Text = reader["Адрес"].ToString();
                        txtName.Text = reader["Название"].ToString();
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd;

                if (azsId == null)
                {
                    cmd = new OleDbCommand("INSERT INTO АЗС (Адрес, Название) VALUES (?, ?)", conn);
                    cmd.Parameters.AddWithValue("?", txtAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtName.Text);
                }
                else
                {
                    cmd = new OleDbCommand("UPDATE АЗС SET Адрес = ?, Название = ? WHERE ID_АЗС = ?", conn);
                    cmd.Parameters.AddWithValue("?", txtAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtName.Text);
                    cmd.Parameters.AddWithValue("?", azsId);
                }

                cmd.ExecuteNonQuery();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
