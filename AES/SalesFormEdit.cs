using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class SalesFormEdit : Form
    {
        private string prodazhaId;
        private TextBox txtAZS, txtToplivo, txtKolvo, txtStoimost, txtZapas, txtKlient;
        private DateTimePicker dtpDate;
        private Button saveButton;

        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        public SalesFormEdit(string id)
        {
            prodazhaId = id;
            InitializeComponent();
            InitializeLayout();

            if (prodazhaId != null)
                LoadProdazhaData();
        }

        private void InitializeLayout()
        {
            this.Text = prodazhaId == null ? "Добавить Продажу" : "Редактировать Продажу";
            this.Size = new Size(400, 400);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            int y = 20;

            Label lblAZS = CreateLabel("ID АЗС:", y); txtAZS = CreateTextBox(y); y += 40;
            Label lblToplivo = CreateLabel("ID Топливо:", y); txtToplivo = CreateTextBox(y); y += 40;
            Label lblDate = CreateLabel("Дата:", y); dtpDate = new DateTimePicker { Top = y, Left = 120, Width = 200 }; y += 40;
            Label lblKolvo = CreateLabel("Кол-во литров:", y); txtKolvo = CreateTextBox(y); y += 40;
            Label lblStoimost = CreateLabel("Общая стоимость:", y); txtStoimost = CreateTextBox(y); y += 40;
            Label lblZapas = CreateLabel("ID Запаса:", y); txtZapas = CreateTextBox(y); y += 40;
            Label lblKlient = CreateLabel("ID Клиента:", y); txtKlient = CreateTextBox(y); y += 40;

            saveButton = new Button
            {
                Text = "Сохранить",
                Top = y,
                Left = 120,
                Width = 200,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            this.Controls.AddRange(new Control[] {
                lblAZS, txtAZS, lblToplivo, txtToplivo, lblDate, dtpDate,
                lblKolvo, txtKolvo, lblStoimost, txtStoimost,
                lblZapas, txtZapas, lblKlient, txtKlient, saveButton
            });
        }

        private Label CreateLabel(string text, int top) =>
            new Label { Text = text, ForeColor = Color.White, Top = top, Left = 20, Width = 100 };

        private TextBox CreateTextBox(int top) =>
            new TextBox { Top = top, Left = 120, Width = 200 };

        private void LoadProdazhaData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Продажи WHERE ID_Продажи = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", prodazhaId);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtAZS.Text = reader["ID_АЗС"].ToString();
                        txtToplivo.Text = reader["ID_Топливо"].ToString();
                        dtpDate.Value = Convert.ToDateTime(reader["Дата"]);
                        txtKolvo.Text = reader["Кол_во_литров"].ToString();
                        txtStoimost.Text = reader["Общая_стоимость"].ToString();
                        txtZapas.Text = reader["ID_Запаса"].ToString();
                        txtKlient.Text = reader["ID_Клиента"].ToString();
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            
            if (!ValidateInputs())
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd;

                
                if (prodazhaId == null)
                {
                    cmd = new OleDbCommand("INSERT INTO Продажи (ID_АЗС, ID_Топливо, Дата, Кол_во_литров, Общая_стоимость, ID_Запаса, ID_Клиента) VALUES (?, ?, ?, ?, ?, ?, ?)", conn);
                }
                else 
                {
                    cmd = new OleDbCommand("UPDATE Продажи SET ID_АЗС = ?, ID_Топливо = ?, Дата = ?, Кол_во_литров = ?, Общая_стоимость = ?, ID_Запаса = ?, ID_Клиента = ? WHERE ID_Продажи = ?", conn);
                }

                
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtAZS.Text)); 
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtToplivo.Text)); 
                cmd.Parameters.AddWithValue("?", dtpDate.Value); 
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtKolvo.Text)); 
                cmd.Parameters.AddWithValue("?", Convert.ToDecimal(txtStoimost.Text)); 
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtZapas.Text)); 
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtKlient.Text)); 

                
                if (prodazhaId != null)
                {
                    cmd.Parameters.AddWithValue("?", prodazhaId); 
                }

                
                cmd.ExecuteNonQuery();
            }

            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidateInputs()
        {
            
            if (string.IsNullOrWhiteSpace(txtAZS.Text) || !int.TryParse(txtAZS.Text, out _))
            {
                MessageBox.Show("ID АЗС должен быть числом.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtToplivo.Text) || !int.TryParse(txtToplivo.Text, out _))
            {
                MessageBox.Show("ID Топливо должен быть числом.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtKolvo.Text) || !int.TryParse(txtKolvo.Text, out _))
            {
                MessageBox.Show("Кол-во литров должно быть числом.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtStoimost.Text) || !decimal.TryParse(txtStoimost.Text, out _))
            {
                MessageBox.Show("Общая стоимость должна быть числом.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtZapas.Text) || !int.TryParse(txtZapas.Text, out _))
            {
                MessageBox.Show("ID Запас должен быть числом.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtKlient.Text) || !int.TryParse(txtKlient.Text, out _))
            {
                MessageBox.Show("ID Клиент должен быть числом.");
                return false;
            }

            return true;
        }
    }
}
