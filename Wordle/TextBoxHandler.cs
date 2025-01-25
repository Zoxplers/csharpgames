using System;
using System.Windows.Forms;
using System.Drawing;

namespace Wordle
{
    internal class TextBoxHandler : WinFormHandler
    {
        TextBox textBox;
        int row, col;

        public TextBoxHandler(int row, int col)
        {
            textBox = new TextBox();
            textBox.AutoSize = false;
            textBox.Enabled = false;
            textBox.MaxLength = 1;
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.TextChanged += UpperCase;
            this.row = row;
            this.col = col;
        }

        public TextBox TextBox { get => textBox; }
        public int Row { get => row; }
        public int Col { get => col; }

        /// <summary>
        /// Attempts to capitalize all textbox input.
        /// </summary>
        public void UpperCase(object sender, EventArgs e)
        {
            if(textBox.Text != "" && textBox.Text[0] >= 'a' && textBox.Text[0] <= 'z')
            {
                textBox.Text = ((char)('A' - 'a' + textBox.Text[0])).ToString();
            }
        }

        /// <summary>
        /// Fixes the size, position, and font of textbox based on the amount of guess and the size of the word.
        /// </summary>
        public void Update(int wordSize, int guesses)
        {
            textBox.Size = new Size((textBox.Parent.Width-14)/wordSize, (int)((textBox.Parent.Height-37)*0.8/guesses));
            textBox.Location = new Point(row * textBox.Size.Width, col * textBox.Size.Height);
            textBox.Font = new Font("Arial", (float)(textBox.Height * 10 / 22), FontStyle.Regular);
        }

        public override void Update()
        {
            throw new Exception("Use Update(int wordSize, int guesses) instead.");
        }
    }
}