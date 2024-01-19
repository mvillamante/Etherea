using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class GameOver : Form
    {
        internal Label gameOverLabel = new Label();
        private Button playAgainBtn = new Button();
        private Button lBoardsBtn = new Button();
        private Button mainMenuBtn = new Button();
        public GameOver() 
        {
            FormLayout();
            GameOverLabel();
            PlayAgainBtn();
            Leaderboards();
            MainMenu();
        }

        internal void GameOverLabel()
        {
            gameOverLabel = new Label
            {
                Text = "Game Over!",
                Font = new Font("The Wild Breath of Zelda", 70),
                Location = new Point(105, 150),
                AutoSize = true,
                BackColor = Color.Black,
                ForeColor = Color.White
            };
            this.Controls.Add(gameOverLabel);
        }

        private void PlayAgainBtn()
        {
            playAgainBtn = new Button
            {
                Text = "Play Again",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("The Wild Breath of Zelda", 20),
                Location = new Point(240, 265),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            playAgainBtn.Click += PlayAgainBtn_Click;
            this.Controls.Add(playAgainBtn);
        }

        private void MainMenu()
        {
            mainMenuBtn = new Button
            {
                Text = "Main Menu",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("The Wild Breath of Zelda", 20),
                Location = new Point(243, 360),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            mainMenuBtn.Click += MainMenu_Click;
            this.Controls.Add(mainMenuBtn);
        }

        private void Leaderboards()
        {
            lBoardsBtn = new Button
            {
                Text = "Leaderboards",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("The Wild Breath of Zelda", 20),
                Location = new Point(229, 315),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            lBoardsBtn.Click += LBoards_Click;
            this.Controls.Add(lBoardsBtn);
        }

        private void PlayAgainBtn_Click(object sender, EventArgs e)
        {
            StoryLine storyLine = new StoryLine();
            this.Close();
            storyLine.Show();
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to go back to main menu?", "Go Back to Main Menu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                mainMenu.Show();
                 
                // Play Music
                MusicPlayer mplayer = new MusicPlayer();
                mplayer.PlayTitleMusic();
            }

        }
        private void LBoards_Click(object sender, EventArgs e)
        {
            LeaderBoards lboards = new LeaderBoards();
            this.Close();
            lboards.Show();

            // Play Music
            MusicPlayer mplayer = new MusicPlayer();
            mplayer.PlayTitleMusic();
        }

        private void FormLayout()
        { //Changes the Layout of the Current Form

            //Remove Minimize and Maximize Button
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //Resize Current Form
            Size = new Size(602, 488);

            this.FormBorderStyle = FormBorderStyle.Fixed3D;

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.DoubleBuffered = true;

            this.Icon = Resources.etherea_icon;
        }
    }
}

