using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class OrderDialog : Form
    {
        private string orderId;
        private TextBox fuelTypeTextBox;
        private TextBox quantityTextBox;
        private Button saveButton;
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public OrderDialog(string orderId = null)
        {
            this.orderId = orderId;
            InitializeComponent();
            InitializeLayout();
            if (orderId != null)
            {
                LoadOrderData();
            }
        }

        private void InitializeLayout()
        {
            this.Text = orderId == null ? "Добавление нового заказа" : "Редактирование заказа";
            this.Size = new Size(400, 250);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.StartPosition = FormStartPosition.CenterParent;

            fuelTypeTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Height = 40,
            };

            quantityTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Height = 40,
            };

            saveButton = new Button
            {
                Text = "Сохранить",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Dock = DockStyle.Bottom
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            this.Controls.Add(fuelTypeTextBox);
            this.Controls.Add(quantityTextBox);
            this.Controls.Add(saveButton);
        }

        private void LoadOrderData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM Заказы WHERE ID_Заказа = ?", conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = Convert.ToInt32(orderId);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    fuelTypeTextBox.Text = reader["ID_Топливо"].ToString();
                    quantityTextBox.Text = reader["Кол_во_литров"].ToString();
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            
            if (orderId == null)
            {
                MessageBox.Show("Невозможно удалить. ID заказа не найден.");
                return;
            }

            
            var confirmation = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmation == DialogResult.No)
            {
                return; 
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand("DELETE FROM Заказы WHERE ID_Заказа = ?", conn);
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = Convert.ToInt32(orderId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Заказ успешно удален!");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти заказ для удаления.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении заказа: " + ex.Message);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }

    }
}
