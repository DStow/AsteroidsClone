using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    public static class MovementHelper
    {
        public static Vector2 MoveObjectInDirectionWithScreenWrap(GameTime gameTime, Vector2 position, Vector2 direction, Vector2 size, float speed)
        {
            var velocity = new Vector2(direction.X * speed, direction.Y * speed);
            return MoveObjectInVelocityWithScreenWrap(gameTime, position, velocity, size);
        }

        public static Vector2 MoveObjectInVelocityWithScreenWrap(GameTime gameTime, Vector2 position, Vector2 velocity, Vector2 size)
        {
            float xPos = position.X, yPos = position.Y;
            xPos += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            yPos += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (xPos < 0 - (size.X / 2))
            {
                xPos = AsteroidsGame.SCREEN_WIDTH - (size.X / 2);
            }
            else if (xPos > AsteroidsGame.SCREEN_WIDTH + (size.X / 2))
            {
                xPos = 0 + (size.X / 2);
            }

            if (yPos < 0 - (size.Y / 2))
            {
                yPos = AsteroidsGame.SCREEN_HEIGHT - (size.Y / 2);
            }
            else if (yPos > AsteroidsGame.SCREEN_HEIGHT + (size.Y / 2))
            {
                yPos = 0 + (size.Y / 2);
            }

            return new Vector2(xPos, yPos);
        }
    }
}