using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings();

          
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            //Setting a default
            new Settings();

            //Crea un nuovo snake
            Snake.Clear();
            Circle head = new Circle {X = 10, Y = 5};
            Snake.Add(head);


            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        //Cibo random
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle {X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos)};
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            //Controlla se è game over
            if (Settings.GameOver)
            {
                //Controlla se un tasto è premuto
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //Set colour of snake

                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //Draw head
                    
                    else
                        snakeColour = Brushes.Green;
                    //disegna snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                    //disegna cibo
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else
            {
                string gameOver = "Game over \nPunteggio finale: " + Settings.Score ;
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    //prendi la massima x e la massima y
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //collisione con i bordi.
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    //collisione con il corpo
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //collisione con il cibo (mangia)
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //muovi il corpo
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //aggiungi il cerchio al corpo
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //aggiorna punti
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Snake v1.0","Mirko Bastianini");
        }

        private void esciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
