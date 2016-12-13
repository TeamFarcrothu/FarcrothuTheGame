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
    using GameUsers;

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
        private StatsManager statsManager;
        private UpdateStateManager updateStateManager;
        private DrawGameObjectsManager drawObjectsManager;
        private PlayGameManager playManager;
        private readonly InputHandler inputHandler;

        private Boss boss;

        private bool bossHasInstance;
        private bool introPlayed;

        private GameDatabase db;

        private ButtonFactory mainMenuButtons;
        private ButtonFactory pauseMenuButtons;
        private ButtonFactory singleLoginButtons;
        private LoadButtonFactory loadButtons;

        private FormFactory mainMenuForms;
        private FormFactory singleLoginForms;

        private SaveAndLoadDbManager databaseManager;
        private UserDbManager userDbManager;
        private UsersRepository usersRepository;

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
            this.loadButtons = new LoadButtonFactory(new Vector2(900, 300));
            this.mainMenuForms = new FormFactory();
            this.singleLoginForms = new FormFactory();
            this.databaseManager = new SaveAndLoadDbManager();
            this.userDbManager = new UserDbManager();
            this.usersRepository = new UsersRepository();
            this.updateStateManager = new UpdateStateManager();
            this.drawObjectsManager = new DrawGameObjectsManager();
            this.playManager = new PlayGameManager();
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
                        this.Play(gameTime);
                        this.gameState = this.updateStateManager.UpdatePlayerState(this.db.Players, 
                            this.db.Bullets, this.starfield,
                            gameTime, this.gameState, this.keyState);
                        break;
                    }
                case State.TwoPlayers:
                    {
                        this.previousState = gameState;
                        this.Play(gameTime);
                        this.gameState = this.updateStateManager.UpdatePlayerState(this.db.Players,
                            this.db.Bullets, this.starfield,
                            gameTime, this.gameState, this.keyState);
                        break;
                    }

                case State.Menu:

                    gameState = this.updateStateManager.UpdateMenuState(this.mainMenuButtons, this.mainMenuForms, gameState);
                    this.ChoosePlayerModeByStateMenu(gameTime);
                    break;

                case State.Pause:

                    this.gameState = this.updateStateManager.UpdatePauseMenu
                        (this.pauseMenuButtons, this.previousState, gameState, starfield, gameTime);

                    break;

                case State.SaveGame:
                    
                    this.gameState = this.updateStateManager.UpdateSaveState(this.databaseManager, this.db.Players,
                        this.db.Enemies, this.db.Asteroids, this.usersRepository);
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
                    this.gameState = this.updateStateManager.UpdateRegisterState
                        (this.userDbManager, this.mainMenuForms, this.starfield, gameTime);
                    break;

                case State.SingleLogInMenu:

                    this.gameState = this.updateStateManager.UpdateSingleLoginMenu(this.singleLoginForms, this.singleLoginButtons,
                        this.gameState, this.usersRepository, this.starfield, gameTime);
                    break;
                case State.SingleLoadGameMenu:

                    this.gameState = this.updateStateManager.UpdateSingleLoadGameMenu
                        (this.singleLoginButtons, this.starfield, this.gameState, gameTime);
                    this.ChoosePlayerModeByStateMenu(gameTime);
                    break;
                case State.LoadGameMenu:

                    this.gameState = this.updateStateManager.UpdateLoadGameMenu(this.loadButtons, this.databaseManager,
                        this.usersRepository, this.inputHandler, db.Players, db.Enemies, db.Asteroids,
                        this.starfield, gameTime);
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
            this.statsManager = new StatsManager();
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
            this.statsManager = new StatsManager();
            this.usersRepository = new UsersRepository();

            this.bossHasInstance = false;
            gameState = State.Menu;
        }

        private void Play(GameTime gameTime)
        {
            this.gameState = this.playManager.Play(this.db, this.gameState, this.boss, this.bossHasInstance, this.statsManager,
                gameTime, this.random);
            //Enable boss mode if some of players have enough points   ## its 150 just for testing
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
                        this.drawObjectsManager.DrawAllGameObjects(this.db, this.spriteBatch, this.starfield);
                        if (this.bossHasInstance)
                            this.DrawBoss();
                        break;
                    }
                case State.OnePlayer:
                    {
                        this.drawObjectsManager.DrawAllGameObjects(this.db, this.spriteBatch, this.starfield);

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
                case State.SingleLoadGameMenu:
                    this.DrawStarfield(TexturesManager.MenuImage);
                    this.singleLoginButtons.DrawButtons(this.spriteBatch);
                    this.singleLoginButtons.RemoveAllButtons();
                    break;
                case State.LoadGameMenu:
                    this.DrawStarfield(TexturesManager.MenuImage);
                    this.loadButtons.DrawButtons(this.spriteBatch);
                    this.loadButtons.RemoveAllButtons();
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