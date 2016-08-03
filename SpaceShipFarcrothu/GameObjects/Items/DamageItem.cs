namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Utils.Assets;

    public class DamageItem : Item
    {
        private const int DamageConst = 5;

        public DamageItem(Vector2 position) 
            : base(position)
        {
            this.Texture = TexturesManager.ItemDamageTexture;
            this.ItemDamage = DamageConst;
        }
    }
}