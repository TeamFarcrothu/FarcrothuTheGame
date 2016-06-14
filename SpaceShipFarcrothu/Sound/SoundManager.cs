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
        public Song intro;


        //Constructor
        public SoundManager()
        {
            playerShootSound = null;
            explodeSound = null;
            bgMusic = null;
            intro = null;
        }

        public void LoadContent(ContentManager content)
        {
            playerShootSound = content.Load<SoundEffect>("playershoot");
            explodeSound = content.Load<SoundEffect>("explode");
            bgMusic = content.Load<Song>("theme");
            intro = content.Load<Song>("intro");
        }
    }
}