using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Texture_Rotation
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D tankTexture;
        Rectangle tankRect;
        Vector2 tankLocation;

        SpriteFont textFont;

        Vector2 tankSpeed;
        float tankDirection;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            tankDirection = 0f;


            base.Initialize();
            //tankRect = new Rectangle(100, 100, tankTexture.Width, tankTexture.Height);
            tankLocation = new Vector2(100, 100); // Location of the center of the tank
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tankTexture = Content.Load<Texture2D>("tank");
            textFont = Content.Load<SpriteFont>("fontInfo");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            tankDirection += 0.02f;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.DrawString(textFont, "Center of rotation:  " + tankRect.Center.ToVector2(), new Vector2(10, 400), Color.White);

            _spriteBatch.Draw(tankTexture, tankLocation, null, Color.White, tankDirection, new Vector2(tankTexture.Width/2, tankTexture.Height/2), 1.0f, SpriteEffects.None, 1);
            //_spriteBatch.Draw(tankTexture, tankRect, Color.White); 

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
