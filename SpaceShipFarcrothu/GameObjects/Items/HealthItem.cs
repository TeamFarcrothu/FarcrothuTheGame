namespace SpaceShipFartrothu.GameObjects.Items
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class HealthItem : Item
    {
        private const int HealthPower = 5; // This can be changed later.

        /* By setting damage to ZERO, we make sure not to increase player damage
           when adding health item */
        private const int ZeroDamagePower = 0;

        public HealthItem(Texture2D texture, Vector2 position)
            : base(texture, position, HealthPower, ZeroDamagePower)
        {
        }
    }
}