using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TPUSnake
{
    public partial class Game : Form
    {
        private int rI, rJ, rx, ry, rsx, rsy;
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[900];
        private PictureBox[] wall = new PictureBox[900];
        private int[,] coordinates = new int[900, 900];
        private Label labelScore;
        private Label labelLevel;
        private Label nick;
        private TextBox nickname;
        private string name;
        private Button but;
        private int dirX, dirY;
        private int _width = 800;
        private int _height = 600;
        private int _sizeOfSides = 25;
        private int score = 0;
        private int level = 0;
        private string[] arr = new string[10];
        public Game() 
        {
            InitializeComponent();

            this.Text = "Snake";
            this.Width = 1280;
            this.Height = 720;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackgroundImage = TPUSnake.Properties.Resources.background;

            dirX = 1;
            dirY = 0;

            but = new Button();
            but.Location = new Point(1185, 26);
            but.Size = new Size(30, 30);
            but.BackgroundImage = TPUSnake.Properties.Resources.save;
            but.BackgroundImageLayout = ImageLayout.Stretch;
            but.FlatStyle = FlatStyle.Popup;
            but.BackColor = Color.White;
            but.Click += new EventHandler(but_Click);
            this.Controls.Add(but);
            
            nickname = new TextBox();
            nickname.Location = new Point(1050,25);
            nickname.Font = new Font("Kristen ITC", 16, FontStyle.Regular);
            nickname.Size = new Size(120, 14);
            nickname.ForeColor = Color.Black;
            this.Controls.Add(nickname);

            nick = new Label();
            nick.Text = "Nickname:";
            nick.Location = new Point(900, 25);
            nick.Size = new Size(300, 30);
            nick.Font = new Font("Kristen ITC", 20, FontStyle.Regular);
            nick.BackColor = Color.Transparent;
            this.Controls.Add(nick);

            labelScore = new Label();
            labelScore.Text = "Score: " + score;
            labelScore.Location = new Point(960, 135);
            labelScore.Size = new Size(300, 50);
            labelScore.Font = new Font("Kristen ITC", 30, FontStyle.Regular);
            labelScore.BackColor = Color.Transparent;
            this.Controls.Add(labelScore);

            labelLevel = new Label();
            labelLevel.Text = "Level " + level;
            labelLevel.Location = new Point(1000, 100);
            labelLevel.Size = new Size(300, 30);
            labelLevel.Font = new Font("Kristen ITC", 20, FontStyle.Regular);
            labelLevel.BackColor = Color.Transparent;
            this.Controls.Add(labelLevel);

            Random rand = new Random();
            rsx = rand.Next(75, _height - _sizeOfSides);
            int tempI = rsx % _sizeOfSides;
            rsx -= tempI;
            rsy = rand.Next(75, _height - _sizeOfSides);
            int tempJ = rsy % _sizeOfSides;
            rsy -= tempJ;
            snake[0] = new PictureBox();
            snake[0].Image = TPUSnake.Properties.Resources.head;
            snake[0].Location = new Point(rsx, rsy);
            snake[0].SizeMode = PictureBoxSizeMode.Zoom;
            snake[0].BackColor = Color.Transparent;
            snake[0].Size = new Size(_sizeOfSides-1, _sizeOfSides-1);
            this.Controls.Add(snake[0]);

            fruit = new PictureBox();
            fruit.BackColor = Color.Transparent;
            fruit.Size = new Size(_sizeOfSides, _sizeOfSides);
            fruit.SizeMode = PictureBoxSizeMode.Zoom;
            fruit.Image = TPUSnake.Properties.Resources.banana;

            _generateFruit();

            timer.Tick += new EventHandler(_update);
            timer.Interval = 600;

            this.KeyDown += new KeyEventHandler(OKP);
        }

        private void _restart()
        {
            

            for (int i = 0; i < 900; i++)
            {
                for (int j = 0; j < 900; j++)
                {
                    coordinates[i, j] = 0;
                }
            }

            for (int _j = 0; _j <= level; _j++)
                this.Controls.Remove(wall[_j]);

            _gameover();

            score = 0;
            labelScore.Text = "Score: " + score;
            level = 0;
            labelLevel.Text = "Level " + level;

            dirX = 1;
            dirY = 0;

            Random r = new Random();
            rsx = r.Next(75, _height - _sizeOfSides);
            int tempI = rsx % _sizeOfSides;
            rsx -= tempI;
            rsy = r.Next(75, _height - _sizeOfSides);
            int tempJ = rsy % _sizeOfSides;
            rsy -= tempJ;
            snake[0] = new PictureBox();
            snake[0].Image = TPUSnake.Properties.Resources.head;
            snake[0].Location = new Point(rsx, rsy);
            snake[0].SizeMode = PictureBoxSizeMode.Zoom;
            snake[0].BackColor = Color.Transparent;
            snake[0].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            this.Controls.Add(snake[0]);

            _generateFruit();

            timer.Interval = 600;

            timer.Start();
        }
        private void _level()
        {
            if ((snake[0].Location.X == rI) && (snake[0].Location.Y == rJ)) {
                if ((score % 2 == 0) && (score != 0))
                {
                    _generateWall();

                    labelLevel.Text = "Level " + ++level;
                    if (timer.Interval > 25)
                    {
                        timer.Interval -= 25;
                    }
                }
            }
        }

        private void _eatFruit()
        {
            if(snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                labelScore.Text = "Score: " + ++score;
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 40 * dirX, snake[score - 1].Location.Y - 40 * dirY);
                snake[score].Size = new Size(_sizeOfSides-1, _sizeOfSides-1);
                snake[score].Image = TPUSnake.Properties.Resources.body;
                snake[score].BackColor = Color.Transparent;
                snake[score].SizeMode = PictureBoxSizeMode.Zoom;
                this.Controls.Add(snake[score]);
                _generateFruit();
            }
        }

        private void _generateFruit()
        {
                Random r = new Random();
                rI = r.Next(100, _height - _sizeOfSides);
                int tempI = rI % _sizeOfSides;
                rI -= tempI;
                rJ = r.Next(100, _height - _sizeOfSides);
                int tempJ = rJ % _sizeOfSides;
                rJ -= tempJ;
                fruit.Location = new Point(rI, rJ);
                this.Controls.Add(fruit);
        }

        private void _generateWall()
        {
            Random r = new Random();
            rx = r.Next(100, _height - _sizeOfSides);
            int tempI = rx % _sizeOfSides;
            rx -= tempI;
            ry = r.Next(100, _height - _sizeOfSides);
            int tempJ = ry % _sizeOfSides;
            ry -= tempJ;
            rx += 25;
            ry += 25;

            coordinates[rx,ry] = 1;

            wall[level] = new PictureBox();
            wall[level].Location = new Point(rx, ry);
            wall[level].Image = TPUSnake.Properties.Resources.wall;
            wall[level].SizeMode = PictureBoxSizeMode.Zoom;
            wall[level].BackColor = Color.Transparent;
            wall[level].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            this.Controls.Add(wall[level]);

            while (coordinates[rI, rJ] == 1)
            {
                _generateFruit();
            }
        }

        private void _checkBorders()
        {
            if (snake[0].Location.X < 50)
            {
                _gameover();
            }
            if (snake[0].Location.X > _width+25)
            {
                _gameover();
            }
            if (snake[0].Location.Y < 50)
            {
                _gameover();
            }
            if (snake[0].Location.Y > _height+5)
            {
                _gameover();
            }
        }

        private void _eatItself()
        {
            for (int _i = 1; _i < score; _i++)
            {
                if (snake[0].Location == snake[_i].Location)
                {
                    _gameover();
                }
            }
            if (coordinates[snake[0].Location.X,snake[0].Location.Y] == 1)
            {
                _gameover();
            }
        }

        private void _moveSnake()
        {
            for(int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i-1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * (_sizeOfSides), snake[0].Location.Y + dirY * (_sizeOfSides));
            _eatItself();
        }

        private void _update(Object myObject, EventArgs eventsArgs)
        {
            _level();
            _checkBorders();
            _eatFruit();
            _moveSnake();
        }

        private void OKP (object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Down":
                    dirY = 1;
                    dirX = 0;
                    break;
                case "Space":
                    _restart();
                    break;
                case "Escape":
                    this.Close();
                    break;
                case "F":
                    _generateFruit();
                    break;
            }
        }

        private void _gameover()
        {
            timer.Stop();
            for (int _j = 0; _j <= score; _j++)
                this.Controls.Remove(snake[_j]);
            _scoreboard();
        }

        private void but_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            name = nickname.Text;
            nickname.Enabled = false;
            but.Enabled = false;
            timer.Start();
        }

        private void _scoreboard()
        {
            int count = 0;

            arr = File.ReadAllLines("./bin/Debug/scoreboard.txt");
            int a = 0;
            int c = -1;
            for (int i = 0; i < 10; i++)
            {
                string s = arr[i];
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == ' ')
                    {
                        count = j;
                        break;
                    }
                }

                if (score > int.Parse(s.Substring(0, count)) && a == 0)
                {
                    c = i;
                    a++;
                }
            }

            File.WriteAllText("./bin/Debug/scoreboard.txt", string.Empty);
            StreamWriter f = new StreamWriter("./bin/Debug/scoreboard.txt");

            for (int i = 0; i <= 9; i++)
            {
                if (i < c) f.WriteLine(arr[i]);
                if (i == c)
                {
                    f.WriteLine(score + " " + name);
                }
                if (i > c) f.WriteLine(arr[i - 1]);
            }

            f.Close();
            a = 0;
        }
    }
}
