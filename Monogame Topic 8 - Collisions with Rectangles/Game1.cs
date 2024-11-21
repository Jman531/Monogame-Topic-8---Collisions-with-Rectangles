using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monogame_Topic_8___Collisions_with_Rectangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int pacSpeed, coinsCollected;

        Rectangle window;

        KeyboardState keyboardState;
        MouseState mouseState;

        Texture2D pacLeftTexture, pacRightTexture, pacUpTexture, pacDownTexture, currentPacTexture;
        Rectangle pacRect;

        Texture2D exitTexture;
        Rectangle exitRect;

        Texture2D barrierTexture;
        List<Rectangle> barriers;

        Texture2D coinTexture;
        List<Rectangle> coins;

        SpriteFont coinFont;

        Random generator = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0, 0, 800, 500);

            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            base.Initialize();
            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);

            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));

            coinsCollected = 0;
            coins = new List<Rectangle>();
            coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));

            exitRect = new Rectangle(700, 380, 100, 100);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            pacLeftTexture = Content.Load<Texture2D>("PacLeft");
            pacRightTexture = Content.Load<Texture2D>("PacRight");
            pacUpTexture = Content.Load<Texture2D>("PacUp");
            pacDownTexture = Content.Load<Texture2D>("PacDown");
            currentPacTexture = pacRightTexture;

            exitTexture = Content.Load<Texture2D>("hobbit_door");
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            coinTexture = Content.Load<Texture2D>("coin");
            coinFont = Content.Load<SpriteFont>("Coin Count");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;

                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Right;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;

                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Left - pacRect.Width;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;

                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Bottom;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;

                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Top - pacRect.Height;
                    }
            }

            if (pacRect.Right > window.Width)
            {
                pacRect.X = 740;
            }
            if (pacRect.Left < 0)
            {
                pacRect.X = 0;
            }
            if (pacRect.Bottom > window.Height)
            {
                pacRect.Y = 440;
            }
            if (pacRect.Top < 0)
            {
                pacRect.Y = 0;
            }

            for (int i = 0; i < coins.Count; i++)
            {
                for (int j = 0; j < coins.Count; j++)
                {
                    if (i != j && coins[i].Intersects(coins[j]))
                    {
                        coins.RemoveAt(i);
                        coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
                    }
                }
                foreach (Rectangle barrier in barriers)
                    if (coins[i].Intersects(barrier))
                    {
                        coins.RemoveAt(i);
                        coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
                    }
                if (coins[i].Intersects(exitRect))
                {
                    coins.RemoveAt(i);
                    coins.Add(new Rectangle(generator.Next(0, (800 - coinTexture.Width)), generator.Next(0, (500 - coinTexture.Height)), coinTexture.Width, coinTexture.Height));
                }
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                    coinsCollected++;
                }
            }
            if (exitRect.Contains(pacRect) && mouseState.LeftButton == ButtonState.Pressed)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            _spriteBatch.DrawString(coinFont, "Coins Collected: " + coinsCollected, new Vector2(0, 0), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
