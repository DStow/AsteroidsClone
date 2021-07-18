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

        public void DrawAsteroids(SpriteBatch spriteBatch)
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.DrawAsteroid(spriteBatch);
            }
        }
    }
}