using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class EditFuel : Form
    {
        private string fuelId; 
        private TextBox txtFuelName, txtPrice, txtType;
        private Button saveButton;

        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public EditFuel(string id)
        {
            fuelId = id;
            InitializeComponent();
            InitializeLayout();

            if (fuelId != null)
                LoadFuelData();
        }

        private void InitializeLayout()
        {
            this.Text = fuelId == null ? "Добавить топливо" : "Редактировать топливо";
            this.Size = new Size(350, 250);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            
            Label lblFuelName = new Label { Text = "Название:", ForeColor = Color.White, Top = 20, Left = 20, Width = 80 };
            txtFuelName = new TextBox { Top = 20, Left = 110, Width = 180 };

            Label lblPrice = new Label { Text = "Цена:", ForeColor = Color.White, Top = 60, Left = 20, Width = 80 };
            txtPrice = new TextBox { Top = 60, Left = 110, Width = 180 };

            Label lblType = new Label { Text = "Тип:", ForeColor = Color.White, Top = 100, Left = 20, Width = 80 };
            txtType = new TextBox { Top = 100, Left = 110, Width = 180 };

            
            saveButton = new Button
            {
                Text = "Сохранить",
                Top = 140,
                Left = 110,
                Width = 180,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            this.Controls.Add(lblFuelName);
            this.Controls.Add(txtFuelName);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(lblType);
            this.Controls.Add(txtType);
            this.Controls.Add(saveButton);
        }

        private void LoadFuelData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Название, Стоимость_за_литр, Тип FROM Топливо WHERE ID_Топливо = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", fuelId);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtFuelName.Text = reader["Название"].ToString();
                        txtPrice.Text = reader["Стоимость_за_литр"].ToString();
                        txtType.Text = reader["Тип"].ToString();
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFuelName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtType.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd;

                if (fuelId == null)
                {
                    
                    cmd = new OleDbCommand("INSERT INTO Топливо (Название, Стоимость_за_литр, Тип) VALUES (?, ?, ?)", conn);
                    cmd.Parameters.AddWithValue("?", txtFuelName.Text);
                    cmd.Parameters.AddWithValue("?", txtPrice.Text);
                    cmd.Parameters.AddWithValue("?", txtType.Text);
                }
                else
                {
                    
                    if (string.IsNullOrEmpty(fuelId))
                    {
                        MessageBox.Show("ID топлива не найден. Пожалуйста, выберите правильное топливо для редактирования.");
                        return;
                    }

                    cmd = new OleDbCommand("UPDATE Топливо SET Название = ?,Стоимость_за_литр = ?, Тип = ? WHERE ID_Топливо = ?", conn);
                    cmd.Parameters.AddWithValue("?", txtFuelName.Text);
                    cmd.Parameters.AddWithValue("?", txtPrice.Text);
                    cmd.Parameters.AddWithValue("?", txtType.Text);
                    cmd.Parameters.AddWithValue("?", fuelId);  
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Запись успешно сохранена.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
