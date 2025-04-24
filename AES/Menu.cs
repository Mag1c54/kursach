using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AES
{
    public partial class Menu : Form
    {
        private Label titleLabel;
        private FlowLayoutPanel menuPanel;
        private Button buttonClients, buttonOrders, buttonProduct, buttonSales, buttonSupplier;

        public Menu()
        {
            InitializeComponent();
            InitializeDarkTheme();
        }


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        private void InitializeDarkTheme()
        {
            this.Text = "Главное меню";
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30); 


            titleLabel = new Label();
            titleLabel.Text = "Добро пожаловать!";
            titleLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titleLabel.ForeColor = Color.WhiteSmoke;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 60;


            menuPanel = new FlowLayoutPanel();
            menuPanel.Dock = DockStyle.Fill;
            menuPanel.FlowDirection = FlowDirection.TopDown;
            menuPanel.WrapContents = false;
            menuPanel.Padding = new Padding(30, 10, 30, 30);
            menuPanel.AutoScroll = true;


            buttonClients = CreateMenuButton("Заказы", button1_Click);
            buttonOrders = CreateMenuButton("Топливо", button2_Click);
            buttonProduct = CreateMenuButton("Клиенты", button3_Click);
            buttonSales = CreateMenuButton("АЗС", button4_Click);
            buttonSupplier = CreateMenuButton("Продажи", button5_Click);


            menuPanel.Controls.Add(buttonClients);
            menuPanel.Controls.Add(buttonOrders);
            menuPanel.Controls.Add(buttonProduct);
            menuPanel.Controls.Add(buttonSales);
            menuPanel.Controls.Add(buttonSupplier);


            this.Controls.Add(menuPanel);
            this.Controls.Add(titleLabel);
        }

        private Button CreateMenuButton(string text, EventHandler clickHandler)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.Size = new Size(300, 50);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(45, 45, 48);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Margin = new Padding(0, 10, 0, 0);
            btn.Cursor = Cursors.Hand;


            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 10, 10));


            btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(65, 65, 70); };
            btn.MouseLeave += (s, e) => { btn.BackColor = Color.FromArgb(45, 45, 48); };

            btn.Click += clickHandler;

            return btn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            CreateOrderForm orderForm = new CreateOrderForm();
            orderForm.ShowDialog();
       

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            FuelForm fuel = new FuelForm();
            fuel.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            СlientsForm сlients = new СlientsForm();
            сlients.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hide();
            AZSForm azs = new AZSForm();
            azs.ShowDialog();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Hide();
            SalesForm sales = new SalesForm();
            sales.ShowDialog();

        }
    }
}