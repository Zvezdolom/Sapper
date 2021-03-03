using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalTestForFun
{
    public partial class Game : Form
    {
        // Main Game Settings
        public static int GameSizeX = new int();
        public static int GameSizeY = new int();
        public static int BombCount = new int();

        // Other Variables
        public static int GridSize = new int();
        public int[] DataArray = new int[new int()];
        public int BombCode = -5;
        public int OpenValue = 0;
        Random RandomValue = new Random();

        public Color ColorLightGreen = Color.FromArgb(170, 215, 81);
        public Color ColorDarkGreen = Color.FromArgb(162, 209, 73);
        public Color ColorLightBrown = Color.FromArgb(229, 194, 159);
        public Color ColorDarkBrown = Color.FromArgb(215, 184, 153);
        public Color ColorQuestion = Color.FromArgb(193, 40, 40);
        public Color BackColorQuestion = Color.FromArgb(206, 174, 143);
        public Color ColorBomb = Color.FromArgb(56, 56, 56);
        public Color BackColorBomb = Color.FromArgb(182, 149, 116);
        public Color Value1 = Color.FromArgb(25, 118, 210);
        public Color Value2 = Color.FromArgb(56, 142, 60);
        public Color Value3 = Color.FromArgb(211, 47, 47);
        public Color Value4 = Color.FromArgb(123, 31, 162);
        public Color Value5 = Color.FromArgb(254, 144, 4);
        public Color Value6 = Color.FromArgb(0, 151, 167);
        public Color Value7 = Color.FromArgb(66, 66, 66);
        public Color Value8 = Color.FromArgb(244, 194, 13);

        public Game(int GameSizeX, int GameSizeY, int BombCount)
        {
            InitializeComponent();
            Game.GameSizeX = GameSizeX;
            Game.GameSizeY = GameSizeY;
            Game.BombCount = BombCount;
            GridSize = GameSizeX * GameSizeY;
            DataArray = new int[GridSize];

            Setup();
            ButtonGenerator();
            MapGenerator();
        }
        public void Setup()
        {
            this.Width = 34 + (40 * GameSizeX);
            this.Height = 55 + (40 * GameSizeY)+75;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Сапер";
        }
        public void ButtonGenerator()
        {
            int top = 10+75;
            int left = 10;
            bool color = true;

            for (int i = 0; i < GridSize; i++)
            {
                Button button = new Button()
                {
                    Left = left,
                    Top = top,
                    Width = 40,
                    Height = 40,
                    TabStop = false,
                    Font = new Font("Microsoft Sans Serif", 21, FontStyle.Bold),
                    Name = ("" + (i + 0)).ToString(),
                    Text = "",
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Flat
                };

                if (color)
                    button.BackColor = ColorLightGreen;
                else
                    button.BackColor = ColorDarkGreen;
                color = !color;

                button.FlatAppearance.BorderSize = 0;

                this.Controls.Add(button);

                left += button.Height;

                if (i % GameSizeX == GameSizeX - 1)
                {
                    top += button.Width;
                    left = 10;
                    if (GameSizeX % 2 == 0)
                        color = !color;
                }
            }

            foreach (var item in this.Controls)
                if (item is Button)
                    ((Button)item).MouseDown += new MouseEventHandler(ButtonClick);
        }
        public void MapGenerator()
        {
            for (int i = 0; i < DataArray.Length; i++)
                DataArray[i] = 0;

            for (int i = 0; i < BombCount; i++)
            {
                int Pos = RandomValue.Next(0, GridSize);
                if (DataArray[Pos] != BombCode)
                {
                    DataArray[Pos] = BombCode;
                    AddValueAroundBombs(Pos);
                }
                else
                {
                    i--;
                }
            }
        }
        public void AddValueAroundBombs(int Pos)
        {
            int Horizontal = Pos % GameSizeX;
            int Vertical = Pos / GameSizeX; // Ни в коем случае не GameSizeY!!! строчка (линия) идет по X, а не по Y!!!

            for (int x = -1 + Horizontal; x <= 1 + Horizontal; x++)
            {
                for (int y = -1 + Vertical; y <= 1 + Vertical; y++)
                {
                    if (x >= 0 && x < GameSizeX && y >= 0 && y < GameSizeY)
                    {
                        if ((x == Horizontal && y == Vertical) || DataArray[ConvertXYToPos(x, y)] == BombCode)
                        {
                            continue;
                        }
                        DataArray[ConvertXYToPos(x, y)] = DataArray[ConvertXYToPos(x, y)] + 1;
                    }
                }
            }
        }
        public void OpenValueAroundButton(int Pos)
        {
            int Horizontal = Pos % GameSizeX;
            int Vertical = Pos / GameSizeX; // Ни в коем случае не GameSizeY!!! строчка (линия) идет по X, а не по Y!!!

            if (DataArray[Pos] == 0)
            {
                for (int x = -1 + Horizontal; x <= 1 + Horizontal; x++)
                {
                    for (int y = -1 + Vertical; y <= 1 + Vertical; y++)
                    {
                        if (x >= 0 && x < GameSizeX && y >= 0 && y < GameSizeY)
                        {
                            foreach (var item in this.Controls)
                            {
                                if (item is Button && ((Button)item).Name == (ConvertXYToPos(x, y)).ToString() && (((Button)item).Text == "" || ((Button)item).Text == "❓"))
                                {
                                    ((Button)item).Text = DataArray[ConvertXYToPos(x, y)].ToString();

                                    OpenValue++;

                                    ChangeColorForButton(((Button)item), x, y);

                                    if (OpenValue == GridSize - BombCount)
                                    {
                                        GameStatus(1);
                                    }
                                    // Типа логика должна быть как у рекурсии... но типа и так работает, но лучше исправить
                                    OpenValueAroundButton(ConvertXYToPos(x, y));
                                }
                            }
                        }
                    }
                }
            }
            if (DataArray[Pos] > 0)
            {
                foreach (var item in this.Controls)
                {
                    if (item is Button && ((Button)item).Name == (Pos).ToString() && ((Button)item).Text == "")
                    {
                        ((Button)item).Text = DataArray[Pos].ToString();
                        OpenValue++;

                        ChangeColorForButton(((Button)item), ConvertPosToX(Pos), ConvertPosToY(Pos));

                        if (OpenValue == GridSize - BombCount)
                        {
                            GameStatus(1);
                        }
                    }
                }
            }
            if (DataArray[Pos] == BombCode)
            {
                foreach (var item in this.Controls)
                {
                    if (item is Button)
                    {

                        if (DataArray[Convert.ToInt32(((Button)item).Name)] == BombCode)
                        {
                            ((Button)item).Text = "💣";
                            ((Button)item).ForeColor = ColorBomb;
                            ((Button)item).BackColor = BackColorBomb;
                        }
                    }
                }

                GameStatus(2);
            }
        }
        public void GameStatus(int status)
        {
            foreach (var item in this.Controls)
                if (item is Button)
                    ((Button)item).Enabled = false;

            Label label = new Label()
            {
                Font = new Font("Microsoft Sans Serif", 21, FontStyle.Bold),
                Width = this.Width - 10,
                Height = 40,
                Top = 5,
                Visible = true,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Parent = this
            };
            label.BringToFront();

            Button RetryButton = new Button()
            {
                Left = 10,
                Top = 45,
                Width = 100,
                Height = 40,
                Font = new Font("Microsoft Sans Serif", 21, FontStyle.Bold),
                Text = "Retry",
                Name = "RetryButton",
                TextAlign = ContentAlignment.MiddleCenter,
                TabStop = false
            };
            RetryButton.MouseDown += new MouseEventHandler(FinalButton);


            Button MainMenuButton = new Button()
            {
                Left = this.Width - 100 - 25,
                Top = 45,
                Width = 100,
                Height = 40,
                Font = new Font("Microsoft Sans Serif", 21, FontStyle.Bold),
                Text = "Exit",
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "ExitButton",
                TabStop = false
            };
            MainMenuButton.MouseDown += new MouseEventHandler(FinalButton);

            switch (status)
            {
                case 1:
                    label.ForeColor = Color.DarkGreen;
                    label.Text = "Win";
                    break;
                case 2:
                    label.ForeColor = Color.DarkRed;
                    label.Text = "Lose";
                    break;
            }
            this.Controls.Add(label);
            this.Controls.Add(RetryButton);
            this.Controls.Add(MainMenuButton);
        }
        public void FinalButton(object sender, MouseEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "RetryButton":
                    var StartForm = new StartForm();
                    StartForm.Closed += (s, args) => this.Close();
                    StartForm.Show();
                    this.Hide();
                    break;
                case "ExitButton":
                    Environment.Exit(0);
                    break;
            }
        }
        public void ButtonClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OpenValueAroundButton(Convert.ToInt32(((Button)sender).Name));
            }
            else if (e.Button == MouseButtons.Right)
            {
                if ((((Button)sender).Text) == "❓")
                    ((Button)sender).Text = "";
                else
                    ((Button)sender).Text = "❓";

                ChangeColorForButton(((Button)sender), ConvertPosToX(Convert.ToInt32(((Button)sender).Name)), ConvertPosToY(Convert.ToInt32(((Button)sender).Name)));
            }
        }
        public int ConvertPosToX(int Pos)
        {
            return (Pos % GameSizeX);
        }
        public int ConvertPosToY(int Pos)
        {
            return (Pos / GameSizeX);
        }
        public int ConvertXYToPos(int X, int Y)
        {
            return (Y * GameSizeX + X);
        }
        public void ChangeColorForButton(Button button, int x, int y)
        {
            switch (button.Text)
            {
                case "0":
                    button.Text = " ";
                    break;
                case "1":
                    button.ForeColor = Value1;
                    break;
                case "2":
                    button.ForeColor = Value2;
                    break;
                case "3":
                    button.ForeColor = Value3;
                    break;
                case "4":
                    button.ForeColor = Value4;
                    break;
                case "5":
                    button.ForeColor = Value5;
                    break;
                case "6":
                    button.ForeColor = Value6;
                    break;
                case "7":
                    button.ForeColor = Value7;
                    break;
                case "8":
                    button.ForeColor = Value8;
                    break;
                case "❓":
                    button.ForeColor = ColorQuestion;
                    button.BackColor = BackColorQuestion;
                    return;
                case "":
                    button.BackColor = BackColorQuestion;

                    if (GameSizeX % 2 == 0)
                        if ((ConvertXYToPos(x, y)) % 2 == 0)
                            if (y % 2 != 0)
                                button.BackColor = ColorDarkGreen;
                            else
                                button.BackColor = ColorLightGreen;
                        else
                            if (y % 2 != 0)
                                button.BackColor = ColorLightGreen;
                            else
                                button.BackColor = ColorDarkGreen;
                    else
                        if ((ConvertXYToPos(x, y)) % 2 == 0)
                            button.BackColor = ColorLightGreen;
                        else
                            button.BackColor = ColorDarkGreen;
                    return;
            }

            if (GameSizeX % 2 == 0)
                if ((ConvertXYToPos(x, y)) % 2 == 0)
                    if (y % 2 != 0)
                        button.BackColor = ColorLightBrown;
                    else
                        button.BackColor = ColorDarkBrown;
                else
                    if (y % 2 != 0)
                        button.BackColor = ColorDarkBrown;
                    else
                        button.BackColor = ColorLightBrown;
            else
                if ((ConvertXYToPos(x, y)) % 2 == 0)
                    button.BackColor = ColorDarkBrown;
                else
                    button.BackColor = ColorLightBrown;
        }
    }
}
