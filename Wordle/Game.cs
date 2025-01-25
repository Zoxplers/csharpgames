using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace Wordle
{
    class Game
    {
        //Constants
        private const string DATASET = "wordle-answers-alphabetical.txt";
        private const string DATASETBACKUP = "IHTCG.txt";

        //Variables
        Random random;
        List<TextBoxHandler[]> textBoxRows;
        GuessButtonHandler guessButton;
        int currentRow;
        string[] data;
        string word;
        string guessString;

        /// <summary>
        /// Creates a game of WORDLE using textboxes and a guess button.
        /// </summary>
        public Game(List<TextBoxHandler[]> textBoxRows, GuessButtonHandler guessButton)
        {
            random = new Random();
            this.textBoxRows = textBoxRows;
            this.guessButton = guessButton;
            guessButton.Button.Click += Guess;
            currentRow = 0;
            try
            {
                data = LoadData(DATASET);
            }
            catch
            {
                try
                {
                    data = LoadData(DATASETBACKUP);
                }
                catch
                {
                    MessageBox.Show("Unable to find dataset");
                }
            }
            word = data[random.Next(data.Length)];
        }

        /// <summary>
        /// Taken from System.IO.File.ReadAllLines, edited to automatically convert to uppercase.
        /// </summary>
        private string[] LoadData(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line.ToUpper());
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Enables the text boxes in current row.
        /// </summary>
        public void EnableCurrentRow()
        {
            foreach (TextBoxHandler textBox in textBoxRows[currentRow])
            {
                textBox.TextBox.Enabled = true;
                textBox.TextBox.TextChanged += CheckRow;
            }
            CheckRow(null, EventArgs.Empty);
        }

        /// <summary>
        /// Guess button is clicked
        /// </summary>
        private void Guess(object sender, EventArgs e)
        {
            //Variables
            bool win = true;

            //Get list for yellow boxes
            List<char> charList = new List<char>();
            foreach(char i in word)
            {
                charList.Add(i);
            }

            //Green Boxes
            foreach (TextBoxHandler textBox in textBoxRows[currentRow])
            {
                if (textBox.TextBox.Text[0] == word[textBox.Row])
                {
                    textBox.TextBox.BackColor = Color.DarkSeaGreen;
                    charList.Remove(textBox.TextBox.Text[0]);
                }
                else
                {
                    win = false;
                }
            }

            //Yellow Boxes
            //Need to separate green and yellow into different loops, because entire row needs to be checked for green first.
            //Otherwise, charList would not be accurate for yellow
            foreach (TextBoxHandler textBox in textBoxRows[currentRow])
            {
                if (charList.Contains(textBox.TextBox.Text[0]))
                {

                    textBox.TextBox.BackColor = Color.Goldenrod;
                    charList.Remove(textBox.TextBox.Text[0]);
                }
            }

            //Win
            foreach (TextBoxHandler textBox in textBoxRows[currentRow])
            {
                textBox.TextBox.Enabled = false;
                textBox.TextBox.TextChanged -= CheckRow;
            }
            if (win)
            {
                guessButton.Button.Enabled = false;
                guessButton.Button.Text = "You Win!";
            }
            //Continues the game
            else
            {
                currentRow++;
                //Lose
                if (currentRow >= textBoxRows.Count)
                {
                    guessButton.Button.Enabled = false;
                    guessButton.Button.Text = "You Lose! The word was " + word;
                }
                else
                {
                    EnableCurrentRow();
                }
            }
        }

        /// <summary>
        /// If the current row is a valid word, enable guess button.
        /// </summary>
        private void CheckRow(object sender, EventArgs e)
        {
            int col = 0;
            bool guessState = true;
            char[] guess = new char[5];

            //Check all textboxes in current row if empty while initializing guessString variable
            foreach (TextBoxHandler textBox in textBoxRows[currentRow])
            {
                if (textBox.TextBox.Text == "")
                {
                    guessState = false;     
                }
                else
                {
                    guess[col] = textBox.TextBox.Text[0];
                }
                col++;
            }

            //if textboxes are not empty, find guessString in dataset
            guessString = new string(guess);
            if (guessState)
            {
                guessState = false;
                foreach(string s in data)
                {
                    if (s.Equals(guessString))
                    {
                        guessState = true;
                    }
                }
            }
            guessButton.Button.Enabled = guessState;
        }
    }
}