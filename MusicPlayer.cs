using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class MusicPlayer
    {
        SoundPlayer player = new SoundPlayer();

        public MusicPlayer() { }

        internal void PlayTitleMusic()
        {
            player.SoundLocation = "bgmusic.wav";
            player.Play();
        }

        internal void StopTitleMusic()
        {
            player.SoundLocation = "bgmusic.wav";
            player.Stop();
        }

        internal void MiaAtkSFX()
        {
            player.SoundLocation = "AtkBgMusic.wav";
            player.Play();
        }

        internal void PlayGameOver()
        {
            player.SoundLocation = "GameOverSFX.wav";
            player.Play();
        }

        internal void NpcChoice()
        {
            player.SoundLocation = "NpcChoiceClick.wav";
            player.Play();
        }
    }
}