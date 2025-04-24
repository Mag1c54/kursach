using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class SalesForm : Form
    {
        private DataGridView dataGridView;
        private Button addButton;
        private Button backButton; 
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public SalesForm()
        {
            InitializeComponent();
            InitializeLayout();
            LoadData();
        }

        private void InitializeLayout()
        {
            this.Text = "Продажи";
            this.Size = new Size(1000, 500);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.StartPosition = FormStartPosition.CenterScreen;

            
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 400,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(60, 60, 65),
                    ForeColor = Color.White
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(50, 50, 55),
                    ForeColor = Color.White
                }
            };

            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;

            
            addButton = new Button
            {
                Text = "Добавить продажу",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += AddButton_Click;

            
            backButton = new Button
            {
                Text = "Вернуться в меню",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            backButton.FlatAppearance.BorderSize = 0;
            backButton.Click += BackButton_Click;

            this.Controls.Add(dataGridView);
            this.Controls.Add(addButton);
            this.Controls.Add(backButton);
        }

        private void LoadData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        Продажи.ID_Продажи, 
                        АЗС.Название AS АЗС, 
                        Топливо.Название AS Топливо, 
                        Продажи.Дата, 
                        Продажи.Кол_во_литров, 
                        Продажи.Общая_стоимость,
                        Запасы_топлива.ID_запаса,
                        Клиенты.ФИО AS Клиент
                    FROM (((Продажи
                    INNER JOIN АЗС ON Продажи.ID_АЗС = АЗС.ID_АЗС)
                    INNER JOIN Топливо ON Продажи.ID_Топливо = Топливо.ID_Топливо)
                    INNER JOIN Запасы_топлива ON Продажи.ID_Запаса = Запасы_топлива.ID_запаса)
                     LEFT JOIN Клиенты ON Продажи.ID_Клиента = Клиенты.ID_Клиента";

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            SalesFormEdit editForm = new SalesFormEdit(null);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string saleId = dataGridView.Rows[e.RowIndex].Cells["ID_Продажи"].Value.ToString();
                SalesFormEdit editForm = new SalesFormEdit(saleId);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
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
