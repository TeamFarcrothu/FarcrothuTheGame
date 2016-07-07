using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SpaceShipFartrothu.Multimedia
{
    public class VideoManager
    {
        public Video Video;

        //Constructor
        public VideoManager()
        {
            this.Video = null;
        }

        public void LoadContent(ContentManager content)
        {
            this.Video = content.Load<Video>("introVideo");
        }
    }
}