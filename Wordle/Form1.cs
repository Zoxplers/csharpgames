using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Wordle
{
    public partial class Form1 : Form
    {
        //Constants
        private const int GUESSES = 6;
        private const int WORD_SIZE = 5;

        //Variables
        List<TextBoxHandler[]> textBoxRows;
        GuessButtonHandler guessButton;
        Game game;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialize Variables
            textBoxRows = new List<TextBoxHandler[]>();

            //Initialize Text Boxes
            for (int i = 0; i < GUESSES; i++)
            {
                TextBoxHandler[] textBoxRow = new TextBoxHandler[WORD_SIZE];
                for (int j = 0; j < WORD_SIZE; j++)
                {
                    TextBoxHandler textBox = new TextBoxHandler(j,i);
                    textBoxRow[j] = textBox;
                    this.Controls.Add(textBox.TextBox);
                }
                textBoxRows.Add(textBoxRow);
            }

            //Initialize Guess Button
            guessButton = new GuessButtonHandler();
            this.Controls.Add(guessButton.Button);

            Form1_Resize(this, EventArgs.Empty);

            //Run Game
            game = new Game(textBoxRows, guessButton);
            game.EnableCurrentRow();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.Width >= 136 && this.Height >= 60)
            {
                //Resize Text Boxes
                foreach (TextBoxHandler[] i in textBoxRows)
                {
                    foreach (TextBoxHandler j in i)
                    {
                        j.Update(WORD_SIZE, GUESSES);
                    }
                }

                //Resize Guess Button
                guessButton.Update();
            }
        }
    }
}
