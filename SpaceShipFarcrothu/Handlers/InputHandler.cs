namespace SpaceShipFartrothu.Handlers
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Factories;
    using Interfaces;
    using Utils.Globals;

    public class InputHandler
    {
        public void Move(IPlayer player)
        {
            KeyboardState keyState = Keyboard.GetState();
            // Player movement
            if ((keyState.IsKeyDown(Keys.W) && player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Up) && player.Id == 1))
            {
                player.Position = new Vector2(player.Position.X, player.Position.Y - player.Speed);
            }
            if ((keyState.IsKeyDown(Keys.A) && player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Left) && player.Id == 1))
            {
                player.Position = new Vector2(player.Position.X - player.Speed, player.Position.Y);
            }
            if ((keyState.IsKeyDown(Keys.S) && player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Down) && player.Id == 1))
            {
                player.Position = new Vector2(player.Position.X, player.Position.Y + player.Speed);
            }
            if ((keyState.IsKeyDown(Keys.D) && player.Id == 2) ||
                (keyState.IsKeyDown(Keys.Right) && player.Id == 1))
            {
                player.Position = new Vector2(player.Position.X + player.Speed, player.Position.Y);
            }

            // Moving left and right through screen borders
            if (player.Position.X <= -30 || player.Position.X >= Globals.MAIN_SCREEN_WIDTH)
            {
                if (player.Position.X > Globals.MAIN_SCREEN_WIDTH)
                {
                    player.Position = new Vector2(player.Position.X - Globals.MAIN_SCREEN_WIDTH,
                        player.Position.Y);
                }
                else if (player.Position.X < -30)
                {
                    player.Position = new Vector2(Globals.MAIN_SCREEN_WIDTH, player.Position.Y);
                }
            }

            if (player.Position.Y <= 0)
            {
                player.Position = new Vector2(player.Position.X, 0);
            }
            if (player.Position.Y >= Globals.MAIN_SCREEN_HEIGHT - player.Texture.Height)
            {
                player.Position = new Vector2(player.Position.X,
                    Globals.MAIN_SCREEN_HEIGHT - player.Texture.Height);
            }
        }

        public void PlayerShoot(IRepository<IBullet> bullets, IRepository<IPlayer> players, int playerId)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Player shooting
            if ((keyState.IsKeyDown(Keys.RightControl) && playerId == 2) ||
                (keyState.IsKeyDown(Keys.LeftControl) && playerId == 1))
            {
                //this.Shoot();
                BulletsFactory.PlayerShoot(bullets, players, playerId);
            }
        }
    }
}
