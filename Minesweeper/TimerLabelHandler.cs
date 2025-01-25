using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class TimerLabelHandler : WinFormHandler
    {
        //Variables
        private Label label;
        private Timer timer;
        private int time;
        private bool hasStarted, hasEnded;

        //Constructor
        public TimerLabelHandler()
        {
            label = new Label();
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new Font(label.Font.Name, 12);
            label.AutoSize = true;
            label.Text = "0s";
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += TimerTick;
            hasStarted = false;
            hasEnded = false;
        }

        public Label Label { get => label; }
        public bool HasStarted { get => hasStarted; set => hasStarted = value; }

        /// <summary>
        /// Resizes and repositions label based on label parent's ClientRectangle
        /// </summary>
        internal override void Update(object sender, EventArgs e)
        {
            label.Location = new Point((label.Parent.Width/2)-label.Width/2, (label.Parent.Height/2)-label.Height/2);
        }

        /// <summary>
        /// Increments time and sets label
        /// </summary>
        internal void TimerTick(object sender, EventArgs e)
        {
            time++;
            label.Text = (time/10f).ToString(".00s");
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        internal void Start()
        {
            hasStarted = true;
            timer.Start();
        }

        /// <summary>
        /// Stops timer and ends the game
        /// </summary>
        internal void End(bool win)
        {
            if (!hasEnded)
            {
                hasEnded = true;
                Form1 form = (Form1)label.FindForm();
                form.Games++;
                form.TotalTime += time;
                if (win)
                {
                    form.Wins++;
                    label.Text = "You Win, ";
                }
                else
                {
                    form.Losses++;
                    label.Text = "You Lose, ";
                }
                timer.Stop();
                label.Text += "Time: " + (time / 10f).ToString(".00s");
                Update(this, EventArgs.Empty);
            }
        }
    }
}
