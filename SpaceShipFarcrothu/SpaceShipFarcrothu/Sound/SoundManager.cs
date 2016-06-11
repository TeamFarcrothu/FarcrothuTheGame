using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShipFartrothu.Sound
{

    public class SoundManager
    {
        public SoundEffect playerShootSound;
        public SoundEffect explodeSound;
        public Song bgMusic;


        //Constructor
        public SoundManager()
        {
            playerShootSound = null;
            explodeSound = null;
            bgMusic = null;
        }

        public void LoadContent(ContentManager content)
        {
            //Content.RootDirectory = "Content";
            playerShootSound = content.Load<SoundEffect>("playershoot");
            //TODO once shooting is ready:
            //explodeSound = Content.Load<SoundEffect>("explode");
            bgMusic = content.Load<Song>("theme");
        }
    }
}