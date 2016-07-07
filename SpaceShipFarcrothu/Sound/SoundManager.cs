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
            this.playerShootSound = null;
            this.explodeSound = null;
            this.bgMusic = null;
            this.intro = null;
        }

        public void LoadContent(ContentManager content)
        {
            this.playerShootSound = content.Load<SoundEffect>("playershoot");
            this.explodeSound = content.Load<SoundEffect>("explode");
            this.bgMusic = content.Load<Song>("theme");
            this.intro = content.Load<Song>("intro");
        }
    }
}