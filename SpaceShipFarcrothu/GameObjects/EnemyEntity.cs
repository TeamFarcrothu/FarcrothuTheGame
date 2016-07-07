namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class EnemyEntity : GameObject
    {
        protected EnemyEntity(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        public int ScorePoints { get; set; }
    }
}
