using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class RunningTimer
    {
        public Label timerLabel = new Label();

        internal int timeInSeconds;

        private int seconds = 0;
        private int minutes = 0;

        private readonly Timer gameTimer = new Timer();

        // variable to store the timer score
        internal int timerScoreInSeconds = 0;

        // Add property to get the timer score
        internal int TimerScoreInSeconds => timerScoreInSeconds;

        public RunningTimer()
        {
            InitializeGameTime();
        }

        internal Label GetTimerLabel()
        {
            return timerLabel;
        }

        internal Timer TimerScore()
        {
            return gameTimer;
        }

        internal int GetTimerSeconds() // Get for value of seconds
        {
            return seconds;
        }
        internal void SetTimerSeconds(bool changeTime, int updatedSeconds) // Set for the value of seconds
        {
            int adjustSeconds = updatedSeconds;
            // If TRUE, it will decrease the time based on a certain value (for good choice)
            if (changeTime) 
            { 
                seconds -= adjustSeconds;
            }
            // If FALSE, it will increase the time based on a certain value (for bad choice)
            else if (!changeTime)
            {
                seconds += adjustSeconds;
            }
        }

        internal void UpdateTimerLabel(int addSec)
        {
            seconds += addSec;
            
            timerLabel.Text = $"{minutes}:{seconds.ToString("D2")}";
        }
        private void InitializeGameTime()
        {
            gameTimer.Interval = 1000;
            gameTimer.Tick += Timer_Tick;

            timerLabel.Text = "0:00";
            timerLabel.Location = new Point(285, 20);
            timerLabel.Size = new Size(60, 30);
            timerLabel.BackColor = Color.Transparent;
            timerLabel.Font = new Font("NotJamOldStyle11", 14, FontStyle.Bold);
            timerLabel.ForeColor = Color.White;
        }


        private void Timer_Tick(Object sender, EventArgs e)
        {
            seconds++;
            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            // update timer score
            timerScoreInSeconds++;

            //formatting of timerLabel
            timerLabel.Text = $"{minutes}:{seconds.ToString("D2")}";
        }
    }
}