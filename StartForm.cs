using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalTestForFun
{
    public partial class StartForm : Form
    {
        public int GameSizeX = 5;
        public int GameSizeY = 5;
        public int BombCount = 3;
        public int GameSizeX_min = 5;
        public int GameSizeX_max = 32;
        public int GameSizeY_min = 5;
        public int GameSizeY_max = 22;

        public StartForm()
        {
            InitializeComponent();
        }
        private void StartForm_Load(object sender, EventArgs e)
        {
            PanelVisible(false);
            PrintAllVariablesToTextBox();

            foreach (var item in this.panel1.Controls)
            {
                if (item is Button)
                {
                    ((Button)item).Click += ButtonClick;
                }
            }
            foreach (var item in this.panel1.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).TextChanged += TextBoxTextChanged;
                }
            }
        }
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            try
            {
                switch (((TextBox)sender).Name)
                {
                    case "TBW":
                        GameSizeX = int.Parse(TBW.Text);
                        break;
                    case "TBH":
                        GameSizeY = int.Parse(TBH.Text);
                        break;
                    case "TBB":
                        BombCount = int.Parse(TBB.Text);
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Вводите только цифры", "Error");
            }
            PrintAllVariablesToTextBox();
        }
        private void ButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "Wm1":
                    GameSizeX -= 1;
                    TBW.Text = GameSizeX.ToString();
                    break;
                case "Wp1":
                    GameSizeX += 1;
                    TBW.Text = GameSizeX.ToString();
                    break;
                case "Wm5":
                    GameSizeX -= 5;
                    TBW.Text = GameSizeX.ToString();
                    break;
                case "Wp5":
                    GameSizeX += 5;
                    TBW.Text = GameSizeX.ToString();
                    break;
                case "Hm1":
                    GameSizeY -= 1;
                    TBH.Text = GameSizeY.ToString();
                    break;
                case "Hp1":
                    GameSizeY += 1;
                    TBH.Text = GameSizeY.ToString();
                    break;
                case "Hm5":
                    GameSizeY -= 5;
                    TBH.Text = GameSizeY.ToString();
                    break;
                case "Hp5":
                    GameSizeY += 5;
                    TBH.Text = GameSizeY.ToString();
                    break;
                case "Bm1":
                    BombCount -= 1;
                    TBB.Text = BombCount.ToString();
                    break;
                case "Bp1":
                    BombCount += 1;
                    TBB.Text = BombCount.ToString();
                    break;
                case "Bm5":
                    BombCount -= 5;
                    TBB.Text = BombCount.ToString();
                    break;
                case "Bp5":
                    BombCount += 5;
                    TBB.Text = BombCount.ToString();
                    break;
                case "Bm10":
                    BombCount -= 10;
                    TBB.Text = BombCount.ToString();
                    break;
                case "Bp10":
                    BombCount += 10;
                    TBB.Text = BombCount.ToString();
                    break;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Visible = false;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    GameSizeX = 10;
                    GameSizeY = 8;
                    BombCount = 10;
                    break;
                case 1:
                    GameSizeX = 18;
                    GameSizeY = 14;
                    BombCount = 40;
                    break;
                case 2:
                    GameSizeX = 24;
                    GameSizeY = 20;
                    BombCount = 99;
                    break;
                case 3:
                    GameSizeX = 32;
                    GameSizeY = 22;
                    BombCount = 200;
                    break;
                case 4:
                    Visible = true;
                    break;
            }
            PrintAllVariablesToTextBox();
            PanelVisible(Visible);
        }
        public void PanelVisible(bool Visible)
        {
            if (Visible)
            {
                this.Width = 568;
                this.Height = 643;
                button1.Location = new Point(159, 501);
                panel1.Visible = true;

            } 
            else
            {
                panel1.Visible = false;
                this.Width = 568;
                this.Height = 350;
                button1.Location = new Point(159, 212);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double GridSize = GameSizeX * GameSizeY;
            double Сoefficient = new double();
            
            if (GameSizeX < GameSizeX_min)
            {
                MessageBox.Show("Ширина меньше допустимой", "Error");
                return;
            }
            if (GameSizeX > GameSizeX_max)
            {
                MessageBox.Show("Ширина больше допустимой", "Error");
                return;
            }
            if (GameSizeY < GameSizeY_min)
            {
                MessageBox.Show("Высота меньше допустимой", "Error");
                return;
            }
            if (GameSizeY > GameSizeY_max)
            {
                MessageBox.Show("Высота больше допустимой", "Error");
                return;
            }

            if (GridSize <= BombCount)
            {
                MessageBox.Show("Неверное количество бомб", "Error");
                return;
            }

            if (GridSize >= 25 && GridSize <= 80)
                Сoefficient = 7.0;
            if (GridSize >= 81 && GridSize <= 250)
                Сoefficient = 6.3;
            if (GridSize >= 251 && GridSize <= 480)
                Сoefficient = 4.8;
            if (GridSize >= 481 && GridSize <= 704)
                Сoefficient = 3.52;

            int BombMinValue = Convert.ToInt32(GridSize / (Сoefficient+2.0));
            int BombMaxValue = Convert.ToInt32(GridSize / Сoefficient);

            if (!(BombCount <= BombMaxValue && BombCount >= BombMinValue))
            {
                MessageBox.Show("Неверное количество бомб", "Error");
                return;
            }

            var Form1 = new Game(GameSizeX, GameSizeY, BombCount);
            Form1.Closed += (s, args) => this.Close();
            Form1.Show();
            this.Hide();
        }
        private void PrintAllVariablesToTextBox()
        {
            TBW.Text = GameSizeX.ToString();
            TBH.Text = GameSizeY.ToString();
            TBB.Text = BombCount.ToString();
        }
    }
}
