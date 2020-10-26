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

        MouseState mouseState;
        KeyboardState keyboardState;

        Texture2D tankTexture;
        SpriteFont textFont;

        // Automatically rotating tanks
        Rectangle tank1Rect;
        Vector2 tank1RotationOrigin;

        Rectangle tank2Rect;
        Vector2 tank2RotationOrigin;

        Rectangle tank3Rect;
        Vector2 tank3RotationOrigin;

        
        // Tank that will be rotated by the mouse
        Vector2 tankLocationMouse;
        Rectangle tankHitBoxMouse;

        //  Tank that will be rotated by the keyboard
        Vector2 tankLocationKeyboard;

        float tankAngle;            // Automatic rotation angle
        float tankAngleKeyboard;    // Rotating with keyboard angle
        float tankAngleMouse;       // Pointing towards mouse angle


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            tankAngle = 0f;
            tankAngleKeyboard = 0f;

            base.Initialize();

            // Automatically rotating tanks to give visual on how setting the origin point affects where image is drawn
            tank1Rect = new Rectangle(100, 100, tankTexture.Width, tankTexture.Height);
            tank1RotationOrigin = new Vector2(0, 0);        //Origin is top left of texture

            tank2Rect = new Rectangle(375, 100, tankTexture.Width, tankTexture.Height);
            tank2RotationOrigin = new Vector2(tankTexture.Width / 2, tankTexture.Height / 2);   //Origin is middle of texture

            tank3Rect = new Rectangle(650, 100, tankTexture.Width, tankTexture.Height);
            tank3RotationOrigin = new Vector2(tankTexture.Width, tankTexture.Height);       //Origin is bottom right of texture


            //Tank that follows mouse
            tankLocationMouse = new Vector2(300, 400); // Location of the center of the tank          
            // This is where a hitbox rectangle could be for a rotating tank, but it would be better to use a bounding circle
            tankHitBoxMouse = new Rectangle(300 - tankTexture.Width / 2, 400 - tankTexture.Height / 2, tankTexture.Width, tankTexture.Height);

            //Tank that you control with the keyboard
            tankLocationKeyboard = new Vector2(500, 400);

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
            keyboardState = Keyboard.GetState();

            // Escape Quits
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // Automatic rotation for location examples
            tankAngle += 0.02f;

            // Gets angle of rotation between tank location and mouse location
            tankAngleMouse = GetAngle(tankLocationMouse, new Vector2(mouseState.X, mouseState.Y));

            // Rotates based on Keyboard
            // Rotates counter clockwise if left arrow is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                tankAngleKeyboard -= 0.03f;
                if (tankAngleKeyboard < 0)      // Keeps rotation between 0 and 360 to prevent overflow
                    tankAngleKeyboard = (float)(2 * Math.PI);
            }

            // Rotates clockwise if right arrow is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                tankAngleKeyboard += 0.03f;
                if (tankAngleKeyboard > 2 * Math.PI)    // Keeps rotation between 0 and 360 to prevent overflow
                    tankAngleKeyboard = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            // Origin Location (0, 0)
            _spriteBatch.Draw(tankTexture, tank1Rect, null, Color.White, tankAngle, tank1RotationOrigin, SpriteEffects.None, 1);
            _spriteBatch.Draw(tankTexture, tank1Rect, Color.White);
            _spriteBatch.DrawString(textFont, "Center of rotation: (0, 0)  ", new Vector2(40, 220), Color.White);


            // Origin location middle of tankRect
            _spriteBatch.Draw(tankTexture, tank2Rect, null, Color.White, tankAngle, tank2RotationOrigin , SpriteEffects.None, 1);
            _spriteBatch.Draw(tankTexture, tank2Rect, Color.White);
            _spriteBatch.DrawString(textFont, "Center of rotation\nmiddle of texture", new Vector2(350, 220), Color.White);

            // Origin location  bottom right of tankRect
            _spriteBatch.Draw(tankTexture, tank3Rect, null, Color.White, tankAngle, tank3RotationOrigin, SpriteEffects.None, 1);
            _spriteBatch.Draw(tankTexture, tank3Rect, Color.White);
            _spriteBatch.DrawString(textFont, "Center of rotation\nbottom right of texture", new Vector2(580, 220), Color.White);

            // Rotates to follow the mouse
            _spriteBatch.Draw(tankTexture, tankLocationMouse, null, Color.White, tankAngleMouse, new Vector2(tankTexture.Width / 2, tankTexture.Height / 2), 1f, SpriteEffects.None, 1);
            _spriteBatch.DrawString(textFont, "Angle: " + (int)(360 - Math.Round(tankAngleMouse * (180 / Math.PI), 1)) % 360 + " degrees", new Vector2(40, 320), Color.White);
            if (tankHitBoxMouse.Contains(mouseState.X, mouseState.Y))   // Displays message if cursor intersects with mouse controlled tank
                _spriteBatch.DrawString(textFont, "Mouse Hitting Tank", new Vector2(40, 380), Color.White);

            // Keyboard controlled angle
            _spriteBatch.Draw(tankTexture, tankLocationKeyboard, null, Color.White, tankAngleKeyboard, new Vector2(tankTexture.Width / 2, tankTexture.Height / 2), 1f, SpriteEffects.None, 1);
            _spriteBatch.DrawString(textFont, "Angle: " + (int)(360 - Math.Round(tankAngleKeyboard * (180 / Math.PI), 1)) % 360 + " degrees", new Vector2(575, 320), Color.White);

            _spriteBatch.End();


            base.Draw(gameTime);
        }

        // Determines the angle in radians that the origin texture needs to be rotated by in order to point at secondPoint
        public float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;
            float run = secondPoint.X - originPoint.X;

            // First or Fourth Quadrant
            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)
                return (float)Math.Atan(rise / run);
            //Second or Third Quadrant
            else
                return (float)(Math.PI + Math.Atan(rise / run));
        }
    }
}
