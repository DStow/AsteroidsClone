using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class Player
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Size { get; } = new Vector2(17, 23);
        public float Rotation { get; set; }
        public Vector2 Position { get; private set; } = new Vector2(25, 25);
        public Vector2 Velocity { get; private set; } = new Vector2(0, 0);
        private const float ROTATE_SPEED = 8;

        public void UpdatePlayer(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.A))
            {
                Rotation -= ROTATE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ks.IsKeyDown(Keys.D))
            {
                Rotation += ROTATE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void DrawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color.White, Rotation, new Vector2(Size.X / 2, Size.Y / 2), 1f, SpriteEffects.None, 1f);
        }
    }
}