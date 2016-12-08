namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using System.Linq;
    using Handlers;
    using GameObjects;
    using Factories;
    using Interfaces;
    using Data;
    using Utils.Assets;
    using Utils.Enums;
    using Utils.Globals;
    using Handlers.Buttons;
    using System.Threading;
    using Services;
    using Handlers.Forms;

    public class GameEngine : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly StarField starfield;
        private readonly HUD hud;

        private SpriteBatch spriteBatch;
        private KeyboardState keyState;
        private VideoPlayer videoPlayer;
        private Texture2D texture;

        private Random random;
        private State gameState;
        private State previousState;

        private Player player;
        private Player player2;
        private readonly InputHandler inputHandler;

        private Boss boss;

        private bool bossHasInstance;
        private bool introPlayed;

        private GameDatabase db;

        private ButtonFactory mainMenuButtons;
        private ButtonFactory pauseMenuButtons;
        private ButtonFactory singleLoginButtons;

        private FormFactory mainMenuForms;
        private FormFactory singleLoginForms;

        private SaveAndLoadDbManager databaseManager;
        private UserDbManager userDbManager;
        private UsersRepository users;

        public GameEngine()
        {
            this.starfield = new StarField();
            this.inputHandler = new InputHandler();
            this.graphics = new GraphicsDeviceManager(this)
            {

                IsFullScreen = false,
                PreferredBackBufferWidth = Globals.MAIN_SCREEN_WIDTH,
                PreferredBackBufferHeight = Globals.MAIN_SCREEN_HEIGHT
            };
            this.IsMouseVisible = true;
            this.Window.Title = "Traveling to FARCROTHU";
            this.Content.RootDirectory = "Content";
            this.mainMenuButtons = new ButtonFactory();
            this.pauseMenuButtons = new ButtonFactory();
            this.singleLoginButtons = new ButtonFactory();
            this.mainMenuForms = new FormFactory();
            this.singleLoginForms = new FormFactory();
            this.databaseManager = new SaveAndLoadDbManager();
            this.userDbManager = new UserDbManager();
            this.users = new UsersRepository();

        }

        protected override void Initialize()
        {
            this.db = new GameDatabase();
            this.gameState = State.Intro;
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.videoPlayer = new VideoPlayer();

            this.random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            TexturesManager.LoadContent(this.Content);
            SoundManager.LoadContent(this.Content);
            VideoManager.LoadContent(this.Content);
            this.mainMenuForms.CreateForm(new Vector2(900, 300), "Username:", true);
            this.mainMenuForms.CreateForm(new Vector2(900, 363), "Password:", false);
            this.mainMenuForms.CreateForm(new Vector2(900, 426), "Confirm Password:", false);
            this.singleLoginForms.CreateForm(new Vector2(900, 300), "Username:", true);
            this.singleLoginForms.CreateForm(new Vector2(900, 363), "Password:", false);
            this.starfield.LoadContent(this.Content);
            MediaPlayer.Play(SoundManager.IntroSong);
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
            this.Exit();
        }

        protected override void Update(GameTime gameTime)
        {
            this.keyState = Keyboard.GetState();

            if (this.keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (this.keyState.IsKeyDown(Keys.OemMinus) && MediaPlayer.Volume >= 0.02f)
            {
                MediaPlayer.Volume -= 0.02f;
            }

            if (this.keyState.IsKeyDown(Keys.OemPlus) && MediaPlayer.Volume <= 0.98f)
            {
                MediaPlayer.Volume += 0.02f;
            }

            if (this.keyState.IsKeyDown(Keys.Space) && this.videoPlayer.State == MediaState.Playing)
            {
                this.videoPlayer.Stop();
            }

            //UPDATING Game state
            switch (this.gameState)
            {
                case State.Intro:
                    this.GetIntroByStateIntro();
                    break;

                case State.OnePlayer:
                    {
                        this.previousState = gameState;
                        this.starfield.Speed = 3;

                        this.Play(gameTime);

                        //Clear dead players
                        this.db.Players.GetAll().RemoveAll(p => p.IsAlive == false);

                        //Set gameover
                        if (this.db.Players.GetAll().All(p => p.IsAlive == false))
                        {
                            this.gameState = State.GameOver;
                        }
                        if (keyState.IsKeyDown(Keys.P))
                        {
                            gameState = State.Pause;
                            break;
                        }

                        //update players
                        this.db.Players.GetAll().ForEach(p => p.Update(gameTime));
                        this.db.Players.GetAll().ForEach(p => p.InputHandler.Move(p));
                        this.db.Players.GetAll().ForEach(p => p.InputHandler.PlayerShoot(this.db.Bullets, this.db.Players, p.Id));

                        this.starfield.Update(gameTime);
                        break;
                    }
                case State.TwoPlayers:
                    {
                        this.previousState = gameState;
                        this.starfield.Speed = 3;

                        this.Play(gameTime);

                        //Clear dead players
                        this.db.Players.GetAll().RemoveAll(p => p.IsAlive == false);

                        //Set gameover
                        if (this.db.Players.GetAll().All(p => p.IsAlive == false))
                        {
                            this.gameState = State.GameOver;
                        }
                        if (keyState.IsKeyDown(Keys.P))
                        {
                            gameState = State.Pause;
                            break;
                        }

                        //update players
                        this.db.Players.GetAll().ForEach(p => p.Update(gameTime));
                        this.db.Players.GetAll().ForEach(p => p.InputHandler.Move(p));
                        this.db.Players.GetAll().ForEach(p => p.InputHandler.PlayerShoot(this.db.Bullets, this.db.Players, p.Id));

                        this.starfield.Update(gameTime);
                        break;
                    }

                //UPDATING MENU STATE

                case State.Menu:

                    mainMenuButtons.CreateButton(new Vector2(500, 300), State.SingleLogInMenu, "Singleplayer Mode");
                    mainMenuButtons.CreateButton(new Vector2(500, 400), State.TwoPlayers, "Multiplayer Mode");
                    mainMenuButtons.CreateButton(new Vector2(500, 500), State.HighScores, "High Scores");
                    mainMenuButtons.CreateButton(new Vector2(900, 500), State.Register, "Register User");

                    MouseState mouse = Mouse.GetState();
                    mainMenuForms.UpdateForms(mouse);
                    gameState = this.mainMenuButtons.ReturnButtonState(mouse, gameState);

                    this.ChoosePlayerModeByStateMenu(gameTime);

                    break;

                case State.Pause:

                    pauseMenuButtons.CreateButton(new Vector2(500, 300), State.SaveGame, "Save game");
                    pauseMenuButtons.CreateButton(new Vector2(500, 400), previousState, "Continue");
                    pauseMenuButtons.CreateButton(new Vector2(500, 500), State.Quit, "Quit");

                    MouseState pauseMouse = Mouse.GetState();
                    gameState = this.pauseMenuButtons.ReturnButtonState(pauseMouse, gameState);
                    this.starfield.Update(gameTime);
                    this.starfield.Speed = 1;
                    break;

                case State.SaveGame:
                    this.databaseManager.SaveGame(db.Players, db.Enemies, db.Asteroids);
                    this.gameState = State.Pause;
                    Thread.Sleep(200);
                    break;

                case State.LoadGame:
                    this.databaseManager.LoadGame(db.Players, db.Enemies, db.Asteroids,
                        inputHandler);
                    gameState = State.TwoPlayers;
                    break;
                //UPDATING GAMEOVER STATE or WINNING STATE
                case State.GameOver:
                case State.Winning:
                    this.keyState = Keyboard.GetState();

                    if (this.keyState.IsKeyDown(Keys.Space))
                    {
                        this.SetupNewGame();
                    }

                    this.starfield.Update(gameTime);
                    break;
                case State.Quit:
                    Thread.Sleep(200);
                    this.SetupNewGame();
                    break;
                case State.Register:

                    MouseState mouseRegister = Mouse.GetState();
                    userDbManager.RegisterUser(this.mainMenuForms.GetAllForms());
                    Thread.Sleep(50);
                    this.gameState = State.Menu;
                    this.mainMenuForms.EraseForms();
                    this.starfield.Update(gameTime);
                    break;

                case State.SingleLogInMenu:
                    MouseState mouseLogin = Mouse.GetState();
                    singleLoginForms.UpdateForms(mouseLogin);
                    singleLoginButtons.CreateButton(new Vector2(900, 426), State.AfterSingleLogInMenu, "LogIn");
                    this.gameState = singleLoginButtons.ReturnButtonState(mouseLogin, gameState);
                    this.starfield.Update(gameTime);
                    break;
                case State.AfterSingleLogInMenu:

                    MouseState mouseAfterLogin = Mouse.GetState();
                    singleLoginButtons.CreateButton(new Vector2(500, 300), State.OnePlayer, "New Game");
                    singleLoginButtons.CreateButton(new Vector2(500, 400), State.LoadGame, "Load Game");
                    this.gameState = singleLoginButtons.ReturnButtonState(mouseAfterLogin, gameState);
                    this.starfield.Update(gameTime);
                    this.ChoosePlayerModeByStateMenu(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void GetIntroByStateIntro()
        {
            if (this.videoPlayer.State == MediaState.Stopped && this.introPlayed == false)
            {
                this.videoPlayer.Play(VideoManager.Video);
                this.introPlayed = true;
            }
            else if (this.videoPlayer.State == MediaState.Stopped && this.introPlayed == true)
            {
                this.gameState = State.Menu;
            }
        }

        private void ChoosePlayerModeByStateMenu(GameTime gameTime)
        {
            this.keyState = Keyboard.GetState();

            //Setup two players game
            if (this.gameState == State.TwoPlayers)
            {
                this.player = new Player(new Vector2(600, 600), this.inputHandler, 1);
                this.db.Players.AddEntity(this.player);

                this.player2 = new Player(new Vector2(700, 600), this.inputHandler, 2);
                this.db.Players.AddEntity(this.player2);

                this.gameState = State.TwoPlayers;
                MediaPlayer.Play(SoundManager.BgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            //Setup single player game
            if (this.gameState == State.OnePlayer)
            {
                this.player = new Player(new Vector2(600, 600), this.inputHandler, 1);
                this.db.Players.AddEntity(this.player);

                this.gameState = State.OnePlayer;
                MediaPlayer.Play(SoundManager.BgMusic);
                //MediaPlayer.Volume = 0.5f;
            }

            this.starfield.Update(gameTime);
            this.starfield.Speed = 1;
        }

        private void SetupNewGame()
        {
            MediaPlayer.Stop();

            this.db.Enemies.Dispose();
            this.db.Asteroids.Dispose();
            this.db.Explosions.Dispose();
            this.db.Players.Dispose();
            this.db.Bullets.Dispose();
            this.db.Items.Dispose();

            this.bossHasInstance = false;
            gameState = State.Menu;
        }

        private void Play(GameTime gameTime)
        {
            //Enable boss mode if some of players have enough points   ## its 150 just for testing
            if (this.db.Players.GetAll().Any(s => s.Score >= 1000))
            {
                this.EnableBossMode(gameTime);
            }
            else
            {
                for (int i = 0; i < this.db.Enemies.GetCount(); i++)
                {
                    BulletsFactory.EnemyShoot(this.db.Bullets, this.db.Enemies.GetAll()[i]);
                }

                //Creating entities
                EnemyFactory.CreateEnemies(this.db.Enemies, this.random);
                AsteroidFactory.CreateAsteroids(this.db.Asteroids, this.random);

                ItemFactory.CreateItems(this.db.Items, this.db.Enemies.GetAll().Cast<IGameObject>().ToList(), this.random);
                ItemFactory.CreateItems(this.db.Items, this.db.Asteroids.GetAll().Cast<IGameObject>().ToList(), this.random);

                // Handle collisions between players and enemy objects
                CollisionHandler.CheckForCollision(this.db.Asteroids.GetAll().Cast<IGameObject>().ToList(), this.db.Players.GetAll(), this.db.Explosions.GetAll());
                CollisionHandler.CheckForCollision(this.db.Enemies.GetAll().Cast<IGameObject>().ToList(), this.db.Players.GetAll(), this.db.Explosions.GetAll());

                // Handle collisions between players and enemy items
                CollisionHandler.CheckPlayerItemCollisions(this.db.Items.GetAll(), this.db.Players.GetAll());

                ExplosionFactory.CreateExplosion(this.db.Explosions, this.db.Enemies.GetAll().Cast<IGameObject>().ToList());
                ExplosionFactory.CreateExplosion(this.db.Explosions, this.db.Asteroids.GetAll().Cast<IGameObject>().ToList());

                //Updating entities
                this.db.Enemies.GetAll().ForEach(e => e.Update(gameTime));
                this.db.Asteroids.GetAll().ForEach(a => a.Update(gameTime));
                this.db.Items.GetAll().ForEach(i => i.Update(gameTime));

                // Cleaning with mr.Proper
                EntityCleanerHandler.ClearEnemies(this.db.Enemies);
                EntityCleanerHandler.ClearAsteroids(this.db.Asteroids);
                EntityCleanerHandler.ClearExplosion(this.db.Explosions);
                EntityCleanerHandler.ClearPlayers(this.db.Players);
            }

            //Update 
            this.db.Bullets.GetAll().ForEach(b => b.Update(gameTime));
            this.db.Explosions.GetAll().ForEach(e => e.Update(gameTime));


            StatsManager.UpdatePlayersStats(this.db.Players.GetAll());

            //Handle collisions between bullets and gameobjects
            CollisionHandler.CheckPlayerBulletsCollisions(this.db.Enemies.GetAll().Cast<IGameObject>().ToList(), this.db.Bullets.GetAll(), this.db.Players.GetAll(), this.db.Explosions.GetAll());
            CollisionHandler.CheckPlayerBulletsCollisions(this.db.Asteroids.GetAll().Cast<IGameObject>().ToList(), this.db.Bullets.GetAll(), this.db.Players.GetAll(), this.db.Explosions.GetAll());
            CollisionHandler.CheckEnemiesBulletsCollisions(this.db.Bullets.GetAll(), this.db.Players.GetAll());

            EntityCleanerHandler.ClearBullets(this.db.Bullets);
        }

        private void EnableBossMode(GameTime gameTime)
        {
            this.boss = Boss.Instance;
            this.bossHasInstance = true;

            if (this.bossHasInstance)
            {
                CollisionHandler.CheckBossBulletsCollisions(this.db.Bullets.GetAll(), this.db.Players.GetAll());
                CollisionHandler.CheckPlayerBulletsCollisions(new List<IGameObject>() { this.boss }, this.db.Bullets.GetAll(), this.db.Players.GetAll(), this.db.Explosions.GetAll());

                BulletsFactory.BossShoot(this.db.Bullets, this.boss);

                this.boss.Update(gameTime);

                if (!this.boss.IsVisible)
                {
                    this.gameState = State.Winning;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();
            switch (this.gameState)
            {
                // DRAWING PLAYING STATE
                case State.TwoPlayers:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        //this.hud.Draw(this.spriteBatch);
                        this.DrawAllGameObjects();

                        if (this.bossHasInstance)
                            this.DrawBoss();

                        break;
                    }
                case State.OnePlayer:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        //this.hud.Draw(this.spriteBatch);
                        this.DrawAllGameObjects();

                        if (this.bossHasInstance)
                            this.DrawBoss();

                        break;
                    }
                // DRAWING MENU STATE
                case State.Menu:
                    {
                        this.DrawStarfield(TexturesManager.MenuImage);
                        this.mainMenuButtons.DrawButtons(this.spriteBatch);
                        this.mainMenuButtons.RemoveAllButtons();
                        this.mainMenuForms.DrawForms(this.spriteBatch);
                        break;
                    }
                case State.Pause:
                    {
                        this.DrawStarfield(TexturesManager.MenuImage);
                        this.pauseMenuButtons.DrawButtons(this.spriteBatch);
                        this.pauseMenuButtons.RemoveAllButtons();
                        break;
                    }
                case State.SingleLogInMenu:
                    this.DrawStarfield(TexturesManager.MenuImage);
                    this.singleLoginButtons.DrawButtons(this.spriteBatch);
                    this.singleLoginForms.DrawForms(this.spriteBatch);
                    this.singleLoginButtons.RemoveAllButtons();
                    break;
                case State.AfterSingleLogInMenu:
                    this.DrawStarfield(TexturesManager.MenuImage);
                    this.singleLoginButtons.DrawButtons(this.spriteBatch);
                    this.singleLoginButtons.RemoveAllButtons();
                    break;
                // DRAWING GAMEOVER STATE
                case State.GameOver:
                    {
                        this.DrawStarfield(TexturesManager.GameoverImage); break;
                    }
                // DRAWING WINNING STATE
                case State.Winning:
                    {
                        this.DrawStarfield(TexturesManager.WinningImage); break;
                    }

                //DRAWING INTRO VIDEO
                case State.Intro:
                    if (this.videoPlayer.State != MediaState.Stopped)
                    {
                        this.texture = this.videoPlayer.GetTexture();
                        this.spriteBatch.Draw(this.texture, this.GraphicsDevice.Viewport.Bounds, Color.White);
                    }
                    break;
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawAllGameObjects()
        {
            this.db.Enemies.GetAll().ForEach(e => e.Draw(this.spriteBatch));
            this.db.Bullets.GetAll().ForEach(b => b.Draw(this.spriteBatch));
            this.db.Asteroids.GetAll().ForEach(a => a.Draw(this.spriteBatch));
            this.db.Explosions.GetAll().ForEach(e => e.Draw(this.spriteBatch));

            foreach (var item in this.db.Items.GetAll())
            {
                item.Draw(this.spriteBatch);
            }
            this.db.Players.GetAll().ForEach(p => p.Draw(this.spriteBatch));
        }

        private void DrawBoss()
        {
            if (this.db.Asteroids.GetAll().Any())
                this.db.Asteroids.Dispose();
            if (this.db.Enemies.GetAll().Any())
                this.db.Enemies.Dispose();
            if (this.db.Items.GetAll().Any())
                this.db.Items.Dispose();

            this.boss.Draw(this.spriteBatch);
        }

        private void DrawStarfield(Texture2D stateImage)
        {
            this.starfield.Draw(this.spriteBatch);
            this.spriteBatch.Draw(stateImage, new Vector2(0, 0), Color.White);
        }
    }
}