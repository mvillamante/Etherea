using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    public class NameInput
        {
            public TextBox getName;
            public Label userName;
            internal PictureBox pBoxBbtn;

            // Properties to store values
            // MainMenu Class will call get the value of these variables
            public string[] ValName { get; set; } = { "Btn_Start" };
            public string[] ResName { get; set; } = { "Start_Button" };
            public Size[] SizeVal { get; set; } = { new Size(169, 76) };
            public Point[] LocVal { get; set; } = { new Point(216, 224) };

            public NameInput()
            {
            }

            public TextBox UserInput()
            {

                // Create the TextBox and set the font
                getName = new TextBox
                {
                    Location = new Point(151, 176),
                    Size = new Size(288, 34),
                    Font = new Font("NotJamOldStyle11", 12, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Center,
                };
                return getName;
            }

            public Label UserName()
            {
                userName = new Label
                {
                Location = new Point(151, 150),
                Text = "Enter your name:",
                Size = new Size(288, 34),
                Font = new Font("NotJamOldStyle11", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
                };
                return userName;
            }
            internal void InitializeBackButton()
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
            }

            private void BackLBoards_Click(object sender, EventArgs e)
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
            }
        }
    }


