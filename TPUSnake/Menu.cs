using System;
using System.Windows.Forms;

namespace TPUSnake
{
    public partial class Menu : Form
    {
        public string nickname;

        public Menu()
        {
            InitializeComponent();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            Game gameWindow = new Game();
            gameWindow.Show();
        }

        private void quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HelpForm(object sender, EventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.Show();
        }
    }
}
