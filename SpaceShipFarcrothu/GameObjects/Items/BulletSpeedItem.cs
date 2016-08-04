namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Utils.Assets;

    public class BulletSpeedItem : Item
    {
        private const int BulletSpeedConst = 2;

        public BulletSpeedItem(Vector2 position) : base(position)
        {
            this.Texture = TexturesManager.ItemBulletSpeedTexture;
            this.ItemBulletSpeed = BulletSpeedConst;
        }
    }
}
