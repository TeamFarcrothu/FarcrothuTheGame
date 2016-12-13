using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShipFartrothu.Data;
using SpaceShipFartrothu.Utils.Enums;
using SpaceShipFartrothu.GameObjects;
using SpaceShipFartrothu.Interfaces;
using SpaceShipFartrothu.Factories;

namespace SpaceShipFartrothu.Handlers
{
    public class PlayGameManager
    {
        internal State EnableBossMode(Boss boss, bool bossHasInstance, GameDatabase db, State gameState, GameTime gameTime)
        {
            State state = gameState;
            boss = Boss.Instance;
            bossHasInstance = true;

            if (bossHasInstance)
            {
                CollisionHandler.CheckBossBulletsCollisions(db.Bullets.GetAll(), db.Players.GetAll());
                CollisionHandler.CheckPlayerBulletsCollisions(new List<IGameObject>() { boss }, db.Bullets.GetAll(), 
                    db.Players.GetAll(), db.Explosions.GetAll());

                BulletsFactory.BossShoot(db.Bullets, boss);

                boss.Update(gameTime);

                if (!boss.IsVisible)
                {
                    state = State.Winning;
                }
            }
            return state;
        }

        internal State Play(GameDatabase db, State gameState, Boss boss, bool bossHasInstance, 
            StatsManager statsManager, GameTime gameTime, Random random)
        {
            State state = gameState;
            if (db.Players.GetAll().Any(s => s.Score >= 10000))
            {
                state = this.EnableBossMode(boss, bossHasInstance,
                    db, gameState, gameTime);
            }
            else
            {
                for (int i = 0; i < db.Enemies.GetCount(); i++)
                {
                    BulletsFactory.EnemyShoot(db.Bullets, db.Enemies.GetAll()[i]);
                }

                //Creating entities
                EnemyFactory.CreateEnemies(db.Enemies, random);
                AsteroidFactory.CreateAsteroids(db.Asteroids, random);

                ItemFactory.CreateItems(db.Items, db.Enemies.GetAll().Cast<IGameObject>().ToList(), random);
                ItemFactory.CreateItems(db.Items, db.Asteroids.GetAll().Cast<IGameObject>().ToList(), random);

                // Handle collisions between players and enemy objects
                CollisionHandler.CheckForCollision(db.Asteroids.GetAll().Cast<IGameObject>().ToList(), db.Players.GetAll(), db.Explosions.GetAll());
                CollisionHandler.CheckForCollision(db.Enemies.GetAll().Cast<IGameObject>().ToList(), db.Players.GetAll(), db.Explosions.GetAll());

                // Handle collisions between players and enemy items
                CollisionHandler.CheckPlayerItemCollisions(db.Items.GetAll(), db.Players.GetAll());

                ExplosionFactory.CreateExplosion(db.Explosions, db.Enemies.GetAll().Cast<IGameObject>().ToList());
                ExplosionFactory.CreateExplosion(db.Explosions, db.Asteroids.GetAll().Cast<IGameObject>().ToList());

                //Updating entities
                db.Enemies.GetAll().ForEach(e => e.Update(gameTime));
                db.Asteroids.GetAll().ForEach(a => a.Update(gameTime));
                db.Items.GetAll().ForEach(i => i.Update(gameTime));

                // Cleaning with mr.Proper
                EntityCleanerHandler.ClearEnemies(db.Enemies);
                EntityCleanerHandler.ClearAsteroids(db.Asteroids);
                EntityCleanerHandler.ClearExplosion(db.Explosions);
                EntityCleanerHandler.ClearPlayers(db.Players);
            }

            //Update 
            db.Bullets.GetAll().ForEach(b => b.Update(gameTime));
            db.Explosions.GetAll().ForEach(e => e.Update(gameTime));

            statsManager.UpdatePlayersStats(db.Players.GetAll());

            //Handle collisions between bullets and gameobjects
            CollisionHandler.CheckPlayerBulletsCollisions(db.Enemies.GetAll().Cast<IGameObject>().ToList(), db.Bullets.GetAll(), db.Players.GetAll(), db.Explosions.GetAll());
            CollisionHandler.CheckPlayerBulletsCollisions(db.Asteroids.GetAll().Cast<IGameObject>().ToList(), db.Bullets.GetAll(), db.Players.GetAll(), db.Explosions.GetAll());
            CollisionHandler.CheckEnemiesBulletsCollisions(db.Bullets.GetAll(), db.Players.GetAll());

            EntityCleanerHandler.ClearBullets(db.Bullets);

            return state;
        }
    }
}
