namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Utils.Assets;

    public class HealthItem : Item
    {
        private const int HealthConst = 30;

        public HealthItem(Vector2 position) 
            : base(position)
        {
            this.Texture = TexturesManager.ItemHealthTexture;
            this.ItemHealth = HealthConst;
        }
    }
}