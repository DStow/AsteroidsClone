using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    public class Laser
    {
        public Texture2D LaserSprite { get; set; }
        public float Rotation { get; set; }
        public Vector2 Size { get; } = new Vector2(8, 8);
        public Vector2 Position { get; private set; } = new Vector2(25, 25);
        public Vector2 Direction { get; private set; }
        private const float SPEED = 400f;
        public float Lifespan { get; private set; }

        public Laser(Texture2D laserSprite, Vector2 position, float rotation)
        {
            LaserSprite = laserSprite;
            Position = position;
            Rotation = rotation;

            float xDir, yDir;
            xDir = (float)Math.Cos(Rotation);
            yDir = (float)Math.Sin(rotation);

            Direction = new Vector2(xDir, yDir);
        }

        public void UpdateLaser(GameTime gameTime)
        {
            Position = MovementHelper.MoveObjectInDirectionWithScreenWrap(gameTime, Position, Direction, Size, SPEED);

            Lifespan += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void DrawLaser(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(LaserSprite, Position, null, Color.White, Rotation, new Vector2(Size.X / 2, Size.Y / 2), 1f, SpriteEffects.None, 1f);
        }
    }
}
