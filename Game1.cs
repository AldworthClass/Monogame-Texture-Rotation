using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Monogame_Texture_Rotation
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D tankTexture;
        Rectangle tankRect1;
        Rectangle tankRect2;
        Rectangle tankRect3;
        Rectangle tankRect4;

        Vector2 tankLocation;

        SpriteFont textFont;

        Vector2 tankSpeed;
        float tankDirection;
        float tankAngle;

        MouseState mouseState;
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
            tankRect1 = new Rectangle(100, 100, tankTexture.Width, tankTexture.Height);
            tankRect2 = new Rectangle(375, 100, tankTexture.Width, tankTexture.Height);
            tankRect3 = new Rectangle(650, 100, tankTexture.Width, tankTexture.Height);

            //Tank that follows mouse
            tankLocation = new Vector2(400, 400); // Location of the center of the tank          
            // This is where a hitbox rectangle could be for rotating tank, but it would be better to use a bounding circle
            tankRect4 = new Rectangle(400 - tankTexture.Width / 2, 400 - tankTexture.Height / 2, tankTexture.Width, tankTexture.Height);
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
            mouseState = Mouse.GetState();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            tankDirection += 0.02f;

            //Gets angle of rotation from tank location and mouse location
            tankAngle = GetAngle(tankLocation, new Vector2(mouseState.X, mouseState.Y));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            //_spriteBatch.DrawString(textFont, "Center of rotation:  " + tankRect.Center.ToVector2(), new Vector2(10, 400), Color.White);

            // Origin Location (0, 0)
            _spriteBatch.Draw(tankTexture, tankRect1, null, Color.White, tankDirection, new Vector2(0, 0), SpriteEffects.None, 1);
            _spriteBatch.Draw(tankTexture, tankRect1, Color.White);
            _spriteBatch.DrawString(textFont, "Center of rotation: (0, 0)  ", new Vector2(40, 220), Color.White);


            // Origin location middle of tankRect
            _spriteBatch.Draw(tankTexture, tankRect2, null, Color.White, tankDirection, new Vector2(tankTexture.Width / 2, tankTexture.Height / 2), SpriteEffects.None, 1);
            _spriteBatch.Draw(tankTexture, tankRect2, Color.White);
            _spriteBatch.DrawString(textFont, "Center of rotation\nmiddle of texture", new Vector2(350, 220), Color.White);

            // Origin location  bottom right of tankRect
            _spriteBatch.Draw(tankTexture, tankRect3, null, Color.White, tankDirection, new Vector2(tankTexture.Width, tankTexture.Height), SpriteEffects.None, 1);
            _spriteBatch.Draw(tankTexture, tankRect3, Color.White);
            _spriteBatch.DrawString(textFont, "Center of rotation\nbottom right of texture", new Vector2(580, 220), Color.White);

            // Rotates to follow the mouse
            _spriteBatch.Draw(tankTexture, tankLocation, null, Color.White, tankAngle, new Vector2(tankTexture.Width / 2, tankTexture.Height / 2), 1f, SpriteEffects.None, 1);
            _spriteBatch.DrawString(textFont, "Angle: " + (int)(360 - Math.Round(tankAngle * (180 / Math.PI), 1)) % 360 + " degrees", new Vector2(40, 320), Color.White);

            _spriteBatch.End();


            base.Draw(gameTime);
        }

        // Must figure out how to get the correct angle in Quadrants 2-4 and convert them to clockwise rotation 
        public float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;
            float run = secondPoint.X - originPoint.X;

            // First or Fourth Quadrant
            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)
                return (float)Math.Atan(rise / run);
            //Second or Third Quadrant
            else
                return (float)Math.PI + (float)Math.Atan(rise / run);
        }
    }
}
