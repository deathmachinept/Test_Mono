using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    public class UI 
    {
        private Texture2D engine;
        private Texture2D tracks;
        private Texture2D ammobox;     
        private Texture2D AP;
        private Texture2D HE;
        private Texture2D backgroundUI;
        public SpriteFont Font;
        public SpriteBatch x;


        public static string ammoValue { set; get; }
        public static string apValue { set; get; }
        public static string heValue { set; get; }
        public UI(ContentManager content,SpriteBatch x)
        {
            this.x = x;
            this.ammobox = content.Load<Texture2D>("ui/ammo box2");
            this.engine = content.Load<Texture2D>( "ui/new_tank_engine");
            this.tracks = content.Load<Texture2D>( "ui/new_tank_traks");
            this.AP = content.Load<Texture2D>( "ui/APTigerF");
            this.HE = content.Load<Texture2D>("ui/HeTigerF");
            this.backgroundUI = content.Load<Texture2D>("ui/Background");
            this.Font = content.Load<SpriteFont>("fonts/myUiFont");
            ammoValue = "800";
            apValue = "40";
            heValue = "20";
            //this.engine.SetPosition(new Vector2((float)1f, (float)1f));
        }



        public void Draw(GameTime gameTime)
        {
            x.Begin();

            int altura = x.GraphicsDevice.Viewport.Height;
            int largura = x.GraphicsDevice.Viewport.Width;
            x.Draw(backgroundUI, new Vector2((largura / 2)-110, altura - 105), Color.White);
            x.Draw(engine, new Vector2((largura / 2) - 50, altura - 100), Color.White);
            x.Draw(tracks, new Vector2((largura/2)-105, altura - 100), Color.White);
            x.Draw(AP, new Vector2((largura / 2) -1, altura - 100), Color.White);
            x.Draw(HE, new Vector2((largura / 2) + 30, altura - 101), Color.White);
            x.Draw(ammobox, new Vector2((largura / 2)+ 80, altura - 93), Color.White);

            x.DrawString(Font, apValue, new Vector2((largura / 2) + 12, altura - 73), Color.White);
  
            x.DrawString(Font, heValue, new Vector2((largura / 2) + 45, altura - 73), Color.White);
            x.DrawString(Font, ammoValue, new Vector2((largura / 2) + 97, altura - 73), Color.White);

            x.End();
        }

    }
}
