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
            float xPos = position.X, yPos = position.Y;
            xPos += direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            yPos += direction.Y * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (xPos < 0 - size.X - 1f)
            {
                xPos = AsteroidsGame.SCREEN_WIDTH + size.X;
            }
            else if (xPos > AsteroidsGame.SCREEN_WIDTH + size.X + 1f)
            {
                xPos = 0 - size.X;
            }

            if (yPos < 0 - size.Y - 1f)
            {
                yPos = AsteroidsGame.SCREEN_WIDTH + size.Y;
            }
            else if (yPos > AsteroidsGame.SCREEN_WIDTH + size.Y + 1f)
            {
                yPos = 0 - size.Y;
            }

            return new Vector2(xPos, yPos);
        }
    }
}