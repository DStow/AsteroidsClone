using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    public class Player
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Size { get; } = new Vector2(23, 17);
        public float Rotation { get; set; }
        public Vector2 Position { get; private set; } = new Vector2(25, 25);
        public Vector2 Velocity { get; private set; } = new Vector2(0, 0);

        private const float ROTATE_SPEED = 8f;
        private const float MAX_SPEED = 200f;
        private const float SPEED_DECAY = 20f;
        private const float ACCELERATION = 200f;
        private const float DEACCELERATION = 220f;

        public void UpdatePlayer(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            UpdatePlayerRotation(gameTime, keyboardState);

            UpdatePlayerAcceleration(gameTime, keyboardState);

            UpdatePlayerPosition(gameTime, keyboardState);
        }

        private void UpdatePlayerRotation(GameTime gameTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Rotation -= ROTATE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                Rotation += ROTATE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void UpdatePlayerAcceleration(GameTime gameTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                // Update velocity
                // Get velocity figures?
                float xVel = Velocity.X, yVel = Velocity.Y;

                xVel += (float)Math.Cos(Rotation) * ACCELERATION * (float)gameTime.ElapsedGameTime.TotalSeconds;
                yVel += (float)Math.Sin(Rotation) * ACCELERATION * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Clamp the speeds down
                xVel = Math.Clamp(xVel, MAX_SPEED * -1, MAX_SPEED);
                yVel = Math.Clamp(yVel, MAX_SPEED * -1, MAX_SPEED);

                Velocity = new Vector2(xVel, yVel);
            }
            else
            {
                float xVel = Velocity.X, yVel = Velocity.Y;
                // Normalize the current velocity vector
                Vector2 playerDir = new Vector2(Velocity.X, Velocity.Y);
                playerDir.Normalize();

                // Use the player Dir as a way to fraction out how much deaccelleration to apply

                // X Axis Decceleration
                if (Math.Abs(xVel) > 0)
                {
                    float xDeaccelAmount = playerDir.X * DEACCELERATION * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Math.Abs(xDeaccelAmount) > Math.Abs(xVel))
                    {
                        xVel = 0;
                    }
                    else
                    {
                        xVel -= xDeaccelAmount;
                    }
                }

                // Y Axis Decceleration
                if (Math.Abs(yVel) > 0)
                {
                    float yDeaccelAmount = playerDir.Y * DEACCELERATION * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Math.Abs(yDeaccelAmount) > Math.Abs(yVel))
                    {
                        yVel = 0;
                    }
                    else
                    {
                        yVel -= yDeaccelAmount;
                    }
                }

                Velocity = new Vector2(xVel, yVel);
            }
        }

        private void UpdatePlayerPosition(GameTime gameTime, KeyboardState keyboardState)
        {
            float xPos = Position.X, yPos = Position.Y;
            xPos += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            yPos += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (xPos < 0 - Size.X - 1f)
            {
                xPos = AsteroidsGame.SCREEN_WIDTH + Size.X;
            }
            else if (xPos > AsteroidsGame.SCREEN_WIDTH + Size.X + 1f)
            {
                xPos = 0 - Size.X;
            }

            if (yPos < 0 - Size.Y - 1f)
            {
                yPos = AsteroidsGame.SCREEN_WIDTH + Size.Y;
            }
            else if (yPos > AsteroidsGame.SCREEN_WIDTH + Size.Y + 1f)
            {
                yPos = 0 - Size.Y;
            }

            Position = new Vector2(xPos, yPos);
        }

        public void DrawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color.White, Rotation, new Vector2(Size.X / 2, Size.Y / 2), 1f, SpriteEffects.None, 1f);
        }
    
        public Vector2 GetNormalizedDirection()
        {
            Vector2 result = new Vector2(Velocity.X, Velocity.Y);
            result.Normalize();
            return result;
        }
    }
}