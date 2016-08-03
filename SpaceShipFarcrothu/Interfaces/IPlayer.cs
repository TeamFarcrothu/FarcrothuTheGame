﻿namespace SpaceShipFartrothu.Interfaces
{
    using Microsoft.Xna.Framework.Graphics;
    using SpaceShipFartrothu.Multimedia;

    public interface IPlayer : IGameObject
    {
        int Id { get; }

        bool IsAlive { get; set; }

        int BulletDamage { get; set; }

        int Score { get; set; }

        int Health { get; set; }

        int BulletDelay { get; set; }

        int MaxHealth { get; set; }

        int Level { get; set; }

        Texture2D BulletTexture { get; set; }

        SoundManager SoundManager { get; set; }
    }
}