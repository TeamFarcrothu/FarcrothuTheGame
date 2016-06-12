using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceShipFartrothu.Core;
using SpaceShipFartrothu.Effects;
using SpaceShipFartrothu.GameObjects;
using SpaceShipFartrothu.Players;
using SpaceShipFartrothu.Sound;

namespace SpaceShipFartrothu.Core
{
    public class GameEngine : Game
    {
        
        //Set first state 
        private State gameState = State.Menu;

        private SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;
        private readonly Random random = new Random();
        //private readonly Player player = new Player();
        private readonly PlayerNew player = new PlayerNew("ship_p1", new Vector2(600, 600), 1);
        //private readonly Player2 player2 = new Player2();
        private readonly PlayerNew player2 = new PlayerNew("ship_p2", new Vector2(700, 600), 2);
        private readonly StarField starfield = new StarField();
        private readonly HUD hud = new HUD();

        private readonly List<Asteroid> asteroids = new List<Asteroid>();
        private readonly List<Enemy> enemyList = new List<Enemy>();        
        private readonly List<Explosion> explosionList = new List<Explosion>();

        private Boss boss;
        private bool bossHasInstance;

        private int enemyBulletDamage;
        private int bossBulletDamage;
        private Texture2D menuImage;

        SoundManager sm = new SoundManager();

        public GameEngine()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1366,
                PreferredBackBufferHeight = 768
            };

            this.Window.Title = "Traveling to FARCROTHU";
            this.Content.RootDirectory = "Content";
            this.enemyBulletDamage = 10;
            this.bossBulletDamage = 30;
            this.menuImage = null;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.hud.LoadContent(this.Content);
            this.player2.LoadContent(this.Content);
            this.player.LoadContent(this.Content);
            this.starfield.LoadContent(this.Content);
            sm.LoadContent(Content);
            this.menuImage = Content.Load<Texture2D>("menu");
            MediaPlayer.Play(sm.bgMusic);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //UPDATING Game state
            switch (gameState)
            {
                case State.Playing:
                    {
                        this.starfield.Speed = 3;

                        //// boss logic
                        //if (this.hud.playerscore >= 10 || this.hud.player2score >= 10)
                        //{
                        //    this.boss.Update(gameTime);
                        //}

                        // if one of the players is alive, keep going
                        if (this.player.isAlive || this.player2.isAlive)
                        {
                            if (this.hud.playerscore >= 10 || this.hud.player2score >= 10)
                            {
                                this.LoadBoss();
                                //  this.boss.Update(gameTime);

                                if (this.boss.boundingBox.Intersects(this.player.boundingBox))
                                {
                                    this.player.health -= 50;
                                    // this.hud.playerscore += 20;
                                    //this.boss.isVisible = false;
                                }

                                if (this.boss.boundingBox.Intersects(this.player2.boundingBox))
                                {
                                    this.player2.health -= 50;
                                    //this.hud.player2score += 20;
                                    //this.boss.isVisible = false;
                                }

                                foreach (Bullet bullet in this.boss.bulletList)
                                {
                                    if (this.player.boundingBox.Intersects(bullet.BoundingBox))
                                    {
                                        this.player.health -= this.enemyBulletDamage;
                                        bullet.IsVisible = false;
                                    }
                                    if (this.player2.boundingBox.Intersects(bullet.BoundingBox))
                                    {
                                        this.player2.health -= this.enemyBulletDamage;
                                        bullet.IsVisible = false;
                                    }
                                }

                                // player bullet lists colliding with boss ship
                                for (int i = 0; i < this.player.bulletList.Count; i++)
                                {
                                    if (this.player.bulletList[i].BoundingBox.Intersects(this.boss.boundingBox))
                                    {
                                        sm.explodeSound.Play();
                                        this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                                            new Vector2(this.boss.position.X + this.boss.texture.Height / 2, this.boss.position.Y + this.boss.texture.Width / 2)));
                                        // this.hud.playerscore += 20;
                                        this.player.bulletList[i].IsVisible = false;
                                        this.boss.health -= 2;
                                        //this.boss.isVisible = false;
                                    }
                                }

                                for (int i = 0; i < this.player2.bulletList.Count; i++)
                                {
                                    if (this.player2.bulletList[i].BoundingBox.Intersects(this.boss.boundingBox))
                                    {
                                        this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                                            new Vector2(this.boss.position.X + boss.texture.Height / 2, this.boss.position.Y + this.boss.texture.Width / 2)));
                                        // this.hud.player2score += 20;
                                        this.player2.bulletList[i].IsVisible = false;
                                        this.boss.health -= 2;
                                        // this.boss.isVisible = false;
                                    }
                                }

                                this.boss.Update(gameTime);

                                foreach (var explosion in this.explosionList)
                                {
                                    explosion.Update(gameTime);
                                }

                            }
                            else
                            {
                                foreach (Enemy enemy in this.enemyList)
                                {
                                    if (enemy.boundingBox.Intersects(this.player.boundingBox))
                                    {
                                        this.player.health -= 40;
                                        this.hud.playerscore += 20;
                                        enemy.isVisible = false;
                                    }

                                    if (enemy.boundingBox.Intersects(this.player2.boundingBox))
                                    {
                                        this.player2.health -= 40;
                                        this.hud.player2score += 20;
                                        enemy.isVisible = false;
                                    }

                                    for (int i = 0; i < enemy.bulletList.Count; i++)
                                    {
                                        if (this.player.boundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                                        {
                                            this.player.health -= this.enemyBulletDamage;
                                            enemy.bulletList[i].IsVisible = false;
                                        }
                                        if (this.player2.boundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                                        {
                                            this.player2.health -= this.enemyBulletDamage;
                                            enemy.bulletList[i].IsVisible = false;
                                        }
                                    }

                                    // player bullet lists colliding with enemy ships
                                    for (int i = 0; i < this.player.bulletList.Count; i++)
                                    {
                                        if (this.player.bulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                                        {
                                            sm.explodeSound.Play();
                                            this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                                                new Vector2(enemy.position.X, enemy.position.Y)));
                                            this.hud.playerscore += 20;
                                            this.player.bulletList[i].IsVisible = false;
                                            enemy.isVisible = false;
                                        }
                                    }

                                    for (int i = 0; i < this.player2.bulletList.Count; i++)
                                    {
                                        if (this.player2.bulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                                        {
                                            this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                                                new Vector2(enemy.position.X, enemy.position.Y)));
                                            this.hud.player2score += 20;
                                            this.player2.bulletList[i].IsVisible = false;
                                            enemy.isVisible = false;
                                        }
                                    }
                                    enemy.Update(gameTime);
                                }

                                foreach (var explosion in this.explosionList)
                                {
                                    explosion.Update(gameTime);
                                }

                                foreach (var asteroid in this.asteroids)
                                {
                                    if (asteroid.BoundingBox.Intersects(this.player.boundingBox))
                                    {
                                        this.hud.playerscore += 5;
                                        asteroid.IsVisible = false;
                                        this.player.health -= 20;
                                    }

                                    if (asteroid.BoundingBox.Intersects(this.player2.boundingBox))
                                    {
                                        asteroid.IsVisible = false;
                                        this.player2.health -= 20;
                                        this.hud.player2score += 5;
                                    }

                                    foreach (var bullet in this.player.bulletList)
                                    {
                                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                                        {
                                            this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                                                new Vector2(asteroid.Position.X, asteroid.Position.Y)));
                                            this.hud.playerscore += 5;
                                            asteroid.IsVisible = false;
                                            bullet.IsVisible = false;
                                        }
                                    }

                                    foreach (var bullet in this.player2.bulletList)
                                    {
                                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                                        {
                                            this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                                                new Vector2(asteroid.Position.X, asteroid.Position.Y)));
                                            this.hud.player2score += 5;
                                            asteroid.IsVisible = false;
                                            bullet.IsVisible = false;
                                        }
                                    }

                                    asteroid.Update(gameTime);
                                }

                                this.LoadAsteroids();
                                this.LoadEnemies();
                            }

                            this.hud.Update(gameTime);
                            this.player2.Update(gameTime);
                            this.player.Update(gameTime);
                            this.starfield.Update(gameTime);
                            this.ManageExplosions();
                            //this.LoadAsteroids();
                            //this.LoadEnemies();
                            //  this.LoadBoss();
                            //[end of] if one of the players is alive, keep going
                        }
                    }
                    break;
                //UPDATING MENU STATE
                case State.Menu:
                    {
                        //Get keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }

                        this.starfield.Update(gameTime);
                        this.starfield.Speed = 1;
                        break;
                    }

                //UPDATING GAMEOVER STATE
                case State.GameOver:
                    {

                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void LoadBoss()
        {
            // Singleton
            if (!this.bossHasInstance)
            {
                this.boss = new Boss(
                    this.Content.Load<Texture2D>("space_Boss_level_1"),
                    new Vector2(501, -600),
                    this.Content.Load<Texture2D>("bullet"));

                this.bossHasInstance = true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            switch (gameState)
            {
                // DRAWING PLAYING STATE
                case State.Playing:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        this.player.Draw(this.spriteBatch);
                        this.player2.Draw(this.spriteBatch);

                        //Update explosions
                        foreach (var explosion in this.explosionList)
                        {
                            explosion.Draw(this.spriteBatch);
                        }

                        //Update asteroids and check for collisions
                        foreach (var asteroid in this.asteroids)
                        {
                            if (this.boss == null || !this.boss.isVisible)
                            {
                                asteroid.Draw(this.spriteBatch);
                            }
                        }

                        foreach (Enemy enemy in this.enemyList)
                        {
                            if (this.boss == null || !this.boss.isVisible)
                            {
                                enemy.Draw(this.spriteBatch);
                            }
                        }

                        this.hud.Draw(this.spriteBatch);

                        if (this.bossHasInstance && this.boss.isVisible)
                        {
                            this.boss.Draw(this.spriteBatch);
                        }

                        break;
                    }
                // DRAWING MENU STATE
                case State.Menu:
                    {
                        this.starfield.Draw(spriteBatch);
                        this.spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);

                        break;
                    }
                // DRAWING GAMEOVER STATE
                case State.GameOver:
                    {
                        break;
                    }

            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        public void LoadAsteroids()
        {
            var newRand = new Random();
            int randomX = this.random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = this.random.Next(-700, -50) + newRand.Next(0, 100);

            if (this.asteroids.Count < 15)
            {
                this.asteroids.Add(new Asteroid(
                    this.Content.Load<Texture2D>("asteroid"),
                    new Vector2(randomX, randomY)));
            }

            for (int i = 0; i < this.asteroids.Count; i++)
            {
                if (!this.asteroids[i].IsVisible)
                {
                    this.asteroids.RemoveAt(i);
                    i--;
                }
            }

        }

        public void LoadEnemies()
        {
            var newRand = new Random();
            int randomX = this.random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = this.random.Next(-700, -50) + newRand.Next(0, 100);

            if (this.enemyList.Count < 3)
            {
                this.enemyList.Add(new Enemy(
                    this.Content.Load<Texture2D>("enemy_ship"),
                    new Vector2(randomX, randomY),
                    this.Content.Load<Texture2D>("bullet")));
            }

            for (int i = 0; i < this.enemyList.Count; i++)
            {
                if (!this.enemyList[i].isVisible)
                {
                    this.enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ManageExplosions()
        {
            for (int i = 0; i < this.explosionList.Count; i++)
            {
                if (!this.explosionList[i].isVisible)
                {
                    this.explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}