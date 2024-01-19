using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class EnemyCharacters
    {
        //For enemy picture box
        internal PictureBox EnemyState;
        private readonly string[] resName = new string[] { "sprout_", "skullwolf_", "guardian_" };
        private readonly Size[] sizeVal = new Size[] { new Size(250, 250), new Size(270, 285), new Size(500,500) };
        internal readonly Point[] enemyLoc = new Point[] { new Point(500, 245), new Point(585, 185), new Point(585, 100) };
        private readonly int[] dmgPoints = new int[] { 5, 10, 20};

        // For enemy health bar
        private ProgressBar EnemyHealthBar;
        private int valHealth = 100;

        // For enemy health bar text
        private Label EnemyHealthLabel;

        //temporary int  enemyIndex
        private readonly Random rdm = new Random();
        private int enemyIndex;
        internal List<int> tempArr = new List<int> { };

        // For the value of the distance the enemy has ran
        private int enemydistanceRan = 0;

        // boolean value determining  when the enemy will follow player mia
        internal bool isfollowingMia;


        public EnemyCharacters() 
        {
            enemyIndex = 0;

            string enemyImg = resName[enemyIndex] + "idle";
            if (!tempArr.Contains(enemyIndex))
            {
                InitializeEnemyCharacter(sizeVal[enemyIndex], enemyLoc[enemyIndex], enemyImg);
                EnemyHealthStatus();
                SetEnemyHealthLabel();
                tempArr.Add(enemyIndex);
            }
        }


        // Call for the initialization of Enemy Image
        internal PictureBox GetEnemyPictureBox()
        {
            return EnemyState;
        }


        // Call for the initialization of Enemy Image
        internal ProgressBar GetEnemyProgressBar()
        {
            return EnemyHealthBar;
        }


        // Get-Set the health value of the enemy
        internal int GetEnemyHealthValue() // Get the health value of the enemy
        {
            return valHealth;
        }
        internal void SetEnemyHealthValue() // Get the health value of the enemy
        {
            valHealth = 100;
            EnemyHealthBar.Value = valHealth;
        }
        internal Label GetEnemyHealthLabel()  // Get for the progress bar text label
        {
            return EnemyHealthLabel;
        }
        internal void SetEnemyHealthLabel() // Set for the progress bar text label
        {
            EnemyHealthLabel.Text = "Health: " + valHealth.ToString();
        }

        //Get-Set the value for the distance mia has ran
        internal int GetEnemyDistanceRan()
        {
            return enemydistanceRan;
        }
        internal void SetEnemyDistanceRan(int newDistance)
        {
            enemydistanceRan = newDistance;
        }

        // Get-Set the X-position of enemy
        internal int GetEnemyX() // Get the X-position value of the enemy
        {
            return EnemyState.Location.X;
        }
        public void SetEnemyX(int newenemyX) // Set the X-position value of the enemy
        {
            EnemyState.Location = new Point(newenemyX, EnemyState.Location.Y);
        }


        // Set the value of enemyIndex
        internal int GetEnemyIndex() // Get the X-position value of the enemy
        {
            return enemyIndex;
        }
        internal void SetEnemyIndex(int newIndex) // Set the X-position value of the enemy
        {
            enemyIndex = newIndex;
        } 


        // Initialization of Character Enemy
        private void InitializeEnemyCharacter(Size size, Point location, string img)
        {
            string enemyImg = resName[enemyIndex] + "idle";
            EnemyState = new PictureBox
            {
                Size = size,
                Location = location,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = (Image)Resources.ResourceManager.GetObject(enemyImg),
                BackColor = Color.Transparent
            };
        }


        // Initialization of Enemy Health Bar and Text Label
        public void EnemyHealthStatus()
        {
            // Initialize value of enemy health bar and its min/max value
            EnemyHealthBar = new ProgressBar
            {
                Maximum = 100,
                Minimum = 0,
                Value = valHealth,
                Location = new Point(360, 12),
                Size = new Size(226, 23),
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.Red
            };

            // Initialize the text of enemy health label
            EnemyHealthLabel = new Label
            {
                Text = "Health: " + valHealth.ToString(),
                Location = new Point(458, 40),
                Size = new Size(235, 30),
                Font = new Font("NotJamOldStyle11", 12, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Red
            };
        }


        // ----------------- ENEMY CHARACTER COMBAT SYSTEM ------------------- //


        internal int EnemyAttack()
        {
            // Change image for enemy_attack
            string enemyImg = resName[enemyIndex] + "atk";
            EnemyState.Image = (Image)Resources.ResourceManager.GetObject(enemyImg);
            EnemyState.BackColor = Color.Transparent;

            int randomMaxDmg = StagePlay.CalculateAtkDamage(enemyIndex, dmgPoints);

            // Use the random index to get the corresponding value from dmgPoints
            //int randomMaxDmg = dmgPoints[enemyIndex];
            int randomHitDmg = rdm.Next((randomMaxDmg + 1) - 3, randomMaxDmg);
            return randomHitDmg;
        }


        internal void EnemyHurt()
        {
            string enemyImg;
            if(valHealth > 0)
            {
                 valHealth -= StagePlay.CalculateAtkDamage();
            }
            
            // Change image for enemy_hurt
            enemyImg = resName[enemyIndex] + "hurt";
            
            if (valHealth <= 0)
            {
                valHealth = 0;
                // Change image for enemy_death
                enemyImg = resName[enemyIndex] + "death";
            }

            EnemyHealthBar.Value = valHealth;

            EnemyState.Image = (Image)Resources.ResourceManager.GetObject(enemyImg);
            EnemyState.BackColor = Color.Transparent;
        }

        // ----------------- ENEMY CHARACTER MOVEMENT ------------------- //

        internal void EnemyStopMoving()
        {
            // Change image back to enemy_idle
            string enemyImg = resName[enemyIndex] + "idle";
            EnemyState.Image = (Image)Resources.ResourceManager.GetObject(enemyImg); // Change image
            EnemyState.BackColor = Color.Transparent;
        }

        internal void EnemyMoving()
        {
            // Change image back to enemy_idle
            string enemyImg = resName[enemyIndex] + "move";
            EnemyState.Image = (Image)Resources.ResourceManager.GetObject(enemyImg); // Change image
            EnemyState.BackColor = Color.Transparent;
        }
    }
}
