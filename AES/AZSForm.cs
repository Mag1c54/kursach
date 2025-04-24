using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class AZSForm : Form
    {
        private DataGridView dataGridView;
        private Button addButton;
        private Button backButton; 
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public AZSForm()
        {
            InitializeComponent();
            InitializeLayout();
            LoadData();
        }

        private void InitializeLayout()
        {
            this.Text = "АЗС";
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

            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;

         
            addButton = new Button
            {
                Text = "Добавить АЗС",
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
            this.Controls.Add(addButton);
            this.Controls.Add(backButton); 
        }

        private void LoadData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM АЗС", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AZSEditForm editForm = new AZSEditForm(null);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string azsId = dataGridView.Rows[e.RowIndex].Cells["ID_АЗС"].Value.ToString();
                AZSEditForm editForm = new AZSEditForm(azsId);
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
