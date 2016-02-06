using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    class Scene // centraliza o draw de sprites
    {
        public SpriteBatch SpriteBatch { get; private set; } 
        public List<Sprite> sprites;
        private List<SlidingBackground> backgrounds;
        public List<ParticlePrimitive> particles;

        public Scene(SpriteBatch sb)
        {
            this.SpriteBatch = sb; //cria um objecto na cena
            this.sprites = new List<Sprite>();
            this.backgrounds = new List<SlidingBackground>();
            this.particles = new List<ParticlePrimitive>();
        }
        public void AddSprite(Sprite s)
        {
            this.sprites.Add(s);
            s.SetScene(this);
        }

        public void AddBackground(SlidingBackground b)
        {
            this.backgrounds.Add(b);
            b.SetScene(this);
        }

        public void RemoveSprite(Sprite s)
        {
            this.sprites.Remove(s);
        }
        public void Update(GameTime gameTime)
        {
            foreach (var sprite in sprites.ToList())
            {
                sprite.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime)
        {
            if (sprites.Count > 0 || backgrounds.Count > 0)
            {
                this.SpriteBatch.Begin();
                //Desenhar os fundos
                foreach (var background in backgrounds)
                    background.Draw(gameTime);

                //Desenhar as sprites|
                foreach (var sprite in sprites)
                {
                    sprite.Draw(gameTime);
                }
                this.SpriteBatch.End();
            }
        }

        public bool collides(Sprite s, out Sprite collided, out Vector2 collisionPoint)
        {
            bool collisionExists = false;
            collided = s;
            collisionPoint = Vector2.Zero;
            foreach (var sprite in sprites){
                if (s == sprite || sprite.name == "infantry/Tracer") continue;
                if (s.collidesWidth(sprite, out collisionPoint))
                {
                    collisionExists = true;
                    collided = sprite;
                    break;
                }
            }
            return collisionExists;
        }
        public void Dispose()
        {
            foreach (var sprite in sprites)
            {
                sprite.Dispose();
            }

            foreach (var background in backgrounds)
                background.Dispose();
        }


    }
}
