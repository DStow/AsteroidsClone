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
        private Texture2D _laserSprite;
        private Texture2D _playerSprite;
        private Texture2D _backdropSprite;
        private List<Laser> _lasers;
        private AsteroidManager _asteroidManager;

        public static SpriteFont MediumFont;
        public static SpriteFont LargeFont;

        public const int SCREEN_WIDTH = 600;
        public const int SCREEN_HEIGHT = 600;

        private const float RELOAD_SPEED = 0.1f;
        private float _timeSinceLastLaserFire = 0f;
        private bool _spaceDown = false;
        private const int MAX_LASERS = 5;
        private const float LASER_LIFETIME = 2;
        public static int Score = 0;
        public static int Lives = 5;
        private const float LOST_LIFE_INVICINIBILITY_TIME = 1f;
        private float _timeSinceLostLife = 0f;
        private int _highestScore = 0;

        public AsteroidsGame()
        {
            _graphics = new GraphicsDeviceManager(this);



            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();


            _player = new Player();
            _lasers = new List<Laser>();
            _asteroidManager = new AsteroidManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            MediumFont = Content.Load<SpriteFont>("fontmedium");
            LargeFont = Content.Load<SpriteFont>("fontlarge");
            _playerSprite = Content.Load<Texture2D>("playership");
            _player.Sprite = _playerSprite;
            _laserSprite = Content.Load<Texture2D>("laser");

            AsteroidSprites.LargeAsteroidSprite = Content.Load<Texture2D>("asteroidlarge");
            AsteroidSprites.MediumAsteroidSprite = Content.Load<Texture2D>("asteroidmedium");
            AsteroidSprites.SmallAsteroidSprite = Content.Load<Texture2D>("asteroidsmall");
            _backdropSprite = Content.Load<Texture2D>("backdrop");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Lives > 0)
            {
                _timeSinceLastLaserFire += (float)gameTime.ElapsedGameTime.TotalSeconds;

                _player.UpdatePlayer(gameTime);
                _timeSinceLostLife += (float)gameTime.ElapsedGameTime.TotalSeconds;

                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space) && _spaceDown == false)
                {
                    _spaceDown = true;
                    AttemptToFireLaser(gameTime);
                }
                else if (keyboardState.IsKeyDown(Keys.Space) == false)
                {
                    _spaceDown = false;
                }

                for (int i = _lasers.Count - 1; i >= 0; i--)
                {
                    _lasers[i].UpdateLaser(gameTime);

                    if (_lasers[i].Lifespan > LASER_LIFETIME)
                        _lasers.RemoveAt(i);
                }

                // Loop through Asteroids and check for player collision...
                if (_timeSinceLostLife > LOST_LIFE_INVICINIBILITY_TIME)
                {
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

                _asteroidManager.UpdateAsteroids(gameTime, _lasers);
            }
            else
            {
                // Don't update game objects are we are on teh score screen?
            }
            base.Update(gameTime);
        }

        private void AttemptToFireLaser(GameTime gameTime)
        {
            // Can't fire a laser if we have max out already
            if (_lasers.Count >= MAX_LASERS)
                return;

            // Check if enough time has passed since last firing
            if (_timeSinceLastLaserFire < RELOAD_SPEED)
                return;

            // if we made it this far it's all good to go

            var newLaser = new Laser(_laserSprite, _player.Position, _player.Rotation);
            _lasers.Add(newLaser);
            _timeSinceLastLaserFire = 0f;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();



            // Draw lives
            for (int i = Lives - 1; i >= 0; i--)
            {
                _spriteBatch.Draw(_playerSprite, new Vector2(SCREEN_WIDTH - ((25 - 5) * (i + 1)), 25), null, Color.White, (float)(System.Math.PI / 2) * 3, Vector2.Zero, 0.65f, SpriteEffects.None, 0f);
            }

            foreach (var laser in _lasers)
            {
                laser.DrawLaser(_spriteBatch);
            }

            _asteroidManager.DrawAsteroids(_spriteBatch);

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
