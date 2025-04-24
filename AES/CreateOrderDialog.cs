using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace AES
{
    public partial class CreateOrderDialog : Form
    {
        private ComboBox fuelTypeComboBox;
        private TextBox quantityTextBox;
        private Button saveButton;
        private string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={System.IO.Path.Combine(Application.StartupPath, "database.accdb")};";


        private readonly string clientLogin;

        public CreateOrderDialog(string login)
        {
            clientLogin = login;
            InitializeComponent();
            InitializeLayout();
            LoadFuelTypes();
        }

        private void InitializeLayout()
        {
            this.Text = "Добавление заказа";
            this.Size = new Size(400, 250);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            fuelTypeComboBox = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 40,
                DropDownStyle = ComboBoxStyle.DropDownList
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
            quantityTextBox.Text = "Кол-во литров";

            saveButton = new Button
            {
                Text = "Сохранить заказ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Bottom,
                Height = 40,
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;
            saveButton.MouseEnter += (s, e) => saveButton.BackColor = Color.FromArgb(65, 65, 70);
            saveButton.MouseLeave += (s, e) => saveButton.BackColor = Color.FromArgb(45, 45, 48);

            this.Controls.Add(fuelTypeComboBox);
            this.Controls.Add(quantityTextBox);
            this.Controls.Add(saveButton);
        }

        private void LoadFuelTypes()
        {
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT ID_Топливо, Название FROM Топливо", conn);
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fuelTypeComboBox.Items.Add(reader["Название"].ToString());
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов топлива: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn?.Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (fuelTypeComboBox.SelectedItem == null ||
                !int.TryParse(quantityTextBox.Text, out var quantity) ||
                quantity <= 0)
            {
                MessageBox.Show("Пожалуйста, выберите топливо и укажите корректное количество.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedFuel = fuelTypeComboBox.SelectedItem.ToString();
            var fuelTypeId = GetFuelTypeId(selectedFuel);
            var clientId = GetClientIdByLogin(clientLogin);

            if (fuelTypeId == 0 || clientId == 0)
            {
                MessageBox.Show("Не удалось определить необходимые идентификаторы.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
                var query = @"
                    INSERT INTO [Заказы] 
                        ([ID_Клиента], [ID_Топливо], [Дата], [Кол_во_литров]) 
                    VALUES (?, ?, ?, ?)";
                var cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = clientId;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = fuelTypeId;
                cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = quantity;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn?.Close();
            }

            MessageBox.Show("Заказ успешно создан!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private int GetFuelTypeId(string fuelName)
        {
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
                const string sql = "SELECT ID_Топливо FROM Топливо WHERE Название = ?";
                var cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.Add("?", OleDbType.VarChar).Value = fuelName;
                var obj = cmd.ExecuteScalar();
                return obj == null || obj == DBNull.Value ? 0 : Convert.ToInt32(obj);
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Ошибка при получении ID топлива: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            finally
            {
                conn?.Close();
            }
        }

        private int GetClientIdByLogin(string login)
        {
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
                const string sql = "SELECT ID_Клиента FROM Клиенты WHERE Логин = ?";
                var cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.Add("?", OleDbType.VarChar).Value = login;
                var obj = cmd.ExecuteScalar();
                return obj == null || obj == DBNull.Value ? 0 : Convert.ToInt32(obj);
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Ошибка при получении ID клиента: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}
