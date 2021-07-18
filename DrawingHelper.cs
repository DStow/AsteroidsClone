using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    public static class DrawingHelper
    {
        public static void DrawGhostIfNeeded(SpriteBatch spriteBatch, Texture2D sprite, Vector2 position, Vector2 size, float rotation)
        {
            if(position.X < size.X / 2)
            {
                spriteBatch.Draw(sprite, new Vector2(position.X + AsteroidsGame.SCREEN_WIDTH, position.Y), null, Color.White, rotation, new Vector2(size.X / 2, size.Y / 2), 1f, SpriteEffects.None, 1f);
            }
            else if(position.X > AsteroidsGame.SCREEN_WIDTH - (size.X / 2))
            {
                spriteBatch.Draw(sprite, new Vector2(position.X - AsteroidsGame.SCREEN_WIDTH, position.Y), null, Color.White, rotation, new Vector2(size.X / 2, size.Y / 2), 1f, SpriteEffects.None, 1f);
            }

            if(position.Y < size.Y / 2)
            {
                // Bottom ghost
                spriteBatch.Draw(sprite, new Vector2(position.X, position.Y + AsteroidsGame.SCREEN_HEIGHT), null, Color.White, rotation, new Vector2(size.X / 2, size.Y / 2), 1f, SpriteEffects.None, 1f);
            }
            else if(position.Y > AsteroidsGame.SCREEN_HEIGHT - (size.Y / 2))
            {
                // Top ghost
                spriteBatch.Draw(sprite, new Vector2(position.X, position.Y - AsteroidsGame.SCREEN_HEIGHT), null, Color.White, rotation, new Vector2(size.X / 2, size.Y / 2), 1f, SpriteEffects.None, 1f);
            }
        }
    }
}