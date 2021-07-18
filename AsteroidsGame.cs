using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Asteroids.Asteroids;

namespace Asteroids
{
    public class AsteroidsGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private Texture2D _laserSprite;
        private Texture2D _playerSprite;
        private List<Laser> _lasers;
        private AsteroidManager _asteroidManager;

        public static SpriteFont MediumFont;

        public const int SCREEN_WIDTH = 600;
        public const int SCREEN_HEIGHT = 600;

        private const float RELOAD_SPEED = 0.1f;
        private float _timeSinceLastLaserFire = 0f;
        private bool _spaceDown = false;
        private const int MAX_LASERS = 5;
        private const float LASER_LIFETIME = 2;
        public static int Score = 0;
        public static int Lives = 5;

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
            _playerSprite = Content.Load<Texture2D>("playership");
            _player.Sprite = _playerSprite;
            _laserSprite = Content.Load<Texture2D>("laser");

            AsteroidSprites.LargeAsteroidSprite = Content.Load<Texture2D>("asteroidlarge");
            AsteroidSprites.MediumAsteroidSprite = Content.Load<Texture2D>("asteroidmedium");
            AsteroidSprites.SmallAsteroidSprite = Content.Load<Texture2D>("asteroidsmall");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _timeSinceLastLaserFire += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _player.UpdatePlayer(gameTime);

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

            _asteroidManager.UpdateAsteroids(gameTime, _lasers);

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

            _spriteBatch.DrawString(MediumFont, "Score: " + Score.ToString(), new Vector2(25, 25), Color.MonoGameOrange);

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

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
