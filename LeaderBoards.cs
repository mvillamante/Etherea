using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class LeaderBoards : Form
    {
        private Label topScorers;
        private PictureBox pBoxBbtn;
        public Label lboardText = new Label();
        private PictureBox mia;
        private readonly Image miaImage = Resources.mia_rightidle;


        internal List<PlayerInfo> leaderboardEntry = new List<PlayerInfo>();

        public LeaderBoards()
        {
            FormLayout();
            InitializeBackButton();
            LboardsComponents();

            UpdateAndDisplayLeaderboard();
            DisplayLeaderboard(topScorers);
        }


        public class PlayerInfo
        {
            public string PlayerName { get; set; }
            public string Score { get; set; }
        }

        private void ReadTextFile()
        {
            string[] playerInfoArr;
            string filePath = "PlayerInfo.txt";

            StreamReader openTxtFile = new StreamReader(filePath);
            {
                while (!openTxtFile.EndOfStream)
                {
                    string line = openTxtFile.ReadLine();
                    playerInfoArr = line.Split('|');

                    if (playerInfoArr.Length >= 2)
                    {
                        // Create a new PlayerInfo instance and add it to the leaderboardEntry list
                        PlayerInfo playerInfo = new PlayerInfo
                        {
                            PlayerName = playerInfoArr[0],
                            Score = playerInfoArr[1]
                        };

                        leaderboardEntry.Add(playerInfo);
                    }
                }
            }
        }


        // Method to update and display the leaderboard
        private void UpdateAndDisplayLeaderboard()
        {
            // Add the current player's score to the leaderboard
            ReadTextFile();


            // Sort the leaderboard by score in descending order
            leaderboardEntry = leaderboardEntry.OrderBy(entry => entry.Score).ToList();

            // Display the top 3 entries
        }

        // Display the top 3 entries in the ListBox
        private void DisplayLeaderboard(Label topScorers)
        {
            string topScorerLabel = "";

            for (int i = 0; i < Math.Min(3, leaderboardEntry.Count); i++)
            {
                topScorerLabel += ($"{i + 1}. {leaderboardEntry[i].PlayerName} - {leaderboardEntry[i].Score}\n\n");
            }
            topScorers.Text = topScorerLabel;


            // Picturebox for Leaderboards Back Button
        }


        private void InitializeBackButton()
        {
            pBoxBbtn = new PictureBox()
            {
                Location = new Point(25, 20),
                Size = new Size(40, 40),
                Image = Resources.backbtn, // lalagay mo filename nung image ni back button
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage, // set ng size mode if need
            };

            pBoxBbtn.Click += BackLBoards_Click;
            this.Controls.Add(pBoxBbtn);
        }

        private void BackLBoards_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            this.Close();
            mainMenu.Show();
        }

        private void LboardsComponents()
        {
            lboardText = new Label
            {
                Text = "Leaderboards",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("NotJamOldStyle11", 25, FontStyle.Bold),
                Size = new Size(400, 70),
                Location = new Point(150, 70),
            };
            this.Controls.Add(lboardText);

            topScorers = new Label
            {
                Location = new Point(-50, -15),
                Font = new Font("NotJamOldStyle11", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(700, 500),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
            };

            this.Controls.Add(topScorers);
        }

        private void FormLayout()
        {
            //Remove Minimize and Maximize Button
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //Resize Current Form
            Size = new Size(602, 488);

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            //this.Text = "ETHEREA: Echoes From The Whispering Forest";

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Leaderboards BG
            this.BackColor = Color.Black;
            this.BackgroundImage = Properties.Resources.lboards_bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Icon = Resources.etherea_icon;
        }
    }
}