#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace TP_Afrika_Korps
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public enum GameState
        {
            Singleplayer,
            Level1,
        }
        static public GraphicsDeviceManager graphics;
        static public SpriteBatch spriteBatch;
        public UI ui;
        static public Random seed = new Random();
        GameState jogo;
        Scene scene;
        //public static List<Sprite> inimigos = new List<Sprite>();

        static public float RandomNumber(float n)
        {
            return (float)(seed.NextDouble() * n);
        }
        static public float RandomNumber(float min, float max)
        {
            return min + ((max - min) * (float)(seed.NextDouble()));
        }


        int currentH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int currentW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

        private int kWindowWidth;
        private int kWindowHeight;
        public Game1()
            : base()
        {
            Content.RootDirectory = "Content";

            Game1.seed = new Random();
            Game1.graphics = new GraphicsDeviceManager(this);

                //currentH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
               // currentW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

                kWindowWidth = (int)(currentW * 0.85f);
                kWindowHeight = (int)(currentH * 0.85f);

            Game1.graphics.PreferredBackBufferWidth = kWindowWidth;
            Game1.graphics.PreferredBackBufferHeight = kWindowHeight;

            jogo = GameState.Level1;
        }
        protected override void Initialize()
        {
            /*graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();*/

            
            Camera.SetGraphicsDeviceManager(graphics);
            Camera.SetTarget(Vector2.Zero);
            Camera.SetWorldWidth(7);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            scene = new Scene(spriteBatch);
            Som.LoadContent(Content);


                ui = new UI(Content,spriteBatch);
                Sprite tankJogador;
                Sprite inimigo;
                Sprite palm;
                //Sprite soldadoJogador;
                SlidingBackground sand = new SlidingBackground(Content, "Ground3");
                scene.AddBackground(sand);
                scene.AddSprite(tankJogador = new Tank(Content, "tank/TigerBase01", "tiger"));



                scene.AddSprite(palm = new Sprite(Content, "Palm6-Myadjst").At(new Vector2(2f, 2f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm7-Myadjst").At(new Vector2(2f, -2f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm6-Myadjst").At(new Vector2(6f, 5f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm6-Myadjst").At(new Vector2(2f, 3f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm7-Myadjst").At(new Vector2(3f, -3f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm7-Myadjst").At(new Vector2(4f, -3f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm6-Myadjst").At(new Vector2(-3f, 2f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm7-Myadjst").At(new Vector2(-4f, 2f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm6-Myadjst").At(new Vector2(-6f, 5f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm6-Myadjst").At(new Vector2(-2f, 3f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm7-Myadjst").At(new Vector2(-3f, 3f)));
                scene.AddSprite(palm = new Sprite(Content, "Palm7-Myadjst").At(new Vector2(-3f, 3f)));

                //scene.AddSprite(tankJogador = new Jogador(Content, palm, false));

                //scene.AddSprite(palm = new Sprite(Content, "Palm5-Myadjst").At(new Vector2(3f, -1f)));
                //scene.AddSprite(tankJogador = new Tank(Content, "tank/ShermanBase", "sherman"));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(8f, 8f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(6f, 8f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(7f, 7f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(7f, 7f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(-2.3f, 3.1f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(-2.3f, 3.2f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(-2.2f, 3.3f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(3f, 2.2f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(3f, 2.3f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(3f, 2.4f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(3f, 2.35f)));
                scene.AddSprite(inimigo = new Jogador(Content, tankJogador, true).At(new Vector2(3f, 3.5f)));
                //inimigos.Add(inimigo);
                //scene.AddSprite(inimigo = new Tank(Content, tankJogador,true).Scl(1.3f).At(new Vector2(7f, 8f)));
               // inimigos.Add(inimigo);


        }
        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
            scene.Dispose();
        }
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (!graphics.IsFullScreen) // nao esta fullscreen
                {
                    Game1.graphics.PreferredBackBufferWidth = currentW;
                    Game1.graphics.PreferredBackBufferHeight = currentH;
                    graphics.ToggleFullScreen();
                    graphics.ApplyChanges();

                }
                else if (graphics.IsFullScreen) //esta full screen
                {
                    Game1.graphics.PreferredBackBufferWidth = (int)(currentW * 0.85f + 0.5f);
                    Game1.graphics.PreferredBackBufferHeight = (int)(currentH * 0.85f + 0.5f);
                    
                    //Console.WriteLine((int)(currentH * 0.85f));
                    //Console.WriteLine(Game1.graphics.PreferredBackBufferHeight);

                    graphics.ToggleFullScreen();
                    graphics.ApplyChanges();
                }
            }


            /*if (Keyboard.GetState().IsKeyDown(Keys.J))
            {

                scene.AddSprite(soldadoJogador = new Jogador(Content, inimigo, false).Scl(1.3f).At(this.position));

            }*/
            scene.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            scene.Draw(gameTime);
            ui.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
