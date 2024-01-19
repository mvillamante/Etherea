using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.IO;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    public class MainMenu : Form
    {

        private readonly List<Image> bgFrames = new List<Image>();
        int currentFrameIndex = 0;
        Timer animationTimer;

        //For Displaying Array of Images
        private readonly string[] valName;
        private readonly string[] resName;
        private readonly Size[] sizeVal;
        private readonly Point[] locVal;

        public PictureBox[] pBoxArray;

        //For Name Input Screen
        internal TextBox getName;
        internal Label userName;
        internal string playerName;

        public MainMenu()
        {
            valName = new string[] { "TitleName", "Btn_Play", "Btn_Instruction", "Btn_Lboards", "Btn_Exit" };
            resName = new string[] { "title_name", "play_button", "instruction_button", "lboard_button", "exit_button" };
            sizeVal = new Size[] { new Size(435, 218), new Size(100, 40), new Size(145, 35), new Size(170, 24), new Size(47, 25) };
            locVal = new Point[] { new Point(75, 65), new Point(470, 289), new Point(405, 327), new Point(375, 372), new Point(494, 410) };

            //Gif As Background
            ChangeBg();

            FormLayout();
            DisplayImage(valName, resName, sizeVal, locVal);

        }

        // ----------------- CLICK EVENTS (Main Menu) ------------------- //  
        private void PlayButton_Click(object sender, EventArgs e)
        {
            NameInput nameInput = new NameInput();
            getName = nameInput.UserInput();
            userName = nameInput.UserName();

            string[] valName = nameInput.ValName;
            string[] resName = nameInput.ResName;
            Size[] sizeVal = nameInput.SizeVal;
            Point[] locVal = nameInput.LocVal;

            nameInput.InitializeBackButton();
            PictureBox backbtn = nameInput.pBoxBbtn;
            this.Controls.Add(backbtn);

            //Remove PictureBoxes
            foreach (PictureBox removePbox in pBoxArray)
            {
                if (removePbox.Parent != null)
                {
                    removePbox.Parent.Controls.Remove(removePbox);
                    removePbox.Dispose();
                }
            }

            //Display New Pictureboxes for NameInput
            DisplayImage(valName, resName, sizeVal, locVal);
            Controls.Add(getName);
            Controls.Add(userName);

        }
        private void LeaderboardsButton_Click(object sender, EventArgs e)
        {
            LeaderBoards leaderBoards = new LeaderBoards();
            this.Hide();
            leaderBoards.Show();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit Game", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                //Terminate this Program
                Application.Exit();
            }
        }


        // ----------------- CLICK EVENTS (Name Input) ------------------- //
        private void StartButton_Click(object sender, EventArgs e)
        {
            string filePath = "PlayerInfo.txt";
            playerName = getName.Text;
            

            try
            {
                StreamWriter writetoFile = File.AppendText(filePath);
                writetoFile.Write(playerName);
                writetoFile.Close();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error writing to file" + ex.Message);
            }

            StoryLine storyLine = new StoryLine();
            this.Hide();
            storyLine.Show();
        }
        internal string GetPlayerName()
        {
            return playerName;
        }

        // ----------------- CLICK EVENTS (How to Play) ------------------- //
        private void InstructionButton_Click(object sender, EventArgs e)
        {
            //babaguhin pa
            Instruction instruct = new Instruction();
            string[] controlName = instruct.ControlName;
            string[] controlDesc = instruct.ControlDesc;
            Size[] controlSize = instruct.ControlSize;
            Point[] controlLoc = instruct.ControlLoc;

            string[] valName = instruct.ValName;
            string[] resName = instruct.ResName;
            Size[] sizeVal = instruct.SizeVal;
            Point[] locVal = instruct.LocVal;



            for (int v = 0; v < controlName.Length; v++)
            {
                Label controlLabel = new Label
                {
                    Size = controlSize[v],
                    Location = controlLoc[v],
                    Text = controlDesc[v],
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("NotJamOldStyle11", 15, FontStyle.Bold)
                };
                this.Controls.Add(controlLabel);
            }

            for (int i = 0; i < valName.Length; i++)
            {
                PictureBox newPictureBox = new PictureBox
                {
                    Size = sizeVal[i],
                    Location = locVal[i],
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = Resources.ResourceManager.GetObject(resName[i]) as Bitmap,
                    BackColor = Color.Transparent
                };
                this.Controls.Add(newPictureBox);
            }

            instruct.InitializeBackButton();
            PictureBox backbtn = instruct.BackBtn;
            this.Controls.Add(backbtn);

            instruct.ControlLabel();
            Label control = instruct.controlLabel;
            this.Controls.Add(control);

            foreach (PictureBox removePbox in pBoxArray)
            {
                if (removePbox.Parent != null)
                {
                    removePbox.Parent.Controls.Remove(removePbox);
                    removePbox.Dispose();
                }
            }

            //Display New Pictureboxes for NameInput
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            this.Hide();
            mainMenu.Show();
        }


        // ----------------- DESIGN AND LAYOUT ------------------- //
        public void DisplayImage(string[] valName, string[] resName, Size[] sizeVal, Point[] locVal)
        {


            // Array of PictureBox
            pBoxArray = new PictureBox[valName.Length];

            for (int i = 0; i < pBoxArray.Length; i++)
            {
                // Set the location and size of the PictureBox control.
                pBoxArray[i] = new PictureBox
                {
                    Size = sizeVal[i],
                    Location = locVal[i]
                };

                // Set the SizeMode to CenterImage/Zoom
                if (valName[i] == "TitleName")
                {
                    pBoxArray[i].SizeMode = PictureBoxSizeMode.CenterImage;
                }
                else
                {
                    pBoxArray[i].SizeMode = PictureBoxSizeMode.Zoom;
                }
                //diswan


                // Define a margin by setting the Padding property
                pBoxArray[i].Padding = new Padding(10);

                // Load image from resources
                Bitmap loadImg = (Bitmap)Resources.ResourceManager.GetObject(resName[i]);



                //Make Image Transparent
                var transparentImage = MakeTransparent(loadImg, Color.Black, 30);


                if (loadImg != null)
                {
                    pBoxArray[i].Image = transparentImage; // Assign the loaded image to the PictureBox
                    pBoxArray[i].BackColor = Color.Transparent;

                    if (valName[i] == "Btn_Play")
                    {
                        pBoxArray[i].Click += PlayButton_Click;
                    }

                    else if (valName[i] == "Btn_Lboards")
                    {
                        pBoxArray[i].Click += LeaderboardsButton_Click;
                    }
                    else if (valName[i] == "Btn_Exit")
                    {
                        pBoxArray[i].Click += ExitButton_Click;
                    }
                    else if (valName[i] == "Btn_Start")
                    {
                        pBoxArray[i].Click += StartButton_Click;
                    }
                    else if (valName[i] == "Btn_Instruction")
                    {
                        pBoxArray[i].Click += InstructionButton_Click;
                    }
                }
                this.Controls.Add(pBoxArray[i]);
                pBoxArray[i].BringToFront();
            }
        }
        public Bitmap MakeTransparent(Bitmap bitmap, Color transparentcolor, int tolerance)
        {
            // Make picturebox background transparent
            if (bitmap == null)
            {
                // Handle the case where the input bitmap is null
                return null;
            }

            Bitmap transparentImage = new Bitmap(bitmap);

            for (int i = transparentImage.Size.Width - 1; i >= 0; i--)
            {
                for (int j = transparentImage.Size.Height - 1; j >= 0; j--)
                {
                    var currentColor = transparentImage.GetPixel(i, j);
                    if (Math.Abs(transparentcolor.R - currentColor.R) < tolerance &&
                        Math.Abs(transparentcolor.G - currentColor.G) < tolerance &&
                        Math.Abs(transparentcolor.B - currentColor.B) < tolerance)
                        transparentImage.SetPixel(i, j, currentColor);
                }
            }

            transparentImage.MakeTransparent(transparentcolor);
            return transparentImage;
        }
        private void FormLayout()
        { //Changes the Layout of the Current Form

            //Remove Minimize and Maximize Button
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //Resize Current Form
            Size = new Size(602, 488);

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Text = "ETHEREA: Echoes From The Whispering Forest";

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Icon = Resources.etherea_icon;
        }


        // ----------------- BACKGROUND GIF FRAMES ------------------- //
        public void ChangeBg()
        {
            InitializeGifFrames();
            InitializeAnimationTimer();
            animationTimer.Start();
        }
        private void InitializeGifFrames()
        {
            int numberOfFrames = 9; // Adjust this based on your requirements

            for (int i = 1; i <= numberOfFrames; i++)
            {
                string resourceName = "title_bg" + i;
                Image frame = (Image)Resources.ResourceManager.GetObject(resourceName);
                bgFrames.Add(frame);
            }
        }
        private void InitializeAnimationTimer()
        {
            animationTimer = new Timer
            {
                Interval = 100 // Set the interval to control the animation speed (in milliseconds).
            };
            animationTimer.Tick += new EventHandler(OnAnimationTimerTick);
        }
        private void OnAnimationTimerTick(object sender, EventArgs e)
        {
            // Update the background image to the next frame
            currentFrameIndex = (currentFrameIndex + 1) % bgFrames.Count; // Loop through frames
            this.BackgroundImage = bgFrames[currentFrameIndex];
        }
    }
}
