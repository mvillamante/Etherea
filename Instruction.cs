using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class Instruction: Form
    {
        public string[] ControlName { get; set; } = { "MRight", "MLeft", "AttackEnemy"};
        public string[] ControlDesc { get; set; } = { "Move Right", "Move Left", "Attack Enemy" };
        public Size[] ControlSize { get; set; } = { new Size(300, 50), new Size(300, 50), new Size(300, 50) };
        public Point[] ControlLoc { get; set; } = { new Point(280, 165), new Point(280, 265), new Point(280, 350) };
        public string[] ValName { get; set; } = { "DKeyPBox", "AKeyPBox", "LArrowKeyPBox", "RArrowKeyPBox", "SpaceKeyPBox" };
        public string[] ResName { get; set; } = { "dKey", "aKey", "leftKey", "rightKey", "spaceKey" };
        public Size[] SizeVal { get; set; } = { new Size(93, 93), new Size(100, 100), new Size(90, 93), new Size(90, 92), new Size(175, 60) };
        public Point[] LocVal { get; set; } = { new Point(83, 125), new Point(80, 225), new Point(180, 225), new Point(180, 125), new Point(83, 330) };


        internal PictureBox BackBtn;
        internal Label controlLabel;


        public Instruction()
        {
            ControlLabel();
            InitializeBackButton();
        }

        internal void ControlLabel()
        {
            controlLabel = new Label()
            {
                Text = "Controls",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("NotJamOldStyle11", 34, FontStyle.Bold),
                Size = new Size(530, 75),
                Location = new Point(174, 50)
            };
            this.Controls.Add(controlLabel);
        }

        internal void InitializeBackButton()
        {
            BackBtn = new PictureBox()
            {
                Location = new Point(25, 20),
                Size = new Size(40, 40),
                Image = Resources.backbtn, // lalagay mo filename nung image ni back button
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage, // set ng size mode if need
            };

            BackBtn.Click += BackButton_Click;
            this.Controls.Add(BackBtn);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            this.Hide();
            mainMenu.Show();
        }


        

    }
}