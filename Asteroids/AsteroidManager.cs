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
        private List<AsteroidBase> _asteroids;
        private Random _random;

        public AsteroidManager()
        {
            _asteroids = new List<AsteroidBase>();
            _random = new Random();
        }

        public void UpdateAsteroids(GameTime gameTime, List<Laser> lasers)
        {
            if (_asteroids.Count == 0)
            {
                // Wave has ended hurrah
                Wave++;
                SpawnAsteroidWave();
            }

            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].UpdateAsteroid(gameTime);
            }

            HandleLaserCollisions(lasers);
        }

        private void HandleLaserCollisions(List<Laser> lasers)
        {
            // ToDo: Implement somehow...
            for (int laserIndex = lasers.Count - 1; laserIndex >= 0; laserIndex--)
            {
                var laser = lasers[laserIndex];
                for (int asteroidIndex = _asteroids.Count - 1; asteroidIndex >= 0; asteroidIndex--)
                {
                    var asteroid = _asteroids[asteroidIndex];
                    if (CollisionHelper.CheckBasicCollision(laser.Position, laser.Size, asteroid.Position, asteroid.Size))
                    {
                        if (asteroid is SmallAsteroid)
                        {
                            _asteroids.RemoveAt(asteroidIndex);
                        }
                        else
                        {
                            PopAsteroid(asteroid);
                            _asteroids.RemoveAt(asteroidIndex);
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
            for (int i = 0; i < 4; i++)
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

                _asteroids.Add(newAsteroid);
            }
        }

        private void PopAsteroid(AsteroidBase parentAsteroid)
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
            babyAsteroid1.Speed = parentAsteroid.Speed + 0.2f;
            _asteroids.Add(babyAsteroid1);

            babyAsteroid2.Position = parentAsteroid.Position;
            Vector2 baby2Dir = new Vector2(parentAsteroid.Direction.X + 0.2f, parentAsteroid.Direction.Y + 0.2f);
            babyAsteroid2.Direction = baby2Dir;
            babyAsteroid2.Speed = parentAsteroid.Speed * 0.2f;
            _asteroids.Add(babyAsteroid2);

        }

        public void DrawAsteroids(SpriteBatch spriteBatch)
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.DrawAsteroid(spriteBatch);
            }
        }
    }
}