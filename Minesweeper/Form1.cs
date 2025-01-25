using System;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        //Variables
        private Game game;
        private int wins, losses, games, totalTime;

        //Constructor
        public Form1()
        {
            wins = 0;
            losses = 0;
            games = 0;
            totalTime = 0;
            InitializeComponent();
        }

        internal Game Game { get => game; }
        public int Wins { get => wins; set => wins = value; }
        public int Losses { get => losses; set => losses = value; }
        public int Games { get => games; set => games = value; }
        public int TotalTime { get => totalTime; set => totalTime = value; }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Game(this.splitContainer1);
            game.Start();
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        private void restartGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(Control child in splitContainer1.Panel1.Controls)
            {
                child.Dispose();
            }
            foreach (Control child in splitContainer1.Panel2.Controls)
            {
                child.Dispose();
            }
            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel2.Controls.Clear();
            game = new Game(this.splitContainer1);
            game.Start();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //Menu Strip Functions
        private void showStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Win/Loss: {wins}/{losses}\nWin/Loss Ratio: {wins*1f/losses}\nAverage time per game: {totalTime*.1/games} seconds", "Stats");
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can start by opening any random block.(Left Click)\nThe numbers show how many mines are adjactent to the block.\nPut a flag on a block where you have confirmed that there is a mine.(Right Click)\nThe game completes when you have opened all the safe blocks.","Instructions");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Zoxplers\n4/16/2022\nCS 3020","About");
        }
    }
}
