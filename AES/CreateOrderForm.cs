using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class CreateOrderForm : Form
    {
        private DataGridView dataGridView;
        private Button deleteButton; 
        private Button backButton; 
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public CreateOrderForm()
        {
            InitializeComponent();
            InitializeLayout();
            LoadData();
        }

        private void InitializeLayout()
        {
            this.Text = "Создание заказа";
            this.Size = new Size(800, 500); 
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.StartPosition = FormStartPosition.CenterScreen;

         
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 350,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White },
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(50, 50, 55), ForeColor = Color.White }
            };

        
            deleteButton = new Button
            {
                Text = "Удалить заказ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Dock = DockStyle.Bottom
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.Click += DeleteButton_Click;

     
            backButton = new Button
            {
                Text = "Вернуться в меню",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Dock = DockStyle.Bottom
            };
            backButton.FlatAppearance.BorderSize = 0;
            backButton.Click += BackButton_Click;

            this.Controls.Add(dataGridView);
            this.Controls.Add(deleteButton);
            this.Controls.Add(backButton);  
        }

        private void LoadData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Заказы", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
              
                string orderId = dataGridView.SelectedRows[0].Cells["ID_Заказа"].Value.ToString();

       
                var confirmation = MessageBox.Show($"Вы уверены, что хотите удалить заказ с ID: {orderId}?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmation == DialogResult.Yes)
                {
           
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
                                LoadData();
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
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
            }
        }

       

     
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();  
            Menu menuForm = new Menu();  
            menuForm.Show(); 
        }
    }
}
