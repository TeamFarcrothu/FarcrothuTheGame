namespace SpaceShipFartrothu.Handlers
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using GameObjects;
    using Factories;
    using Interfaces;
    using Utils.Globals;

    public class InputHandler
    {
        private Player player;
        // Keyboard state monitoring
        // private KeyboardState keyState = Keyboard.GetState();

        public InputHandler(Player player)
        {
            this.Player = player;
        }

        public IPlayer Player { get; set; }

        public void Move(KeyboardState keyState)
        {
            // Player movement
            if ((Keyboard.GetState().IsKeyDown(Keys.W) && this.Player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Up) && this.Player.Id == 1))
            {
                this.Player.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y - this.Player.Speed);
            }
            if ((keyState.IsKeyDown(Keys.A) && this.Player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Left) && this.Player.Id == 1))
            {
                this.Player.Position = new Vector2(this.Player.Position.X - this.Player.Speed, this.Player.Position.Y);
            }
            if ((keyState.IsKeyDown(Keys.S) && this.Player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Down) && this.Player.Id == 1))
            {
                this.Player.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y + this.Player.Speed);
            }
            if ((keyState.IsKeyDown(Keys.D) && this.Player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Right) && this.Player.Id == 1))
            {
                this.Player.Position = new Vector2(this.Player.Position.X + this.Player.Speed, this.Player.Position.Y);
            }

            // Moving left and right through screen borders
            if (this.Player.Position.X <= -30 || this.Player.Position.X >= Globals.MAIN_SCREEN_WIDTH)
            {
                if (this.Player.Position.X > Globals.MAIN_SCREEN_WIDTH)
                {
                    this.Player.Position = new Vector2(this.Player.Position.X - Globals.MAIN_SCREEN_WIDTH,
                        this.Player.Position.Y);
                }
                else if (this.Player.Position.X < -30)
                {
                    this.Player.Position = new Vector2(Globals.MAIN_SCREEN_WIDTH, this.Player.Position.Y);
                }
            }

            if (this.Player.Position.Y <= 0)
            {
                this.Player.Position = new Vector2(this.Player.Position.X, 0);
            }
            if (this.Player.Position.Y >= Globals.MAIN_SCREEN_HEIGHT - this.Player.Texture.Height)
            {
                this.Player.Position = new Vector2(this.Player.Position.X,
                    Globals.MAIN_SCREEN_HEIGHT - this.Player.Texture.Height);
            }
        }

        public void PlayerShoot(KeyboardState keyState, IList<IBullet> bullets)
        {
            // Player shooting
            if ((keyState.IsKeyDown(Keys.RightControl) && this.Player.Id == 2) ||
                (keyState.IsKeyDown(Keys.LeftControl) && this.Player.Id == 1))
            {
                //this.Shoot();
                BulletsFactory.PlayerShoot(bullets, this.Player);
            }
        }
    }
}
