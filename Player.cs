using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class Player
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Size { get; } = new Vector2(23, 17);
        public float Rotation { get; set; }
        public Vector2 Position { get; private set; } = new Vector2(25, 25);
        public Vector2 Velocity { get; private set; } = new Vector2(0, 0);

        private const float ROTATE_SPEED = 8;
        private const float MAX_SPEED = 20;
        private const float SPEED_DECAY = 20f;


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

            if (ks.IsKeyDown(Keys.W))
            {
                // Update velocity
                // Get velocity figures?
                float xVel = Velocity.X, yVel = Velocity.Y;

                xVel += (float)System.Math.Cos(Rotation);
                yVel += (float)System.Math.Sin(Rotation);

                Velocity = new Vector2(xVel, yVel);
                Velocity.Normalize();

                // if (System.Math.Abs(xVel + yVel) <= MAX_SPEED)
                // {
                //     Velocity = new Vector2(xVel, yVel);
                //     Velocity.Normalize();
                // }
            }
            else
            {
                // Decay the velocity
                // float xVel = Velocity.X, yVel = Velocity.Y;
                // if (xVel < 0)
                //     if (System.Math.Abs(xVel) < SPEED_DECAY)
                //         xVel = 0;
                //     else
                //         xVel += SPEED_DECAY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                // else if (xVel > 0)
                //     if (xVel < SPEED_DECAY)
                //         xVel = 0;
                //     else
                //         xVel -= SPEED_DECAY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // if (yVel < 0)
                //     if (System.Math.Abs(yVel) < SPEED_DECAY)
                //         yVel = 0;
                //     else
                //         yVel += SPEED_DECAY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                // else if (yVel > 0)
                //     if (yVel < SPEED_DECAY)
                //         yVel = 0;
                //     else
                //         yVel -= SPEED_DECAY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Velocity = new Vector2(xVel, yVel);
            }

            float xPos = Position.X, yPos = Position.Y;
            xPos += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            yPos += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position = new Vector2(xPos, yPos);
        }

        public void DrawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color.White, Rotation, new Vector2(Size.X / 2, Size.Y / 2), 1f, SpriteEffects.None, 1f);
        }
    }
}