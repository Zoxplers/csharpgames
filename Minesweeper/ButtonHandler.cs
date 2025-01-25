using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class ButtonHandler : WinFormHandler
    {
        //Constants
        private const char FLAG = 'F', BOMB = 'B';

        //Variables
        private Button button;
        private int row, col, data;
        private Color[] foreColors = { Color.Blue, Color.Green, Color.Red, Color.DarkBlue, Color.DarkRed, Color.Cyan, Color.Black, Color.Gray };

        //Constructor
        public ButtonHandler(int row, int col)
        {
            button = new Button();
            button.TabStop = false;
            button.MouseUp += Button_Click;
            this.row = row;
            this.col = col;
            data = 0;
        }

        public Button Button { get => button; }
        public int Row { get => row; }
        public int Col { get => col; }
        public int Data { get => data; set => data = value; }
        public static char GETFLAG => FLAG;

        /// <summary>
        /// Resizes and repositions button based on button parent's ClientRectangle
        /// </summary>
        internal override void Update(object sender, EventArgs e)
        {
            button.Size = new Size((button.Parent.ClientRectangle.Width / Game.GETCOLS), (button.Parent.ClientRectangle.Height / Game.GETROWS));
            button.Location = new Point(row * button.Size.Width, col * button.Size.Height);
        }

        /// <summary>
        /// Separates left click from right click.
        /// </summary>
        internal void Button_Click(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Button_LeftClick(sender, e);
            }
            else
            {
                button.Text = button.Text== FLAG.ToString()? "": FLAG.ToString();
            }
        }

        /// <summary>
        /// Checks if bomb, otherwise checks for bombs around
        /// </summary>
        internal void Button_LeftClick(object sender, EventArgs e)
        {
            if (button.Enabled && button.Text != FLAG.ToString())
            {
                Form1 form = (Form1)button.FindForm();
                button.Enabled = false;
                if(!form.Game.Timer.HasStarted)
                {
                    form.Game.Timer.Start();
                }
                if (data == -1)
                {
                    button.Text = BOMB.ToString();
                    form.Game.ClickBomb();
                }
                else if (data == 0)
                {
                    form.Game.ClickAround(row, col);
                }
                else
                {
                    button.Text = data.ToString();
                }
                form.Game.CheckWin();
            }
        }

        /// <summary>
        /// Increments data if data is not -1
        /// </summary>
        internal void Increment()
        {
            if(data != -1)
            {
                data++;
            }
        }
    }
}
