using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShipFartrothu.Factories;
using SpaceShipFartrothu.Utils.Enums;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpaceShipFartrothu.GameObjects;
using SpaceShipFartrothu.Data;
using SpaceShipFartrothu.Interfaces;
using SpaceShipFartrothu.Services;
using System.Threading;

namespace SpaceShipFartrothu.Handlers
{
    public class UpdateStateManager
    {
        internal State UpdateMenuState(ButtonFactory mainMenuButtons, FormFactory mainMenuForms, State gameState)
        {
            mainMenuButtons.CreateButton(new Vector2(500, 300), State.SingleLogInMenu, "Singleplayer Mode");
            mainMenuButtons.CreateButton(new Vector2(500, 400), State.TwoPlayers, "Multiplayer Mode");
            mainMenuButtons.CreateButton(new Vector2(500, 500), State.HighScores, "High Scores");
            mainMenuButtons.CreateButton(new Vector2(900, 500), State.Register, "Register User");

            MouseState mouse = Mouse.GetState();
            mainMenuForms.UpdateForms(mouse);
            return mainMenuButtons.ReturnButtonState(mouse, gameState);
        }

        internal State UpdatePauseMenu(ButtonFactory pauseMenuButtons, State previousState, State gameState, 
            StarField starfield, GameTime gameTime)
        {
            pauseMenuButtons.CreateButton(new Vector2(500, 300), State.SaveGame, "Save game");
            pauseMenuButtons.CreateButton(new Vector2(500, 400), previousState, "Continue");
            pauseMenuButtons.CreateButton(new Vector2(500, 500), State.Quit, "Quit");

            MouseState pauseMouse = Mouse.GetState();
            starfield.Update(gameTime);
            starfield.Speed = 1;
            return pauseMenuButtons.ReturnButtonState(pauseMouse, gameState);

        }

        internal State UpdateSaveState(SaveAndLoadDbManager databaseManager, IRepository<IPlayer> players, 
            IRepository<IEnemy> enemies, IRepository<IAsteroid> asteroids, UsersRepository usersRepository)
        {
            databaseManager.SaveGame(players, enemies, asteroids, usersRepository);
            Thread.Sleep(200);
            return State.Pause;
        }

        internal State UpdateRegisterState(UserDbManager userDbManager, FormFactory mainMenuForms, StarField starfield, GameTime gameTime)
        {
            MouseState mouseRegister = Mouse.GetState();
            userDbManager.RegisterUser(mainMenuForms.GetAllForms());
            Thread.Sleep(50);
            starfield.Update(gameTime);
            mainMenuForms.EraseForms();
            return State.Menu;
        }

        internal State UpdateSingleLoginMenu(FormFactory singleLoginForms, ButtonFactory singleLoginButtons, 
            State gameState, UsersRepository usersRepository, StarField starfield, GameTime gameTime)
        {
            MouseState mouseLogin = Mouse.GetState();
            singleLoginForms.UpdateForms(mouseLogin);
            singleLoginButtons.CreateButton(new Vector2(900, 426), State.SingleLoadGameMenu, "LogIn");
            gameState = singleLoginButtons.ReturnButtonState(mouseLogin, gameState);
            if (gameState == State.SingleLoadGameMenu)
            {
                usersRepository.AddUser(singleLoginForms);
            }

            starfield.Update(gameTime);

            return gameState;
        }

        internal State UpdateSingleLoadGameMenu(ButtonFactory singleLoginButtons, StarField starfield, 
            State gameState, GameTime gameTime)
        {
            MouseState mouseAfterLogin = Mouse.GetState();
            singleLoginButtons.CreateButton(new Vector2(500, 300), State.OnePlayer, "New Game");
            singleLoginButtons.CreateButton(new Vector2(500, 400), State.LoadGameMenu, "Load Game");
            starfield.Update(gameTime);
            return singleLoginButtons.ReturnButtonState(mouseAfterLogin, gameState);
        }

        internal State UpdateLoadGameMenu(LoadButtonFactory loadButtons, SaveAndLoadDbManager databaseManager, UsersRepository usersRepository, InputHandler inputHandler, IRepository<IPlayer> players, IRepository<IEnemy> enemies, IRepository<IAsteroid> asteroids, StarField starfield, GameTime gameTime)
        {
            int gameId = 0;
            MouseState mouseLoadGame = Mouse.GetState();
            loadButtons.CreateButtons(databaseManager.GetUserGamesId(usersRepository.GetAll()));
            gameId = loadButtons.ReturnGameId(mouseLoadGame);
            starfield.Update(gameTime);
            return databaseManager.LoadGame(players, enemies, asteroids,
                inputHandler, usersRepository, gameId);
        }

        internal State UpdatePlayerState(IRepository<IPlayer> players, IRepository<IBullet> bullets, 
            StarField starfield, GameTime gameTime, 
            State gameState, KeyboardState keyState)
        {
            starfield.Speed = 3;
            State state = gameState;
            //Clear dead players
            players.GetAll().RemoveAll(p => p.IsAlive == false);
            //Set gameover
            if (players.GetAll().All(p => p.IsAlive == false))
            {
                state = State.GameOver;
            }
            if (keyState.IsKeyDown(Keys.P))
            {
                state = State.Pause;
            }
            //update players
            players.GetAll().ForEach(p => p.Update(gameTime));
            players.GetAll().ForEach(p => p.InputHandler.Move(p));
            players.GetAll().ForEach(p => p.InputHandler.PlayerShoot(bullets, players, p.Id));
            starfield.Update(gameTime);

            return state;
        }
    }
}
