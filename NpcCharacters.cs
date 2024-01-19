using ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ANDOSAY_ARGONZA_VILLAMANTE_Etherea_Game
{
    internal class NpcCharacters
    {
        private readonly MiaCharacter mia;

        // Store NPC GIF Images in this picturebox
        internal PictureBox NpcState;

        internal PictureBox dialogueBox;

        public string[] goodOrBadNPC = { "Good NPC", "Bad NPC" };
        public string[] Advantages = { "Lessen Time", "Regain Health", "Give Bow Weapon" };
        public string[] Consequences = { "Lessen Health -10", "Add Time +10", "Take Weapon" };


        internal static Button choice1, choice2, choice3;

        internal Button[] btnName = { choice1, choice2, choice3 };
        internal string[] btnText = { "Choice A", "Choice B", "Choice C" };
        internal Size[] btnSize = { new Size(150, 25), new Size(150, 25), new Size(150, 25) };
        internal Point[] btnLoc = { new Point(62, 205), new Point(220, 205), new Point(378, 205) };

        //private bool isGoodNPC;
        //private bool isBadNPC;

        public NpcCharacters() //bool isGoodNPC
        {
            mia = new MiaCharacter();

            InitializeNPCCharacter();
            NPCDialogue();
        }

        internal PictureBox GetNpcPictureBox()
        {
            return NpcState;
        }

        // Get-Set the X-position of enemy
        internal int GetNpcX() // Get the X-position value of the npc
        {
            return NpcState.Location.X;
        }
        public void SetNpcX(int newnpcX) // Set the X-position value of the npc
        {
            NpcState.Location = new Point(newnpcX, NpcState.Location.Y);
        }

        internal PictureBox GetDialogueBox()
        {
            return dialogueBox;
        }
        private void InitializeNPCCharacter() // Initialization of Character Mia
        {
            NpcState = new PictureBox
            {
                Size = new Size(240, 250),
                Location = new Point(595, 240),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Resources.npc_talk,
                BackColor = Color.Transparent,
            };

        }

        internal void NPCDialogue()
        {
            dialogueBox = new PictureBox()
            {
                Size = new Size(600, 200),
                Location = new Point(-10, 35),
                Image = Resources.dialogueBox,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,

            };
        }
    }
}
