using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    public class MiaCharacter
    {
        // Store Mia GIF Images in this picturebox
        internal PictureBox MiaState;

        // Mia idle animations
        private readonly Image playerMiaRightIdle = Resources.mia_rightidle;
        private readonly Image playerMiaLeftIdle = Resources.mia_leftidle;

        // Mia run animations
        private readonly Image playerMiaRightRun = Resources.mia_rightrun;
        private readonly Image playerMiaLeftRun = Resources.mia_leftrun;

        // Mia hurt animations
        private readonly Image playerMiaRightHurt = Resources.mia_righthurt;
        private readonly Image playerMiaLeftHurt = Resources.mia_lefthurt;

        // Mia dagger animations
        private readonly Image playerMiaRightDagger = Resources.mia_rightdagger; //adjust nga na isang slash lang
        private readonly Image playerMiaLeftDagger = Resources.mia_leftdagger;

        // Mia bow animations
        private readonly Image playerMiaRightBow= Resources.mia_rightbow;
        private readonly Image playerMiaLeftBow = Resources.mia_leftbow;


        // For mia health bar
        private ProgressBar MiaHealthBar;
        private int valHealth = 100;

        // For mia health bar text
        private Label MiaHealthLabel;

        // boolean value if mia is moving right or left
        public bool IsMovingLeft { get; private set; }
        public bool IsMovingRight { get; private set; }

        // boolean value if mia is facing right or left
        internal bool isFacingRight = true;
        private bool isFacingLeft = false;

        // Mia Initial Position
        private int miaX = 0;
        private readonly int miaY = 260;
        private int miadistanceRan = 0;

        // Mia Initial Weapon
        internal string CurrentWeapon;

        public MiaCharacter()
        {
            CurrentWeapon = "dagger";

            InitializeMiaCharacter();
            MiaHealthStatus();
            SetMiaHealthLabel();
        }

        // Call for the initialization of Mia Image
        internal PictureBox GetMiaPictureBox()
        {
            return MiaState;
        }


        // Call for the initialization of Mia Health
        internal ProgressBar GetMiaProgressBar()
        {
            return MiaHealthBar;
        }


        // Get-Set the health value of Mia
        internal int GetMiaHealthValue() // Get for the progress bar
        {
            return valHealth;

        }
        internal void SetMiaHealthValue(bool updateHealth, int changeHealthValue) // Set for the progress bar
        {
            // Mia's health will decrease
            if(!updateHealth)
            {
                if(valHealth > 0 && isFacingRight)
                {
                    MiaState.Image = playerMiaRightHurt;
                }
                else if(valHealth > 0 && isFacingLeft)
                {
                    MiaState.Image = playerMiaLeftHurt;
                }

                valHealth -= changeHealthValue;
                /* If the value of valHealth reaches below 0 then 
                 * it means Mia will reach the default minimum health value*/
                if (valHealth <= 0)
                {
                    valHealth = 0;
                }
                else if (valHealth >= 100) {
                    valHealth = 100;
                }
                MiaHealthBar.Value = valHealth;
                MiaState.BackColor = Color.Transparent;
            }
            // Mia's health will increase
            else if(updateHealth)
            {
                if (valHealth <= 100)
                {
                    valHealth += changeHealthValue; //value babaguhin pa
                    if (valHealth >= 100)
                    {
                        valHealth = 100;
                    }

                    MiaHealthBar.Value = valHealth;
                }
            }
        }
        internal Label GetMiaHealthLabel() // Get for the progress bar text label
        {
            return MiaHealthLabel;
        }
        internal void SetMiaHealthLabel() // Set for the progress bar text label
        {
            MiaHealthLabel.Text = "Health: " + valHealth.ToString();
        }

        // Get-Set X-position of Mia
        internal int GetMiaX()
        {
            return miaX;
        }
        internal void SetMiaX(int newX)
        {
            miaX = newX;
            MiaState.Location = new Point(miaX, miaY);
        }

        //Get-Set the value for the distance mia has ran
        internal int GetMiaDistanceRan()
        {
            return miadistanceRan;
        }
        internal void SetMiaDistanceRan(int newDistance)
        {
            miadistanceRan = newDistance;
        }


        //Get-Set the value the current weapon mia uses
        internal string GetMiaCurrentWeapon()
        {
            return CurrentWeapon;
        }
        internal void SetMiaCurrentWeapon(string newWeapon)
        {
            CurrentWeapon = newWeapon;
        }


        // Get-Set the boolean value if Mia is facing right
        internal bool GetMiaIsFacingRight()
        {
            return isFacingRight;
        }
        internal void SetMiaIsFacingRight()
        {
            isFacingRight = true;
        }


        // Initialization of Character Mia
        private void InitializeMiaCharacter()
        {
            MiaState = new PictureBox
            {
                Size = new Size(260, 255),
                Location = new Point(miaX, miaY),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = playerMiaRightIdle,
                BackColor = Color.Transparent
            };
        }


        // Initialization of Mia Health Bar
        private void MiaHealthStatus()
        {
            // Initialize mia health bar
            MiaHealthBar = new ProgressBar
            {
                Maximum = 100,
                Minimum = 0,
                Value = valHealth,
                Location = new Point(15, 12),
                Size = new Size(226, 23),
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.LightGreen
            };

            // Initialize mia health label
            MiaHealthLabel = new Label
            {
                Text = "Health: " + valHealth.ToString(),
                Location = new Point(15, 40),
                Size = new Size(235, 30),
                Font = new Font("NotJamOldStyle11", 12, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.LightGreen
            };
        }


        // ----------------- MIA CHARACTER COMBAT SYSTEM ------------------- //
        internal void MiaWeaponAtk()
        {
            //Add SFX when Mia is Attacking 
            MusicPlayer mplayer = new MusicPlayer();
            mplayer.MiaAtkSFX();

            if (isFacingRight)
            {
                switch(CurrentWeapon)
                {
                    case "dagger": // Default Weapon (Light-Attack)
                        MiaState.Image = playerMiaRightDagger;

                        break;
                    case "bow": // Bow Weapon (Long-Ranged)
                        MiaState.Image = playerMiaRightBow;
                        break;
                }
            }
            else if (isFacingLeft)
            {
                switch (CurrentWeapon)
                {
                    case "dagger": // Default Weapon (Light-Attack)
                        MiaState.Image = playerMiaLeftDagger;
                        break;
                    case "bow": // Bow Weapon (Long-Ranged)
                        MiaState.Image = playerMiaLeftBow;
                        break;
                }
            }
            MiaState.BackColor = Color.Transparent;
        }
        internal void GiveWeapon()
        {
            // Give a bow weapon if it's a good NPC
            SetMiaCurrentWeapon("Bow");
        }

        internal void TakeWeapon()
        {
            // Change the player's weapon to Dagger if they have Bow or Axe
            if (GetMiaCurrentWeapon() == "Bow")
            {
                SetMiaCurrentWeapon("Dagger");
            }
        }

        // ----------------- MIA CHARACTER MOVEMENT ------------------- //

        internal void MoveRight() // Moving to Right GIF Image
        {
            if (!IsMovingRight)
            {
                MiaState.Image = playerMiaRightRun;
                MiaState.BackColor = Color.Transparent;
            }
            IsMovingRight = true;
            isFacingRight = true;
            isFacingLeft = false;
        }

        internal void MoveLeft() // Moving to Left GIF Image
        { 
            if (!IsMovingLeft)
            {
                MiaState.Image = playerMiaLeftRun;
                MiaState.BackColor = Color.Transparent;
            }
            IsMovingLeft = true;
            isFacingLeft = true;
            isFacingRight = false;

        }

        internal void StopMoving() // Idle GIF Image
        {
            if (IsMovingRight || isFacingRight)
            {
                IsMovingRight = false;
                MiaState.Image = playerMiaRightIdle;
            }
            else if (IsMovingLeft || isFacingLeft)
            {
                IsMovingLeft = false;
                MiaState.Image = playerMiaLeftIdle;
            }
            
            MiaState.BackColor = Color.Transparent;
        }
    
        
    }
}
