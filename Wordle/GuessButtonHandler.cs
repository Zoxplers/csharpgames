using System.Windows.Forms;
using System.Drawing;

namespace Wordle
{
    internal class GuessButtonHandler : WinFormHandler
    {
        Button button;

        public GuessButtonHandler()
        {
            button = new Button();
            button.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular);
            button.Location = new Point(12, 297);
            button.Size = new Size(319, 41);
            button.Text = "Guess";
            button.Enabled = false;
        }

        public Button Button { get => button; }

        /// <summary>
        /// Fixes the size and position of button.
        /// </summary>
        public override void Update()
        {
            button.Size = new Size((int)((button.Parent.Width - 14) * 0.9), (int)((button.Parent.Height - 37) * 0.15));
            button.Location = new Point((int)((button.Parent.Width - 14) * 0.05), (int)((button.Parent.Height - 37) * 0.825));
        }
    }
}