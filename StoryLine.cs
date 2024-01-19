using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class StoryLine : Form
    {
        private MusicPlayer mplayer;

        private Button skipButton;
        private Label textLabel;
        private readonly string text;
        private int len = 0;
        private readonly Timer textTimer;

        public StoryLine()
        {
            FormLayout(); // Changes the Layout of Form
            CreateSkip(); // Creates a Skip Button
            CreateTLabel(); // Creates a Text Label

            textTimer = new Timer
            {
                Interval = 100
            };
            textTimer.Tick += TextTimer_Tick;
            textTimer.Start();

            text = "In the Enchanting Whispering Forest, Mia, an ordinary girl, stumbles upon an extraordinary destiny. " +
                "The forest, once alive with vibrant magic, now slumbers under a malevolent curse. " +
                "Mia's journey unfolds as she heeds ancestral dreams, embarking on a quest to revive the forest's vitality. " +
                "Through stages of mystical beauty and perilous challenges, she faces darkness and rekindles the Whispering Forest's enchantment. " +
                "Mia's awakening as its guardian is a tale about to be written...";

        }

        private void CreateSkip ()
        {
            //skip button layout
            skipButton = new Button
            {
                Text = "Skip >",
                Location = new Point(466, 360),
                Size = new Size(100, 60),
                Font = new Font("NotJamOldStyle11", 15, FontStyle.Bold),

                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                BackColor = Color.Transparent

            };
            skipButton.FlatAppearance.BorderSize = 0;
            skipButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            skipButton.FlatAppearance.MouseDownBackColor = Color.Transparent;

            skipButton.Click += SkipButton_Click;

            this.Controls.Add(skipButton);
        }

        private void CreateTLabel()
        {
            //set text
            textLabel = new Label
            {
                AutoSize = true,
                Location = new Point(35, 50),
                Size = new Size(570, 470),
                Font = new Font("NotJamOldStyle11", 15, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                MaximumSize = new Size(555, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = " "
            };

            this.Controls.Add(textLabel);
        }

        private void TextTimer_Tick(object sender, EventArgs e)
        {
            if (len < text.Length)
            {
                textLabel.Text += text[len];
                len++;
            }
            else
            {
                textTimer.Stop();
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            mplayer = new MusicPlayer();
            mplayer.StopTitleMusic();

            StagePlay stagePlay = new StagePlay();
            this.Close();
            stagePlay.Show();
        }
        private void FormLayout()
        { //Changes the Layout of the Current Form

            //Remove Minimize and Maximize Button
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //Resize Current Form
            Size = new Size(602, 488);

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            //this.Text = "ETHEREA: Echoes From The Whispering Forest";

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = Resources.StoryLine_BG;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.DoubleBuffered = true;

            this.Icon = Resources.etherea_icon;
        }
    }
}
