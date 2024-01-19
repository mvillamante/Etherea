using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class StagePlay : Form
    {
        // Instances Variables
        private readonly MiaCharacter mia;
        private readonly EnemyCharacters enemy;
        private readonly NpcCharacters npc;
        private readonly GameOver gameover;
        private readonly RunningTimer gametimer;
        private readonly MusicPlayer mplayer;

        // For black BG
        private readonly Panel stageTitle = new Panel();
        private int currentAlpha = 255;

        //panel for completed game
        internal Panel storyCompleted = new Panel();
        internal Panel gameCompleted = new Panel();
        internal PictureBox finished_story;
        internal Button continueBtn;

        internal Button lboardsfromFinish;
        internal Button menufromFinish;
        internal Button exitfromFinish;

        internal PictureBox game_finished;

        // Timers
        private readonly Timer backgroundTimer = new Timer();
        private readonly Timer GIFTimer = new Timer();
        private readonly Timer overlayTimer = new Timer();
        private readonly Timer enemyTimer;

        //Images for Background
        internal Image background;
        internal readonly Image background1 = Resources.Stage1_BG; //Stage-1
        internal readonly Image background2 = Resources.Stage2_BG; //Stage-1
        internal readonly Image background3 = Resources.Stage3_BG; //Stage-1

        private Button stageButton;

        // Background something
        internal int backgroundX = 0;
        internal readonly int backgroundSpeed = 25;

        // Stage Level
        private int currentStage = 1;


        // Booleans
        private bool isAnimating = false;
        internal bool isMiaAlive = true;
        private bool playerReachEnemy = false;
        internal bool enemyHasShown = false;


        readonly NameInput nameinput = new NameInput();

        public StagePlay()
        {

            // ------------- INSTANCES FROM DIFFERENT CLASSES ---------------- //

            mia = new MiaCharacter();
            enemy = new EnemyCharacters();
            npc = new NpcCharacters();
            gametimer = new RunningTimer();
            gameover = new GameOver();
            mplayer = new MusicPlayer();

            // ------------- MOVEMENT KEY EVENTS ---------------- //

            this.KeyDown += Mia_KeyDown;
            this.KeyUp += Mia_KeyUp;
            this.KeyPress += Mia_KeyPress;

            // -------------------ADD MIA CONTROLS----------------- //

            this.Controls.Add(mia.GetMiaPictureBox()); //MiaPicture
            this.Controls.Add(mia.GetMiaProgressBar()); //MiaHealthBar
            mia.GetMiaProgressBar().BringToFront();
            this.Controls.Add(mia.GetMiaHealthLabel()); //MiaHealthLabel
            mia.GetMiaHealthLabel().BringToFront();

            // ------------- STAGE DESIGN AND LAYOUT ---------------- //

            FormLayout();
            BackgroundChange();
            StageTitle();

            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Paint += Stage_Paint;

            // ----------------- TIMER EVENTS ------------------- //

            //Background Timer
            backgroundTimer.Interval = 50; // Adjust animation speed if needed
            backgroundTimer.Tick += BackgroundTimer_Tick;
            backgroundTimer.Start();

            //enemyTimer
            enemyTimer = new Timer();
            enemyTimer.Interval = 1000;
            enemyTimer.Tick += EnemyTimer_Tick;
            enemyTimer.Start();

            //GIF Timer
            GIFTimer.Interval = 500; // Adjust animation speed if needed
            GIFTimer.Tick += GIFTimer_Tick;

            // GameTimer Label Controls
            Label timerLabel = gametimer.GetTimerLabel();
            this.Controls.Add(timerLabel);

            // -------------------------------------------------- //

            playerReachEnemy = false;

        }


        // Get the  X-position value of backgroundX
        internal int GetBackgroundX
        {
            get { return backgroundX; }
        }

        // --------------------- KEY HANDLER EVENTS ------------------------ //
        private void Mia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isMiaAlive)
            {
                char keyPressed = e.KeyChar;

                // You can check the character value of the key pressed
                if (keyPressed == ' ' && !isAnimating)
                {
                    // Start the animation when the Space key is pressed
                    isAnimating = true;

                    mia.MiaWeaponAtk();
                    GIFTimer.Start(); // Start the timer for the animation

                    //Enemy will be Attacked
                    if (playerReachEnemy && mia.GetMiaIsFacingRight()) // Mia can only damage the enemy if she is facing right
                    {
                        // Mia attacks the enemy
                        enemy.EnemyHurt();
                        enemy.SetEnemyHealthLabel();
                    }
                }

                //CHECKING LANG TONG N AT M IF NAGANA BA//
                if (char.ToUpper(keyPressed) == 'N') //Temporary Key for Damaging Mia
                {
                    isAnimating = true;

                    mia.SetMiaHealthValue(false, enemy.EnemyAttack());
                    mia.SetMiaHealthLabel();

                    GIFTimer.Start(); // Start the timer for the animation
                }

                if (char.ToUpper(keyPressed) == 'M') //Temporary Key for Regaining health of Mia
                {
                    isAnimating = true;

                    mia.SetMiaHealthValue(true, 50);
                    mia.SetMiaHealthLabel();

                    GIFTimer.Start(); // Start the timer for the animation
                }
            }
        }
        private void Mia_KeyDown(object sender, KeyEventArgs e)
        {
            if (isMiaAlive)
            {
                switch (e.KeyCode)
                {
                    // If (player) Mia moves to the right then it will move towards right
                    case Keys.Right:
                    case Keys.D:
                        Timer time = gametimer.TimerScore();
                        // (player) Mia can only move to the right if the enemy is far (their distance is big)
                        if (!playerReachEnemy && !IsNpcNear()|| mia.GetMiaPictureBox().Right < enemy.GetEnemyPictureBox().Left)
                        {
                            mia.MoveRight();

                            // Start the Game Timer when Mia moves
                            time.Start();
                        }
                        else if (playerReachEnemy && mia.GetMiaPictureBox().Right >= enemy.GetEnemyPictureBox().Left)
                        {
                            mia.SetMiaIsFacingRight();

                            // Start the Game Timer when Mia moves
                            time.Start();
                        }
                        else if (IsNpcNear() && currentStage != 3)
                        {
                            InitializeButtons();
                            this.Controls.Add(npc.GetDialogueBox());

                            // Pause the Game Timer when Next Stage
                            time.Stop();
                            mia.StopMoving();
                            isMiaAlive = false;
                        }
                        break;

                    // If (player) Mia moves to the left then it will move towards left
                    case Keys.Left:
                    case Keys.A:
                        if (!IsNpcNear())
                        {
                            mia.MoveLeft();
                        }
                        else
                        {
                            mia.StopMoving();
                        }
                        break;
                }
            }
        }
        private void Mia_KeyUp(object sender, KeyEventArgs e)
        {
            if (isMiaAlive)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                    case Keys.D:
                    case Keys.Left:
                    case Keys.A:
                        mia.StopMoving();
                        break;
                }
            }
        }


        // --------------------- DISPLAY ENEMY AND NPC ------------------------ //
        internal void AddCharacters(int miaDistance)
        {
            switch (miaDistance)
            {
                // Display Controls for Enemy
                case 300:
                    if (!enemyHasShown)
                    {
                        // Add enemy controls
                        if (!this.Controls.Contains(enemy.GetEnemyPictureBox()))
                        {
                            this.Controls.Add(enemy.GetEnemyPictureBox()); //EnemyPictureBox
                            this.Controls.Add(enemy.GetEnemyProgressBar()); //EnemyHealthBar
                            this.Controls.Add(enemy.GetEnemyHealthLabel()); //EnemyHealthLabel

                            enemy.GetEnemyPictureBox().BringToFront();
                            enemyHasShown = true;
                        }
                    }
                    break;

                // Display Controls for NPC
                case 700:
                    if(currentStage != 3)
                    {
                        // Add npc Controls
                        this.Controls.Add(npc.GetNpcPictureBox());
                        npc.GetNpcPictureBox().BringToFront();
                    };
                    break;
            }
        }
        private bool IsNpcNear()
        {
            int distance = npc.GetNpcPictureBox().Left - mia.GetMiaPictureBox().Right;
            return distance <= 45 && distance >= 10;

        }
        private bool IsEnemyFar()
        {
            int distance = enemy.GetEnemyPictureBox().Left - mia.GetMiaPictureBox().Right;
            return distance > 10 && distance < 5;

        }


        // --------------------- NPC CHOICES ------------------------ //
        internal void RandomNPC()
        {
            Random rand = new Random();
            string randomNPC = npc.goodOrBadNPC[rand.Next(npc.goodOrBadNPC.Length)];
            int advantageLen = npc.Advantages.Length;
            string[] advantages = npc.Advantages;

            while (advantageLen > 1)
            {
                advantageLen--;
                int k = rand.Next(advantageLen + 1);
                string value = advantages[k];
                advantages[k] = advantages[advantageLen];
                advantages[advantageLen] = value;
            }

            int consequenceLen = npc.Consequences.Length;
            string[] consequences = npc.Consequences;
            while (consequenceLen > 1)
            {
                consequenceLen--;
                int k = rand.Next(consequenceLen + 1);
                string value = consequences[k];
                consequences[k] = consequences[consequenceLen];
                consequences[consequenceLen] = value;
            }

            if (randomNPC == "Good NPC")
            {
                npc.btnName[0].Text = advantages[0];
                npc.btnName[1].Text = advantages[1];
                npc.btnName[2].Text = advantages[2];
            }
            else
            {
                npc.btnName[0].Text = consequences[0];
                npc.btnName[1].Text = consequences[1];
                npc.btnName[2].Text = consequences[2];
            }
        }
        private void HandleButton(string buttonText)
        {
            switch (buttonText)
            {
                case "Regain Health":
                    int addValue = 100 - mia.GetMiaHealthValue();

                    mia.SetMiaHealthValue(true, addValue);
                    mia.SetMiaHealthLabel();

                    break;
                case "Give Weapon":
                    mia.GiveWeapon();
                    break;
                case "Lessen Time":
                    gametimer.UpdateTimerLabel(-10);
                    break;
                case "Lessen Health":
                    mia.SetMiaHealthValue(true, -10);
                    mia.SetMiaHealthLabel();
                    break;
                case "Add Time":
                    gametimer.UpdateTimerLabel(+10);
                    break;
                case "Take Weapon":
                    mia.TakeWeapon();
                    break;
            }
            foreach (Button btn in npc.btnName)
            {
                if (btn.Text == buttonText)
                {
                    btn.Enabled = false;
                    break;
                }
            }
        }
        internal void ChoiceA_MouseClick(object sender, EventArgs e)
        {
            Controls.Remove(npc.btnName[1]);
            Controls.Remove(npc.btnName[2]);
            RandomNPC();
            HandleButton(npc.btnName[0].Text);

            StageButton();

            // Add Clicking SFX
            MusicPlayer musicPlayer = new MusicPlayer();
            mplayer.NpcChoice();
        }
        internal void ChoiceB_MouseClick(object sender, EventArgs e)
        {
            Controls.Remove(npc.btnName[0]);
            Controls.Remove(npc.btnName[2]);
            RandomNPC();
            HandleButton(npc.btnName[1].Text);

            StageButton();

            // Add Clicking SFX
            MusicPlayer musicPlayer = new MusicPlayer();
            mplayer.NpcChoice();
        }
        internal void ChoiceC_MouseClick(object sender, EventArgs e)
        {
            Controls.Remove(npc.btnName[0]);
            Controls.Remove(npc.btnName[1]);
            RandomNPC();
            HandleButton(npc.btnName[2].Text);

            StageButton();

            // Add Clicking SFX
            MusicPlayer musicPlayer = new MusicPlayer();
            mplayer.NpcChoice();
        }
        internal void InitializeButtons()
        {
            MouseEventHandler[] btnArray = { ChoiceA_MouseClick, ChoiceB_MouseClick, ChoiceC_MouseClick };
            for (int i = 0; i < npc.btnName.Length; i++)
            {
                npc.btnName[i] = new Button()
                {
                    Size = npc.btnSize[i],
                    Location = npc.btnLoc[i],
                    Text = npc.btnText[i],
                    ForeColor = Color.White,
                    BackColor = Color.Brown,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 }
                };
                npc.btnName[i].MouseClick  +=  btnArray[i];
                this.Controls.Add(npc.btnName[i]);
                npc.btnName[i].BringToFront();
            }
        }


        // ---------------------- STAGE BUTTON ------------------------- //
        internal void StageButton()
        {
            stageButton = new Button()
            {
                Text = "Next Stage >",
                Font = new Font("NotJamOldStyle11", 14, FontStyle.Bold),
                Size = new Size(175, 50),
                Location = new Point(400, 430),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Enabled = true,
                FlatAppearance = { BorderSize = 0 }
            };
            stageButton.MouseClick += StageBtn_Click;

            this.Controls.Add(stageButton);
            stageButton.BringToFront();
        }
        private void StageBtn_Click(object sender, EventArgs e)
        {

            //Remove NPC Controls
            this.Controls.Remove(npc.GetNpcPictureBox());
            this.Controls.Remove(npc.GetDialogueBox());
            this.Controls.Remove(stageButton);

            // Remove buttons
            foreach (Button btn in npc.btnName)
            {
                Controls.Remove(btn);
            }

            // Next Stage
            currentStage += 1;

            // --------------SET TO DEFAULT SETTINGS------------------ //

            // For enemy
            enemy.SetEnemyIndex(currentStage-1);
            enemy.SetEnemyDistanceRan(0);

            int defaultenemyLoc = enemy.enemyLoc[0].X;
            enemy.SetEnemyX(defaultenemyLoc);

            enemy.SetEnemyHealthValue();
            enemy.SetEnemyHealthLabel();

            //For Mia
            mia.SetMiaDistanceRan(0);
            mia.CurrentWeapon = "dagger";
            
            if(currentStage != 3)
            {
                //For NPC
                npc.SetNpcX(595);
            }

            // designs & layout
            backgroundX = 0;
            StageTitle();
            BackgroundChange();

            isMiaAlive = true;
            enemyHasShown = false;
        }

        

        // --------------------------- TIMER EVENT ----------------------------- //
        private void EnemyTimer_Tick(object sender, EventArgs e)
        {
            while (this.Controls.Contains(enemy.GetEnemyPictureBox()))
            {
                // Check if the player is close to the enemy
                if (!IsEnemyFar())
                {
                    playerReachEnemy = true;
                }
                else if (IsEnemyFar()) // enemy will move fast towards mia
                {
                    enemyTimer.Interval = 1000;
                }


                // If the player has reached the enemy
                if (playerReachEnemy)
                {
                    // Check for collision between mia and enemy
                    if (mia.GetMiaPictureBox().Bounds.IntersectsWith(enemy.GetEnemyPictureBox().Bounds))
                    {
                        GIFTimer.Start();

                        /* Perform enemy attack on mia,
                           Update Mia's health, reduce by 10 */
                        mia.SetMiaHealthValue(false, enemy.EnemyAttack());
                        mia.SetMiaHealthLabel();

                        // enemy will attack slowly
                        enemyTimer.Interval = 2000;
                    }

                    else
                    {
                        // Move the enemy towards mia
                        int followLeft = mia.GetMiaPictureBox().Right;
                        enemy.EnemyMoving();
                        if (enemy.GetEnemyPictureBox().Left < followLeft)
                        {
                            enemy.GetEnemyPictureBox().Left += 0;
                        }
                        else if (enemy.GetEnemyPictureBox().Left > followLeft)
                        {
                            enemy.GetEnemyPictureBox().Left -= 20;
                        }
                    }
                    // Check for game over conditions (either mia's health or enemy's health is <= 0) 
                    int miaHealth = mia.GetMiaHealthValue();
                    int enemyHealth = enemy.GetEnemyHealthValue();

                    
                    if (miaHealth <= 0 || enemyHealth <= 0)
                    {
                        enemyTimer.Stop();
                    }
                }
                break;
            }
        }
        private void BackgroundTimer_Tick(object sender, EventArgs e)
        {
            int miaDistance = mia.GetMiaDistanceRan();
            int enemyDistance = enemy.GetEnemyDistanceRan();

            const int targetCenterX = 64;
            const int adjustmentSpeed = 3;

            // ----------------- CENTER MIA WHEN NOT MOVING ------------------- // 
            if (!mia.IsMovingLeft && !mia.IsMovingRight) // Update Mia's distance when not moving
            {
                // You can adjust this value to control the speed of the adjustment
                int miaAdjustment = (targetCenterX - mia.GetMiaX()) / adjustmentSpeed;
                mia.SetMiaX(mia.GetMiaX() + miaAdjustment);

                // Move the background to the right
                backgroundX += miaAdjustment;

                // Update Enemy if Enemy picturebox exists
                if (this.Controls.Contains(enemy.GetEnemyPictureBox()))
                {
                    enemy.SetEnemyX(enemy.GetEnemyX() + miaAdjustment); // Adjust enemySpeed as needed
                }

                // Update Npc if Npc picturebox exists
                if (this.Controls.Contains(npc.GetNpcPictureBox()))
                {
                    npc.SetNpcX(npc.GetNpcX() + miaAdjustment); // Adjust npcSpeed as needed
                }
            }

            // ----------------- MOVE RIGHT OR LEFT ------------------- //  

            if (mia.IsMovingRight && miaDistance <= 100000)
            {
                backgroundX -= backgroundSpeed; // Move the background to the left if going right

                // Update Mia
                mia.SetMiaDistanceRan(miaDistance + 5);
                if (mia.GetMiaX() <= 320)
                {
                    mia.SetMiaX(mia.GetMiaX() + 3);
                }

                // Enemy and NPC character will show up on certain distance value
                AddCharacters(miaDistance);

                // Update Enemy if Enemy picturebox exists
                if (this.Controls.Contains(enemy.GetEnemyPictureBox()))
                {
                    enemy.SetEnemyDistanceRan(enemyDistance - 5);
                    enemy.SetEnemyX(enemy.GetEnemyX() - 3); // Adjust enemySpeed as needed
                }

                // Update Npc if Npc picturebox exists
                if (this.Controls.Contains(npc.GetNpcPictureBox()))
                {
                    npc.SetNpcX(npc.GetNpcX() - backgroundSpeed); // Adjust npcSpeed as needed
                }
            }
            else if (mia.IsMovingLeft && miaDistance != 0)
            {
                backgroundX += backgroundSpeed; // Move the background to the right if going left

                // Update Mia
                mia.SetMiaDistanceRan(miaDistance - 5);
                if (mia.GetMiaX() >= 0)
                {
                    mia.SetMiaX(mia.GetMiaX() - 3);
                }

                // Update Enemy
                if (this.Controls.Contains(enemy.GetEnemyPictureBox()))
                {
                    enemy.SetEnemyDistanceRan(enemyDistance + 5);
                    enemy.SetEnemyX(enemy.GetEnemyX() + 3); // Adjust enemySpeed as needed
                }

                // Update Npc
                if (this.Controls.Contains(npc.GetNpcPictureBox()))
                {
                    npc.SetNpcX(npc.GetNpcX() + backgroundSpeed); // Adjust npcSpeed as needed
                }
            }

            // Ensure the background looping
            if (backgroundX > 0)
            {
                if (currentStage == 1)
                {
                    backgroundX = -background1.Width;
                }
                else if (currentStage == 2)
                {
                    backgroundX = -background2.Width;
                }
                else if (currentStage == 3) 
                {
                    backgroundX = -background3.Width;
                }
            }
            else if (backgroundX < -background1.Width ||backgroundX < -background2.Width || backgroundX < -background3.Width)
            {
                backgroundX = 0;
            }

            this.Invalidate();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            while (currentAlpha > 0)
            {
                currentAlpha -= 5;
            }
            overlayTimer.Stop();
            this.Controls.Remove(stageTitle);// Stop the timer when transparency is fully reduced
        }
        private void GIFTimer_Tick(object sender, EventArgs e)
        {
            // This method is called at regular intervals by the timer
            // It stops the animation and reverts back to the original image
            GIFTimer.Stop(); // Stop the timerd
            isAnimating = false;

            // Set mia and enemy into idle state
            mia.StopMoving();
            enemy.EnemyStopMoving();

            // Remove all controls for Enemy
            Timer gmtimer = gametimer.TimerScore();
            int enemyHealth = enemy.GetEnemyHealthValue();
            if (enemyHealth <= 0)
            {
                this.Controls.Remove(enemy.GetEnemyPictureBox());
                this.Controls.Remove(enemy.GetEnemyProgressBar());
                this.Controls.Remove(enemy.GetEnemyHealthLabel());

                // Set playerReachEnemy to default to make Mia be able to run right 
                playerReachEnemy = false;

                if(currentStage == 3)
                {
                    InitializeLastStoryLine();

                    // Save the Player Time Progress
                    SaveTimerScore();
                }
                
            }

            // Remove all controls for Mia and Enemy
            int miaHealth = mia.GetMiaHealthValue();
            if (miaHealth <= 0)
            {
                this.Controls.Remove(mia.GetMiaPictureBox());
                this.Controls.Remove(mia.GetMiaProgressBar());
                this.Controls.Remove(mia.GetMiaHealthLabel());

                this.Controls.Remove(enemy.GetEnemyPictureBox());
                this.Controls.Remove(enemy.GetEnemyProgressBar());
                this.Controls.Remove(enemy.GetEnemyHealthLabel());

                // Stop the Game Timer when Mia dies
                gmtimer.Stop();

                // Game movement controls will be disabled if mia dies
                isMiaAlive = false;

                // Display GameOver Form
                this.Hide();
                gameover.Show();

                // Add GameOver SFX
                MusicPlayer mplayer = new MusicPlayer();
                mplayer.PlayGameOver();
            }
        }


        // -------------------- SAVE SCORE IN TEXTFILE ----------------------- //
        internal void SaveTimerScore()
        {
            //TextBox playerNameTextBox = nameinput.UserInput();

            string filePath = "PlayerInfo.txt";
            string timeScore = gametimer.timerLabel.Text;
            try
            {
                StreamWriter writetoFile = File.AppendText(filePath);
                writetoFile.WriteLine($"|{timeScore}");
                writetoFile.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file" + ex.Message);
            }
        }

        // --------------------- ENDING ------------------------ //
        private void LboardsButton_Click(object sender, EventArgs e)
        {
            LeaderBoards lboards = new LeaderBoards();
            lboards.Show();
        }
        private void MenuButton_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to go back to main menu?", "Go Back to Main Menu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                mainMenu.Show();
            }
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit Game", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                //Terminate Form1 Program
                this.Close();
            }
        }
        private void ContinueBtn_Click(object sender, EventArgs e)
        {
            storyCompleted.Controls.Clear();

            InitializeFinishScreen();
            gameCompleted.Visible = true;
        }
        internal void InitializeLastStoryLine()
        {
            // Story Completed Panel for StagePlay
            storyCompleted = new Panel
            {
                BackgroundImage = Resources.finishedBg,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(602, 488)
            };
            this.Controls.Add(storyCompleted);
            storyCompleted.BringToFront();


            // finished_story Panel for StagePlay
            finished_story = new PictureBox()
            {
                Size = new Size(602, 440),
                Image = Resources.storyLine_finished,
                Location = new Point(0, 50),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            storyCompleted.Controls.Add(finished_story);


            // Add Continue Button
            continueBtn = new Button()
            {
                Text = "Continue >",
                Font = new Font("NotJamOldStyle11", 12, FontStyle.Bold),
                Size = new Size(175, 60),
                Location = new Point(410, 10),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Enabled = true,
                FlatAppearance = { BorderSize = 0 },

            };
            continueBtn.Click += ContinueBtn_Click;
            this.storyCompleted.Controls.Add(continueBtn);

        }

        internal void InitializeFinishScreen()
        {

            // Panel for Game completed
            gameCompleted = new Panel
            {
                BackgroundImage = Resources.darkfinishedBg,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(602, 488)
            };
            this.Controls.Add(gameCompleted);
            gameCompleted.BringToFront();


            // Panel for Game finished
            game_finished = new PictureBox()
            {
                Size = new Size(602, 350),
                Image = Resources.game_completed,
                Location = new Point(0, 0),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            gameCompleted.Controls.Add(game_finished);


            // Button for leaderboards
            lboardsfromFinish = new Button
            {
                Text = "Leaderboards",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("NotJamOldStyle11", 12),
                Location = new Point(25, 345),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            lboardsfromFinish.Click += LboardsButton_Click;
            gameCompleted.Controls.Add(lboardsfromFinish);


            // Button for main menu
            menufromFinish = new Button
            {
                Text = "Main Menu",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("NotJamOldStyle11", 12),
                Location = new Point(25, 382),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
            };
            menufromFinish.Click += MenuButton_Click;
            gameCompleted.Controls.Add(menufromFinish);


            // Button for Exit
            exitfromFinish = new Button
            {
                Text = "Exit",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("NotJamOldStyle11", 12),
                Location = new Point(25, 417),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
            };
            gameCompleted.Controls.Add(exitfromFinish);
            exitfromFinish.Click += ExitButton_Click;
        }


        // --------------------- BGM AND SFX EFFECTS ------------------------ //
        private void StageMusic()
        {
            MusicPlayer mplayer = new MusicPlayer();
            switch (currentStage)
            {
                case 1:
                    //mplayer.PlayStageMusic();
                    break;
                case 2:
                    //mplayer.PlayStageMusic();
                    break;
                case 3:
                    //mplayer.PlayStageMusic();
                    break;
            }
        }
        

        // -------------------- STAGE DESIGN AND LAYOUT ----------------------- //

        private void Stage_Paint(object sender, PaintEventArgs e)
        {
            int formHeight = this.Height;

            //Draw the scrolling background
            e.Graphics.DrawImage(background, backgroundX, 0, background.Width, formHeight);
            e.Graphics.DrawImage(background, backgroundX + background.Width, 0, background.Width, formHeight);
        }
        private void StageTitle()
        {
            //Change the text in stage title
            Label[] stageTitleLabel = new Label[3];
            string[] stageTitles = { "Stage 1: The Mysterious Sylvan Forest", "Stage 2: The FrostFall Glade Forest", "Stage 3: The Enchanting Whispering Forest" };

            stageTitle.BackColor = Color.Black;
            stageTitle.Size = new Size(602, 488);

            for (int i = 0; i < stageTitleLabel.Length; i++)
            {
                stageTitleLabel[i] = new Label()
                {
                    Text = stageTitles[i],
                    Size = new Size(525, 100),
                    Location = new Point(30, 150),
                    BackColor = Color.Transparent,
                    Font = new Font("The Wild Breath of Zelda", 20),
                    ForeColor = Color.White,
                    AutoSize = false
                };
            }

            stageTitle.Controls.Clear();

            switch (currentStage)
            {
                case 1:
                    stageTitle.Controls.Add(stageTitleLabel[0]);
                    break;
                case 2:
                    stageTitle.Controls.Add(stageTitleLabel[1]);
                    break;
                case 3:
                    stageTitle.Controls.Add(stageTitleLabel[2]);
                    break;
            }
            stageTitle.BringToFront();

            this.Controls.Add(stageTitle);
            stageTitle.BringToFront();
            overlayTimer.Interval = 3000;
            overlayTimer.Tick += Timer_Tick;
            overlayTimer.Start();
        }
        private void BackgroundChange()
        {
            switch (currentStage)
            {
                case 1:
                    background = Resources.Stage1_BG;
                    break;
                case 2:
                    background = Resources.Stage2_BG;
                    break;
                case 3:
                    background = Resources.Stage3_BG;
                    break;
            }
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

            this.Icon = Resources.etherea_icon;
        }

        public static int CalculateAtkDamage()
        {
            return 15;
        }

        public static int CalculateAtkDamage(int index, int[] array)
        {
            return array[index];
        }

    }
}
