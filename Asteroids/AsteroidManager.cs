using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Asteroids.Asteroids
{
    public class AsteroidManager
    {
        public int Wave { get; private set; }
        private List<AsteroidBase> _asteroids;

        public AsteroidManager()
        {
            _asteroids = new List<AsteroidBase>();
        }

        public void UpdateAsteroids(GameTime gameTime, List<Laser> lasers)
        {
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

        public void DrawAsteroids(SpriteBatch spriteBatch)
        {
            foreach(var asteroid in _asteroids)
            {
                asteroid.DrawAsteroid(spriteBatch);
            }
        }
    }
}