namespace SpaceShipFartrothu.Utils.Assets
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public static class TexturesManager
    {
        public static Texture2D ButtonTexture { get; private set; }
        public static Texture2D MenuImage { get; private set; }
        public static Texture2D GameoverImage { get; private set; }
        public static Texture2D WinningImage { get; private set; }
        public static Texture2D Player1Texture { get; private set; }
        public static Texture2D Player2Texture { get; private set; }

        public static Texture2D ExplosionTexture { get; private set; }
        public static Texture2D BulletTexture { get; private set; }
        public static Texture2D AsteroidTexture { get; private set; }
        public static Texture2D EnemyTexture { get; private set; }

        public static SpriteFont PlayerScoreFont { get; private set; }

        public static Texture2D ItemArmorTexture { get; private set; }
        public static Texture2D ItemBulletSpeedTexture { get; private set; }
        public static Texture2D ItemDamageTexture { get; private set; }
        public static Texture2D ItemHealthTexture { get; private set; }
        public static Texture2D ItemShipSpeedTexture { get; private set; }

        public static Texture2D BossTexture { get; private set; }
        public static Texture2D HealthTexture { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            MenuImage = content.Load<Texture2D>("menu_image");
            GameoverImage = content.Load<Texture2D>("gameover_image");
            WinningImage = content.Load<Texture2D>("winning_image");
            Player1Texture = content.Load<Texture2D>("ship_p1");
            Player2Texture = content.Load<Texture2D>("ship_p2");
            AsteroidTexture = content.Load<Texture2D>("asteroid");
            ExplosionTexture = content.Load<Texture2D>("explosion");
            BulletTexture = content.Load<Texture2D>("bullet");
            ButtonTexture = content.Load<Texture2D>("button");


            PlayerScoreFont = content.Load<SpriteFont>("georgia");

            ItemHealthTexture = content.Load<Texture2D>("health_potion");//---------------------------------
            ItemArmorTexture = content.Load<Texture2D>("armor_potion");//---------------------------------
            ItemDamageTexture = content.Load<Texture2D>("damage_potion");//---------------------------------
            ItemBulletSpeedTexture = content.Load<Texture2D>("bullet_speed_potion");//---------------------------------
            ItemShipSpeedTexture = content.Load<Texture2D>("ship_speed_potion");

            BossTexture = content.Load<Texture2D>("space_Boss_Level_1");
            HealthTexture = content.Load<Texture2D>("healthbar");
            EnemyTexture = content.Load<Texture2D>("enemy_ship");
        }
    }
}
