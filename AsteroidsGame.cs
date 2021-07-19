using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Asteroids.Asteroids;
using System;

namespace Asteroids
{
    public class AsteroidsGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;

        private Texture2D _playerSprite;
        private Texture2D _backdropSprite;

        private AsteroidManager _asteroidManager;
        private LaserManager _laserManager;

        public static SpriteFont MediumFont;
        public static SpriteFont LargeFont;

        public const int SCREEN_WIDTH = 600;
        public const int SCREEN_HEIGHT = 600;



        public static int Score = 0;
        public static int Lives = 5;
        private const float LOST_LIFE_INVICINIBILITY_TIME = 1f;
        private float _timeSinceLostLife = 0f;
        private int _highestScore = 0;
        private bool _drawPlayer = true;
        private FixedTimer _playerDeathFlashTimer;

        public AsteroidsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            _player = new Player();
            _asteroidManager = new AsteroidManager();
            _laserManager = new LaserManager();
            _playerDeathFlashTimer = new FixedTimer(50, false);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fonts
            MediumFont = Content.Load<SpriteFont>("fontmedium");
            LargeFont = Content.Load<SpriteFont>("fontlarge");

            // Player
            _playerSprite = Content.Load<Texture2D>("playership");
            _player.Sprite = _playerSprite;
            
            // lasers and asteroids
            _laserManager.LoadContent(Content);
            AsteroidSprites.LargeAsteroidSprite = Content.Load<Texture2D>("asteroidlarge");
            AsteroidSprites.MediumAsteroidSprite = Content.Load<Texture2D>("asteroidmedium");
            AsteroidSprites.SmallAsteroidSprite = Content.Load<Texture2D>("asteroidsmall");

            // UI
            _backdropSprite = Content.Load<Texture2D>("backdrop");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Lives > 0) // Only update if the game if the player is still in play
            {
                _player.UpdatePlayer(gameTime);
                _timeSinceLostLife += (float)gameTime.ElapsedGameTime.TotalSeconds;

                _laserManager.CheckForLaserFire(_player);

                _laserManager.UpdateLasers(gameTime);

                _asteroidManager.UpdateAsteroids(gameTime, _laserManager.Lasers);

                CheckPlayerAsteroidCollision(gameTime);
            }
            
            base.Update(gameTime);
        }

        private void CheckPlayerAsteroidCollision(GameTime gameTime)
        {
            // Loop through Asteroids and check for player collision...
            if (_timeSinceLostLife > LOST_LIFE_INVICINIBILITY_TIME)
            {
                _drawPlayer = true;
                _playerDeathFlashTimer.Enabled = false;
                for (int asteroidIndex = _asteroidManager.Asteroids.Count - 1; asteroidIndex >= 0; asteroidIndex--)
                {
                    var asteroid = _asteroidManager.Asteroids[asteroidIndex];
                    if (CollisionHelper.CheckBasicCollision(_player.Position, _player.Size, asteroid.Position, asteroid.Size))
                    {
                        Lives--;
                        _timeSinceLostLife = 0;

                        if (!(asteroid is SmallAsteroid))
                            _asteroidManager.PopAsteroid(asteroid);

                        _asteroidManager.Asteroids.RemoveAt(asteroidIndex);

                        break;
                    }
                }
            }
            else
            {
                _playerDeathFlashTimer.Enabled = true;
                _playerDeathFlashTimer.Update(gameTime);

                if (_playerDeathFlashTimer.Tick) { _drawPlayer = !_drawPlayer; }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Draw lives
            for (int i = Lives - 1; i >= 0; i--)
            {
                _spriteBatch.Draw(_playerSprite, new Vector2(SCREEN_WIDTH - ((25 - 5) * (i + 1)), 25), null, Color.White, (float)(System.Math.PI / 2) * 3, Vector2.Zero, 0.65f, SpriteEffects.None, 0f);
            }

            _laserManager.DrawLasers(_spriteBatch);

            _asteroidManager.DrawAsteroids(_spriteBatch);

            if (_drawPlayer)
                _player.DrawPlayer(_spriteBatch);

            if (Lives == 0)
            {
                //Backdrop box
                string scoreText = "Final Score: " + Score;
                var textMeasurement = LargeFont.MeasureString(scoreText);

                _spriteBatch.Draw(_backdropSprite, new Rectangle(Convert.ToInt32((SCREEN_WIDTH / 2) - (textMeasurement.X / 2)) - 15, 185, Convert.ToInt32(textMeasurement.X) + 30, Convert.ToInt32(textMeasurement.Y) + 20), Color.White);
                _spriteBatch.DrawString(LargeFont, scoreText, new Vector2((SCREEN_WIDTH / 2) - (textMeasurement.X / 2), 200), Color.MonoGameOrange);
            }
            else
            {
                _spriteBatch.DrawString(MediumFont, "Score: " + Score.ToString(), new Vector2(25, 25), Color.MonoGameOrange);
            }

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
