namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Utils.Assets;

    public class ArmorItem : Item
    {
        private const int ArmorConst = 1;

        public ArmorItem(Vector2 position) 
            : base(position)
        {
            this.Texture = TexturesManager.ItemArmorTexture;
            this.ItemArmor = ArmorConst;
        }
    }
}
