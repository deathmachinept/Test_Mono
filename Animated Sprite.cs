using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{



    class Animated_Sprite : Sprite
    {
        enum animationtype
        {
            normal,
            oneframe,
            setorigem,
            stop,
        };

        private int ncols, nrows;
        private Point currentFrame;
        private float animationInterval = 1f / 12f;
        private float animationTimer = 0f;
        private bool destroyframe;
        animationtype type;


        
        public bool Loop { get; set; }
        public int origemf { get; set; }
        public int destinof { get; set; }
        public int row { get; set; }
        public int collumn { get; set; }

        public int switchstyle { get; set; }

        public Animated_Sprite(ContentManager contents, String fileName, int nrows, int ncols,float timer,bool destroyf)
            : base(contents, fileName)
        {
            this.ncols = ncols; //numero de colunas na sprite sheet X
            this.nrows = nrows; //numero de linhas na sprite sheet X

            this.animationInterval = timer; //intervalo até recomeço

            this.pixelSize.X = this.pixelSize.X / ncols; //tamanho da spritesheed, numero de colunas
            this.pixelSize.Y = this.pixelSize.Y / nrows; //tamanho da spritesheed, numero de linhas
            this.type = animationtype.stop;
            this.size = new Vector2(1f, (float)pixelSize.Y / (float)pixelSize.X);
            this.origem = this.pixelSize * 0.5f; // centro da imagem/spritesheet
            this.origemf = 0;
            this.destinof = 0;
            this.row = 0;
            this.collumn = 0;

            this.currentFrame = Point.Zero; //frame actual (0,0)

            this.switchstyle = 3;
            this.destroyframe = destroyf;

         }

        public void nextFrame()
        {
            if (currentFrame.X < ncols - 1) //0<10 -1(coordenadas começão em 0 e é passado apenas valores positivos)
            {
                currentFrame.X++; 
            }
            else if (currentFrame.Y < nrows - 1)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
            }
            else
                Destroy();
        }
        public void framebyframe(){
            if (currentFrame.X < ncols - 1)
            {
                currentFrame.X++;
            }
            else if (currentFrame.Y < nrows - 1)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
                // }else if()
            }
            else if (this.destroyframe == true)
            {
                Destroy();
            }
            else
            {
                currentFrame.X = 0;
            }

    }
        public void frameset()
        {

            if (origemf < destinof - 1)
            {
                origemf++;
                currentFrame.X++;
                currentFrame.Y = row;
            }else if(this.destroyframe == true){
                Destroy();
            }
            else
            {
                currentFrame.X = 0;
                currentFrame.Y = row;
            }

        }

        public void changeFrameStart()
        {
            this.type = animationtype.stop;
            currentFrame.X = collumn;
            currentFrame.Y = row;
        }
        public override void Update(GameTime gameTime)
        {

            if (switchstyle == 0)
            {
                this.type = animationtype.normal;
            }
            if (switchstyle == 1)
            {
                this.type = animationtype.oneframe;
            }
            else if (switchstyle == 2)
            {
                this.type = animationtype.setorigem;
                if (origemf != 0)
                {
                    currentFrame.X = origemf;
                }
            }
            else if (switchstyle == 3)
                this.type = animationtype.stop;



            if (type == animationtype.normal)
            {
                animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (animationTimer > animationInterval)
                    {
                        animationTimer = 0f;
                        nextFrame();
                    }
            }
            if (type == animationtype.oneframe)
            {
                animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > animationInterval)
                {
                    animationTimer = 0f;
                    framebyframe();
                }
            }
            if (type == animationtype.setorigem)
            {
                animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > animationInterval)
                {
                    animationTimer = 0f;
                    frameset();

                }

            }
            if (type == animationtype.stop)
                changeFrameStart();
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {

            source = new Rectangle((int)currentFrame.X * (int)pixelSize.X, 
                (int)currentFrame.Y * (int)pixelSize.Y, (int)pixelSize.X, (int)pixelSize.Y);
            base.Draw(gameTime);
        }
        public override void EnableCollisions()
        { // diz que temos colisºoes
            this.hasCollisions = true;
            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));


            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            image.GetData<Color>(0, new Rectangle(
            (int)(currentFrame.X * size.X),
            (int)(currentFrame.Y * size.Y),
            (int)pixelSize.X,
            (int)pixelSize.Y),
            pixels, 0,
            (int)(pixelSize.X * pixelSize.Y));
        }
    }
}
