namespace SpaceShipFartrothu.Core
{
    using Microsoft.Xna.Framework.Graphics;
    using Interfaces;

    public interface IHUD
    {
        int HealthBarSize { get; set; }
        int PlayerDamage { get; set; }
        int PlayerHealth { get; set; }
        int PlayerId { get; }
        int PlayerLevel { get; set; }
        int PlayerScore { get; set; }

        void Draw(SpriteBatch spriteBatch);
        void Update(IPlayer player);
    }
}