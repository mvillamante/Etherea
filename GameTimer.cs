using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class GameTimer
    {
        // For showing the next frame
        private Timer frameTimer;
        public delegate void OnFrameTimerTick(object sender, EventArgs e);
        public event OnFrameTimerTick frameTimer;

        // For displaying the time in the form
        private Timer runningTimer; 
        public Label timerLabel = new Label();

        private int seconds = 0;
        private int minutes = 0;

        // For timing the gif animation
        private Timer gifTimer;

        public double FrameTimerInterval
        {
            get { return frameTimer.Interval; }
            set { frameTimer.Interval = value; }
        }

        public double RunningTimerInterval
        {
            get { return runningTimer.Interval; }
            set { runningTimer.Interval = value; }
        }

        public double GifTimerInterval
        {
            get { return gifTimer.Interval; }
            set { gifTimer.Interval = value; }
        }

        public GameTimer()
        {
            frameTimer = new Timer();

            runningTimer = new Timer();

            gifTimer = new Timer();
        }

        // ----------------- FRAME TIMER ------------------- //

        internal void InitializeFrameTimer()
        {

            animationTimer = new Timer
            {
                Interval = 100 // Set the interval to control the animation speed (in milliseconds).
            };
            animationTimer.Tick += new EventHandler(OnFrameTimerTick);
        }

        private void OnFrameTimerTick(object sender, EventArgs e)
        {
            // Update the background image to the next frame
            currentFrameIndex = (currentFrameIndex + 1) % bgFrames.Count; // Loop through frames
            this.BackgroundImage = bgFrames[currentFrameIndex];
        }

        // ----------------- RUNNING TIMER ------------------- //

        // ----------------- GIF TIMER ------------------- //

        public void StartTimers()
        {
            frameTimer.Start();
            runningTimer.Start();
            gifTimer.Start();
        }

        public void StopTimers()
        {
            frameTimer.Stop();
            runningTimer.Stop();
            gifTimer.Stop();
        }

        private void Timer1Elapsed(object sender, ElapsedEventArgs e)
        {
            // Logic for timer1
            Console.WriteLine("Timer1 elapsed - Player's remaining time.");
        }

        private void Timer2Elapsed(object sender, ElapsedEventArgs e)
        {
            // Logic for timer2
            Console.WriteLine("Timer2 elapsed - Gif animation timing.");
        }

        private void Timer3Elapsed(object sender, ElapsedEventArgs e)
        {
            // Logic for timer3
            Console.WriteLine("Timer3 elapsed - Show the next frame.");
        }
    }
}
