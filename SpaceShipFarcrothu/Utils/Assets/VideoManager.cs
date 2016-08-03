namespace SpaceShipFartrothu.Utils.Assets
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;

    public static class VideoManager
    {
        public static Video Video { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Video = content.Load<Video>("introVideo");
        }
    }
}