using System.Windows.Forms;

namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Play Music
            MusicPlayer mplayer = new MusicPlayer();
            mplayer.PlayTitleMusic();

            StagePlay stage = new StagePlay();
            MainMenu mainMenu = new MainMenu();
            Application.Run(mainMenu);
        }
    }
}
