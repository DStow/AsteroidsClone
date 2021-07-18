using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids.Asteroids
{

    public abstract class AsteroidBase
    {
        public float Rotation { get; set; }
        public Vector2 Size { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; } = 100f;

        public AsteroidBase()
        {
            Size = AsteroidSize();
        }

        public void UpdateAsteroid(GameTime gameTime)
        {
            Position = MovementHelper.MoveObjectInDirectionWithScreenWrap(gameTime, Position, Direction, Size, Speed);
        }

        public void DrawAsteroid(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GetAsteroidSprite(), Position, null, Color.White, Rotation, new Vector2(Size.X / 2, Size.Y / 2), 1f, SpriteEffects.None, 1f);
        }

        protected abstract Texture2D GetAsteroidSprite();

        protected abstract Vector2 AsteroidSize();
    }

}