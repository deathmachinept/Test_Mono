using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
        class GunFire : Animated_Sprite
        {
            public float maxDistance = 2;
            public float velocity = 12;
            private Vector2 sourcePosition;
            private Vector2 direction;
            private bool tracer;

            public GunFire(ContentManager cManager,
                          Vector2 sourcePosition,
                          float rotation, bool tracer)
                : base(cManager, "infantry/mp 40",1,7,1/12,true)
            {
                this.position = sourcePosition; //11px no turret
                this.rotation = rotation;
                this.Scale(1.2f);
                this.direction = new Vector2((float)Math.Sin(rotation),
                                             (float)Math.Cos(rotation));
                this.tracer = tracer;
            }

            public override void Update(GameTime gameTime){

                position = position + direction;
                if ((position - sourcePosition).Length() > maxDistance) 
                {
                    
                    float valor = (position - sourcePosition).Length();
                    this.Destroy();
                }
                base.Update(gameTime);
            }

            public override void Draw(GameTime gameTime)
            {

              // if (tracer == true)
               //{
                    base.Draw(gameTime);
               //}
            }

            public override void Destroy(){
               /* Animated_Sprite explosion;// adicionar loops
                explosion = new Animated_Sprite(cManager, "BulletHit", 1, 14,1/8f,true);
                explosion.switchstyle = 0;
                scene.AddSprite(explosion);
                explosion.SetPosition(this.position);
                explosion.Scale(0.5f);
                explosion.SetRotation(rotation);*/
                
                base.Destroy();
                
            }
        }
    }


