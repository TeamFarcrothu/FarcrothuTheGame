namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using SpaceShipFartrothu.Utils.Assets;

    public class BulletSpeedItem : Item
    {
        private const int BulletSpeedConst = 5;

        public BulletSpeedItem(Vector2 position)
            : base(position)
        {
            //this.Texture = AssetsLoader.
            this.ItemBulletSpeed = BulletSpeedConst;
        }
    }
}
