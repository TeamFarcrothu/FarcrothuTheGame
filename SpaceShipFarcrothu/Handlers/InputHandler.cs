namespace SpaceShipFartrothu.Handlers
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using GameObjects;
    using Microsoft.Xna.Framework.Graphics;
    using Factories;
    using Interfaces;
    using Multimedia;
    using Utils.Globals;

    public class InputHandler
    {
        private Player player;
        // Keyboard state monitoring
        // private KeyboardState keyState = Keyboard.GetState();

        public InputHandler(Player player)
        {
            this.player = player;
        }

        public void Move(KeyboardState keyState)
        {
            // Player movement
            if ((Keyboard.GetState().IsKeyDown(Keys.W) && this.player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Up) && this.player.Id == 1))
            {
                this.player.Position = new Vector2(this.player.Position.X, this.player.Position.Y - this.player.Speed);
            }
            if ((keyState.IsKeyDown(Keys.A) && this.player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Left) && this.player.Id == 1))
            {
                this.player.Position = new Vector2(this.player.Position.X - this.player.Speed, this.player.Position.Y);
            }
            if ((keyState.IsKeyDown(Keys.S) && this.player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Down) && this.player.Id == 1))
            {
                this.player.Position = new Vector2(this.player.Position.X, this.player.Position.Y + this.player.Speed);
            }
            if ((keyState.IsKeyDown(Keys.D) && this.player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Right) && this.player.Id == 1))
            {
                this.player.Position = new Vector2(this.player.Position.X + this.player.Speed, this.player.Position.Y);
            }

            // Moving left and right through screen borders
            if (this.player.Position.X <= -30 || this.player.Position.X >= Globals.MAIN_SCREEN_WIDTH)
            {
                if (this.player.Position.X > Globals.MAIN_SCREEN_WIDTH)
                {
                    this.player.Position = new Vector2(this.player.Position.X - Globals.MAIN_SCREEN_WIDTH,
                        this.player.Position.Y);
                }
                else if (this.player.Position.X < -30)
                {
                    this.player.Position = new Vector2(Globals.MAIN_SCREEN_WIDTH, this.player.Position.Y);
                }
            }

            if (this.player.Position.Y <= 0)
            {
                this.player.Position = new Vector2(this.player.Position.X, 0);
            }
            if (this.player.Position.Y >= Globals.MAIN_SCREEN_HEIGHT - this.player.Texture.Height)
            {
                this.player.Position = new Vector2(this.player.Position.X,
                    Globals.MAIN_SCREEN_HEIGHT - this.player.Texture.Height);
            }
        }

        public void PlayerShoot(KeyboardState keyState, IList<IBullet> bullets, Texture2D bullletTexture, SoundManager soundManager)
        {
            // Player shooting
            if ((keyState.IsKeyDown(Keys.LeftControl) && this.player.Id == 2) ||
                (keyState.IsKeyDown(Keys.LeftControl) && this.player.Id == 1))
            {
                //this.Shoot();
                BulletsFactory.PlayerShoot(bullets, this.player, bullletTexture, soundManager);
            }
        }
    }
}
