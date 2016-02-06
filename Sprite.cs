using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    class Sprite 
    {
        public bool hasCollisions { protected set; get; }
        public bool hasCollisionR { protected set; get; }

        protected Texture2D image;
        public Vector2 position; // posição no mundo virtual
        protected float radius; // raio da bounding box
        protected Rectangle primitiveCollison;
        protected Vector2 size;
        public Vector2 origem;
        protected float rotation;
        protected Scene scene;
        protected Vector2 pixelSize; // tamanho da sprite em pixeis, para uso nas animações
        protected Rectangle? source = null; // ? permite atribuir null, rectangulo de origem da sprite para ser imprimida no ecrã
        protected Color[] pixels;
        protected ContentManager cManager;
        public string name;
        protected Color mTintColor; 


        public Sprite(ContentManager contents, String assetName)
        {
            this.cManager = contents;
            this.hasCollisions = false;
            this.hasCollisionR = false;
            this.rotation = 0f;
            this.position = Vector2.Zero;
            this.image = contents.Load<Texture2D>(assetName);
            this.pixelSize = new Vector2(image.Width, image.Height);
            this.size = new Vector2(1f, (float)image.Height / (float)image.Width); //?
            this.name = assetName;
            this.origem = new Vector2(pixelSize.X / 2, pixelSize.Y / 2); // centro da imagem em pixeis
            //this.centroRot = new Vector2(pixelSize.X / 2, pixelSize.Y / 2);

            mTintColor = Color.White;
        }

        public bool collidesWidth(Sprite other, out Vector2 collisionPoint)
        {
            collisionPoint = position; // cale o compilador
            if (!this.hasCollisions) return false;
            if (!other.hasCollisions) return false;

            float distance = (this.position - other.position).Length();

            if ( distance > this.radius + other.radius) return false;

            //
            return this.PixelTouches(other,out collisionPoint);
        }


        public virtual void EnableCollisions()
        { // diz que temos colisºoes
            this.hasCollisions = true;
            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));

            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            image.GetData<Color>(pixels);

        }

        public virtual void EnableCollisionsR(Vector2 posMundo, int X, int Y)
        { // diz que temos colisºoes
            this.hasCollisions = true;
            this.primitiveCollison = new Rectangle((int)posMundo.X, (int)posMundo.Y, X, Y); // rectangulo criado no centro da sprite

            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            image.GetData<Color>(pixels);

        }

        public bool Intersect(Sprite other, out Vector2 collisionPoint) //  permite saber a ultima posição da colisão out altera o collisionPoint
        {
            //se não houver colisão o ponto de colisão é returnoado e 
            // a posição da Sprite podia ser outro valor qualquer

            collisionPoint = position;

            bool touches = false;

            int i = 0;
            while (touches == false && i < pixelSize.X)
            {
                int j = 0;
                while (touches == false && j < pixelSize.Y)
                {
                    if (GetColorAt(i, j).A > 0)
                    {
                        Vector2 CollidePoint = ImagePixelToVirtualWorld(i, j);
                        Vector2 otherPixel = other.VirtualWorldPointToImagePixel(CollidePoint);

                        if (otherPixel.X >= 0 && otherPixel.Y >= 0 &&
                            otherPixel.X < other.pixelSize.X &&
                            otherPixel.Y < other.pixelSize.Y)
                        {
                            if (other.GetColorAt((int)otherPixel.X, (int)otherPixel.Y).A > 0)
                            {
                                touches = true;
                                collisionPoint = CollidePoint;
                            }
                        }

                    }
                    j++;
                }
                i++;
            }
            return touches;

        }



        public Color GetColorAt(int x, int y)
        {
            // se não huver collider da erro!
            return pixels[x + y * (int)pixelSize.X];
        }

        private Vector2 ImagePixelToVirtualWorld(int i, int j)
        {
            float x = i * size.X / (float)pixelSize.X;
            float y = j * size.Y / (float)pixelSize.Y;
            return new Vector2(position.X + x - (size.X * 0.5f),
                                                                 position.Y - y + (size.Y * 0.5f));
        }

        private Vector2 VirtualWorldPointToImagePixel(Vector2 p)
        {
            Vector2 delta = p - position;
            float i = delta.X * pixelSize.X / size.X; // regra de 3 simples
            float j = delta.Y * pixelSize.Y / size.Y;

            i += pixelSize.X * 0.5f;
            j = pixelSize.Y * 0.5f-j;

            return new Vector2(i, j);
        }

        public bool PixelTouches(Sprite other, out Vector2 collisionPoint) //  permite saber a ultima posição da colisão out altera o collisionPoint
        {
            //se não houver colisão o ponto de colisão é returnoado e 
            // a posição da Sprite podia ser outro valor qualquer

            collisionPoint = position;

            bool touches = false;

            int i = 0;
            while (touches == false && i < pixelSize.X)
            {
                int j = 0;
                while (touches == false && j < pixelSize.Y)
                {
                    if (GetColorAt(i, j).A > 0)
                    {
                        Vector2 CollidePoint = ImagePixelToVirtualWorld(i, j);
                        Vector2 otherPixel = other.VirtualWorldPointToImagePixel(CollidePoint);

                        if (otherPixel.X >= 0 && otherPixel.Y >= 0 &&
                            otherPixel.X < other.pixelSize.X &&
                            otherPixel.Y < other.pixelSize.Y)
                        {
                            if (other.GetColorAt((int)otherPixel.X, (int)otherPixel.Y).A > 0)
                            {
                                touches = true;
                                collisionPoint = CollidePoint;
                            }
                        }

                    }
                    j++;
                }
                i++;
            }
            return touches;

        }
       

        public virtual void Scale(float scale)
        {
            this.size *= scale;
        }
        public virtual void SetScene(Scene s)
        {
            this.scene = s;
        }
        public Sprite Scl(float scale)
        {
            this.Scale(scale);
            return this;
        }

        public void LookAt(Vector2 p)
        {
            this.rotation = (float)Math.Atan2(p.X,p.Y);
        }
        public virtual void Draw(GameTime gameTime)
        {
            Rectangle pos = Camera.WorldSize2PixelRectangle(this.position, this.size); //? this.position posição da sprite no mundo virtual, tamanho da sprite no mundo virtual size 
           // Console.WriteLine("pos X "+pos.X+"pos Y"+ pos.Y+"pos Width "+pos.Width+"pos Height "+pos.Height);

            // pos a posição da sprite na camera, mundo começa a desenhar/coordenadas no canto inferior esquerdo
            // retorna o tamanho da sprite em pixeis camera
            // scene.SpriteBatch.Draw(this.image, pos, Color.White);
            scene.SpriteBatch.Draw(this.image, pos, source, mTintColor, //Adds a sprite to a batch of sprites for rendering using the specified texture,  
            this.rotation, this.origem,// O centro da sprite position, source rectangle, color, rotation, origin, lle, effects and layer.
            SpriteEffects.None, 0);
        }

        public virtual void SetRotation(float r)
        {
            this.rotation = r;
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Dispose()
        {
            this.image.Dispose();
        }
        public virtual void Destroy()
        {
            this.scene.RemoveSprite(this);
        }
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
        public Sprite At(Vector2 p)
        {
            this.SetPosition(p);
            return this;
        }

    }
}
