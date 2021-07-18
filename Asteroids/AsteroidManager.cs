using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Asteroids.Asteroids
{
    public class AsteroidManager
    {
        public int Wave { get; private set; } = 0;
        public List<AsteroidBase> Asteroids;
        private Random _random;

        public AsteroidManager()
        {
            Asteroids = new List<AsteroidBase>();
            _random = new Random();
        }

        public void UpdateAsteroids(GameTime gameTime, List<Laser> lasers)
        {
            if (Asteroids.Count == 0)
            {
                // Wave has ended hurrah
                Wave++;
                SpawnAsteroidWave();
            }

            for (int i = Asteroids.Count - 1; i >= 0; i--)
            {
                Asteroids[i].UpdateAsteroid(gameTime);
            }

            HandleLaserCollisions(lasers);
        }

        private void HandleLaserCollisions(List<Laser> lasers)
        {
            // ToDo: Implement somehow...
            for (int laserIndex = lasers.Count - 1; laserIndex >= 0; laserIndex--)
            {
                var laser = lasers[laserIndex];
                for (int asteroidIndex = Asteroids.Count - 1; asteroidIndex >= 0; asteroidIndex--)
                {
                    var asteroid = Asteroids[asteroidIndex];
                    if (CollisionHelper.CheckBasicCollision(laser.Position, laser.Size, asteroid.Position, asteroid.Size))
                    {
                        if (asteroid is SmallAsteroid)
                        {
                            AsteroidsGame.Score += 30;
                            Asteroids.RemoveAt(asteroidIndex);
                        }
                        else if (asteroid is MediumAsteroid)
                        {
                            AsteroidsGame.Score += 20;
                            PopAsteroid(asteroid);
                            Asteroids.RemoveAt(asteroidIndex);
                        }
                        else if (asteroid is LargeAsteroid)
                        {
                            AsteroidsGame.Score += 10;
                            PopAsteroid(asteroid);
                            Asteroids.RemoveAt(asteroidIndex);
                        }

                        lasers.RemoveAt(laserIndex);
                        break;
                    }
                }
            }
        }

        private void SpawnAsteroidWave()
        {
            // for now we will just spawn 4 larger asteroids per wave...
            for (int i = 0; i < 4 + Wave; i++)
            {
                // We need random X, Y and Velocity
                float xPos, yPos, xDir, yDir;
                xPos = _random.Next(0, AsteroidsGame.SCREEN_WIDTH);
                yPos = _random.Next(0, AsteroidsGame.SCREEN_WIDTH);
                xDir = (float)(_random.NextDouble() * 2) - 1;
                yDir = (float)(_random.NextDouble() * 2) - 1;

                var newAsteroid = new LargeAsteroid();
                newAsteroid.Position = new Vector2(xPos, yPos);
                newAsteroid.Direction = new Vector2(xDir, yDir);

                Asteroids.Add(newAsteroid);
            }
        }

        public void PopAsteroid(AsteroidBase parentAsteroid)
        {
            AsteroidBase babyAsteroid1, babyAsteroid2;

            if (parentAsteroid is LargeAsteroid)
            {
                babyAsteroid1 = new MediumAsteroid();
                babyAsteroid2 = new MediumAsteroid();
            }
            else // Assumed to be a medium asteroid
            {
                babyAsteroid1 = new SmallAsteroid();
                babyAsteroid2 = new SmallAsteroid();
            }


            babyAsteroid1.Position = parentAsteroid.Position;
            Vector2 baby1Dir = new Vector2(parentAsteroid.Direction.X - 0.2f, parentAsteroid.Direction.Y - 0.2f);
            babyAsteroid1.Direction = baby1Dir;
            babyAsteroid1.Speed = parentAsteroid.Speed + 1.2f;
            Asteroids.Add(babyAsteroid1);

            babyAsteroid2.Position = parentAsteroid.Position;
            Vector2 baby2Dir = new Vector2(parentAsteroid.Direction.X + 0.2f, parentAsteroid.Direction.Y + 0.2f);
            babyAsteroid2.Direction = baby2Dir;
            babyAsteroid2.Speed = parentAsteroid.Speed * 1.2f;
            Asteroids.Add(babyAsteroid2);

        }

        public void DrawAsteroids(SpriteBatch spriteBatch)
        {
            foreach (var asteroid in Asteroids)
            {
                asteroid.DrawAsteroid(spriteBatch);
            }
        }
    }
}