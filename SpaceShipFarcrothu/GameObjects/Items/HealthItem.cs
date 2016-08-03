namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;

    public class HealthItem : Item
    {
        private const int HealthConst = 5;

        public HealthItem(Vector2 position)
            : base(position)
        {
            //this.Texture = AssetsLoader.
            this.ItemHealth = HealthConst;
        }
    }
}