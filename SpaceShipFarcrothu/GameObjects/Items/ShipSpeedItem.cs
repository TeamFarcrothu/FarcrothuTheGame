namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using SpaceShipFartrothu.Utils.Assets;

    public class ShipSpeedItem : Item
    {
        private const int DefaultSpeed = 2;

        public ShipSpeedItem(Vector2 position) : base(position)
        {
            this.Texture = TexturesManager.ItemShipSpeedTexture;
            this.ItemShipSpeed = DefaultSpeed;
        }
    }
}
