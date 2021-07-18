﻿using Microsoft.Xna.Framework;
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
        private List<Laser> _lasers;
        private AsteroidManager _asteroidManager;

        public static SpriteFont MediumFont;

        public const int SCREEN_WIDTH = 600;
        public const int SCREEN_HEIGHT = 600;

        private const float RELOAD_SPEED = 0.1f;
        private float _timeSinceLastLaserFire = 0f;
        private const int MAX_LASERS = 5;
        private const float LASER_LIFETIME = 2;

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
            _player.Sprite = Content.Load<Texture2D>("playership");
            _laserSprite = Content.Load<Texture2D>("laser");

            AsteroidSprites.LargeAsteroidSprite = Content.Load<Texture2D>("asteroidlarge");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _timeSinceLastLaserFire += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _player.UpdatePlayer(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                AttemptToFireLaser(gameTime);
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
