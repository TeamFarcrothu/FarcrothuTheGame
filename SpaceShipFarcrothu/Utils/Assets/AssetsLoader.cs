namespace SpaceShipFartrothu.Utils.Assets
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class AssetsLoader
    {
        private readonly ContentManager content;

        public AssetsLoader(ContentManager content)
        {
            this.content = content;
            this.LoadAllAssets();
        }

        public Texture2D MenuImage { get; private set; }
        public Texture2D GameoverImage { get; private set; }
        public Texture2D WinningImage { get; private set; }
        public Texture2D Player1Texture { get; private set; }
        public Texture2D Player2Texture { get; private set; }

        public Texture2D ExplosionTexture { get; private set; }
        public Texture2D BulletTexture { get; private set; }
        public Texture2D AsteroidTexture { get; private set; }
        public Texture2D EnemyTexture { get; private set; }

        public Texture2D ItemTexture { get; private set; }
        public Texture2D BossTexture { get; private set; }
        public Texture2D HealthTexture { get; private set; }

        public void LoadAllAssets()
        {
            this.MenuImage = this.content.Load<Texture2D>("menu_image");
            this.GameoverImage = this.content.Load<Texture2D>("gameover_image");
            this.WinningImage = this.content.Load<Texture2D>("winning_image");
            this.Player1Texture = this.content.Load<Texture2D>("ship_p1");
            this.Player2Texture = this.content.Load<Texture2D>("ship_p2");
            this.AsteroidTexture = this.content.Load<Texture2D>("asteroid");
            this.ExplosionTexture = this.content.Load<Texture2D>("explosion");
            this.BulletTexture = this.content.Load<Texture2D>("bullet");

            this.ItemTexture = this.content.Load<Texture2D>("health_potion");//---------------------------------

            this.BossTexture = this.content.Load<Texture2D>("space_Boss_Level_1");
            this.HealthTexture = this.content.Load<Texture2D>("healthbar");
            this.EnemyTexture = this.content.Load<Texture2D>("enemy_ship");
        }
    }
}
