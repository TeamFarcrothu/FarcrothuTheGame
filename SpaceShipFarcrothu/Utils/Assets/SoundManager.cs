namespace SpaceShipFartrothu.Utils.Assets
{
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;

    public static class SoundManager
    {
        public static SoundEffect PlayerShootSound { get; private set; }
        public static SoundEffect ExplodeSound { get; private set; }
        public static Song BgMusic { get; private set; }
        public static Song IntroSong { get; private set; }

        public static void LoadContent(ContentManager content)
        {
           PlayerShootSound = content.Load<SoundEffect>("playershoot");
           ExplodeSound = content.Load<SoundEffect>("explode");
           BgMusic = content.Load<Song>("theme");
           IntroSong = content.Load<Song>("intro");
        }
    }
}