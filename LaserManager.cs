using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Asteroids
{
    public class LaserManager
    {
        public List<Laser> Lasers { get; private set; }
        private const int MAX_LASERS = 5;
        private const float LASER_LIFETIME = 2;
        private const float RELOAD_SPEED = 0.1f;
        private float _timeSinceLastLaserFire = 0f;
        private bool _spaceDown = false;
        private Texture2D _laserSprite;

        public LaserManager()
        {
            Lasers = new List<Laser>();
        }

        public void LoadContent(ContentManager content)
        {
            _laserSprite = content.Load<Texture2D>("laser");
        }

        public void CheckForLaserFire(Player player)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && _spaceDown == false)
            {
                _spaceDown = true;
                AttemptToFireLaser(player);
            }
            else if (keyboardState.IsKeyDown(Keys.Space) == false)
            {
                _spaceDown = false;
            }
        }

        private void AttemptToFireLaser(Player player)
        {
            // Can't fire a laser if we have max out already
            if (Lasers.Count >= MAX_LASERS)
                return;

            // Check if enough time has passed since last firing
            if (_timeSinceLastLaserFire < RELOAD_SPEED)
                return;

            // if we made it this far it's all good to go
            var newLaser = new Laser(_laserSprite, player.Position, player.Rotation);
            Lasers.Add(newLaser);
            _timeSinceLastLaserFire = 0f;
        }

        public void UpdateLasers(GameTime gameTime)
        {
            _timeSinceLastLaserFire += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = Lasers.Count - 1; i >= 0; i--)
            {
                Lasers[i].UpdateLaser(gameTime);

                if (Lasers[i].Lifespan > LASER_LIFETIME)
                    Lasers.RemoveAt(i);
            }
        }

        public void DrawLasers(SpriteBatch spriteBatch)
        {
            foreach (var laser in Lasers)
            {
                laser.DrawLaser(spriteBatch);
            }
        }
    }
}