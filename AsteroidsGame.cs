using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class AsteroidsGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;

        public static SpriteFont MediumFont;

        public AsteroidsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _player = new Player();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            MediumFont = Content.Load<SpriteFont>("fontmedium");
            _player.Sprite = Content.Load<Texture2D>("playership");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _player.UpdatePlayer(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _player.DrawPlayer(_spriteBatch);

            // Write out hte player velocity for debugging
            _spriteBatch.DrawString(MediumFont, "Velocity: " + _player.Velocity.ToString() + "\nRotation: " + _player.Rotation.ToString("N2"), new Vector2(25, 25), Color.MonoGameOrange);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
