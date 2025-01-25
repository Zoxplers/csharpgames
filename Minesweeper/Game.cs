using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Minesweeper
{
    internal class Game
    {
        //Constants
        private const int ROWS = 10, COLS = 10;

        //Variables
        private Random rnd;
        private List<ButtonHandler[]> buttonRows;
        private TimerLabelHandler timer;
        int bombs;

        //Constructor
        public Game(SplitContainer container)
        {
            rnd = new Random();
            buttonRows = new List<ButtonHandler[]>();
            timer = new TimerLabelHandler();
            bombs = 10;
            for (int i = 0; i < ROWS; i++)
            {
                ButtonHandler[] buttonRow = new ButtonHandler[COLS];
                for (int j = 0; j < COLS; j++)
                {
                    ButtonHandler button = new ButtonHandler(i, j);
                    buttonRow[j] = button;
                    container.Panel1.Controls.Add(button.Button);
                    container.Panel1.SizeChanged += button.Update;
                    button.Update(this, EventArgs.Empty);
                }
                buttonRows.Add(buttonRow);
            }
            container.Panel2.Controls.Add(timer.Label);
            container.Panel2.SizeChanged += timer.Update;
            timer.Update(this, EventArgs.Empty);
        }

        public static int GETROWS => ROWS;
        public static int GETCOLS => COLS;
        internal TimerLabelHandler Timer { get => timer;}

        /// <summary>
        /// Starts a game
        /// </summary>
        public void Start()
        {
            //Setup Bombs
            while (bombs > 0)
            {
                foreach (ButtonHandler[] col in buttonRows)
                {
                    foreach (ButtonHandler button in col)
                    {
                        //super efficient 100/100 woohoo
                        if (rnd.Next(20) == 0 && bombs > 0 && button.Data !=  -1)
                        {
                            button.Data = -1;
                            bombs--;
                            SetupAround(button.Row, button.Col);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lose function
        /// </summary>
        internal void ClickBomb()
        {
            timer.End(false);
            foreach (ButtonHandler[] row in buttonRows)
            {
                foreach(ButtonHandler button in row)
                {
                    if(button.Data == -1)
                    {
                        button.Button_LeftClick(this, EventArgs.Empty);
                    }
                    button.Button.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Check if player won
        /// </summary>
        internal void CheckWin()
        {
            bool win = true;
            foreach (ButtonHandler[] row in buttonRows)
            {
                foreach (ButtonHandler button in row)
                {
                    if((button.Button.Enabled && button.Data != -1) || (!button.Button.Enabled && button.Data == -1))
                    { 
                        win = false;
                    }
                }
            }
            if(win)
            {
                foreach (ButtonHandler[] row in buttonRows)
                {
                    foreach (ButtonHandler button in row)
                    {
                        if (button.Button.Enabled)
                        {
                            button.Button.Text = ButtonHandler.GETFLAG.ToString();
                        }
                    }
                }
                timer.End(true);
            }
        }

        /// <summary>
        /// Increments blocks around bombs
        /// </summary>
        internal void SetupAround(int row, int col)
        {
            if (row == ROWS - 1)
            {
                if (col == COLS - 1)
                {
                    buttonRows[row-1][col].Increment();
                    buttonRows[row-1][col-1].Increment();
                    buttonRows[row][col-1].Increment();
                }
                else if (col > 0)
                {
                    buttonRows[row-1][col].Increment();
                    buttonRows[row-1][col-1].Increment();
                    buttonRows[row-1][col+1].Increment();
                    buttonRows[row][col-1].Increment();
                    buttonRows[row][col+1].Increment();
                }
                else
                {
                    buttonRows[row][col+1].Increment();
                    buttonRows[row-1][col+1].Increment();
                    buttonRows[row-1][col].Increment();
                }
            }
            else if (row > 0)
            {
                if (col == COLS - 1)
                {
                    buttonRows[row-1][col-1].Increment();
                    buttonRows[row][col-1].Increment();
                    buttonRows[row+1][col-1].Increment();
                    buttonRows[row-1][col].Increment();
                    buttonRows[row+1][col].Increment();
                }
                else if (col > 0)
                {
                    buttonRows[row-1][col-1].Increment();
                    buttonRows[row][col-1].Increment();
                    buttonRows[row+1][col-1].Increment();
                    buttonRows[row-1][col].Increment();
                    buttonRows[row+1][col].Increment();
                    buttonRows[row-1][col+1].Increment();
                    buttonRows[row][col+1].Increment();
                    buttonRows[row+1][col+1].Increment();
                }
                else
                {
                    buttonRows[row-1][col+1].Increment();
                    buttonRows[row][col+1].Increment();
                    buttonRows[row+1][col+1].Increment();
                    buttonRows[row-1][col].Increment();
                    buttonRows[row+1][col].Increment();
                }
            }
            else
            {
                if (col == COLS - 1)
                {
                    buttonRows[row][col-1].Increment();
                    buttonRows[row+1][col-1].Increment();
                    buttonRows[row+1][col].Increment();
                }
                else if (col > 0)
                {
                    buttonRows[row][col-1].Increment();
                    buttonRows[row+1][col-1].Increment();
                    buttonRows[row+1][col].Increment();
                    buttonRows[row][col+1].Increment();
                    buttonRows[row+1][col+1].Increment();
                }
                else
                {
                    buttonRows[row][col+1].Increment();
                    buttonRows[row+1][col+1].Increment();
                    buttonRows[row+1][col].Increment();
                }
            }
        }

        /// <summary>
        /// Click the top, bottom, and sides of button.
        /// </summary>
        internal void ClickAround(int row, int col)
        {
            if(row > 0)
            {
                buttonRows[row - 1][col].Button_LeftClick(this, EventArgs.Empty);
                if(col > 0)
                {
                    buttonRows[row - 1][col - 1].Button_LeftClick(this, EventArgs.Empty);
                }
                if(col != COLS - 1)
                {
                    buttonRows[row - 1][col + 1].Button_LeftClick(this, EventArgs.Empty);
                }
            }
            if (col > 0)
            {
                buttonRows[row][col - 1].Button_LeftClick(this, EventArgs.Empty);
            }
            if (row != ROWS - 1)
            {
                buttonRows[row + 1][col].Button_LeftClick(this, EventArgs.Empty);
                if(col > 0)
                {
                    buttonRows[row + 1][col - 1].Button_LeftClick(this, EventArgs.Empty);
                }
                if(col != COLS -1)
                {
                    buttonRows[row + 1][col + 1].Button_LeftClick(this, EventArgs.Empty);
                }
            }
            if (col != COLS - 1)
            {
                buttonRows[row][col + 1].Button_LeftClick(this, EventArgs.Empty);
            }
        }
    }
}
