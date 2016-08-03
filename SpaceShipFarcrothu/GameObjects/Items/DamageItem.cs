namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;

    public class DamageItem : Item
    {
        private const int DamageConst = 5;

        public DamageItem(Vector2 position) 
            : base(position)
        {
            //this.Texture = AssetsLoader.
            this.Damage = DamageConst;
        }
    }
}