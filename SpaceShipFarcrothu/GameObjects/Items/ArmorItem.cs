namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;

    public class ArmorItem : Item
    {
        private const int ArmorConst = 5;

        public ArmorItem(Vector2 position)
            : base(position)
        {
            //this.Texture = AssetsLoader.
            this.ItemArmor = ArmorConst;
        }
    }
}
