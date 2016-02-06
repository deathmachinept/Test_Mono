using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
        class Bullet : Sprite
        {
            public float maxDistance = 2;
            public float velocity = 12;
            public float airAtrition = 0.01f;
            private Vector2 sourcePosition;
            private Vector2 direction;
            private bool tracer;
            public bool sandhit;

            public Bullet(ContentManager cManager,
                          Vector2 sourcePosition,
                          float rotation, float maxDistance, bool tracer)
                : base(cManager, "infantry/Tracer")
            {
                this.position = sourcePosition; //11px no turret
                this.sourcePosition = sourcePosition;
                this.rotation = rotation;
                this.Scale(0.1f);
                this.maxDistance = maxDistance;
                this.direction = new Vector2((float)Math.Sin(rotation),
                                             (float)Math.Cos(rotation));
                this.tracer = tracer;
                mTintColor.R += 255;
                mTintColor.G += 0;
                mTintColor.B += 0;
                this.sandhit = true;
                this.EnableCollisions();
            }

            public override void Update(GameTime gameTime){

                position = position + direction * velocity *
                      (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(velocity >6){
                    velocity = velocity - (velocity * airAtrition);
                    airAtrition = airAtrition + 0.02f;
                }

                Sprite other;
                Vector2 colPosition;


                if ((position - sourcePosition).Length() > maxDistance) 
                {
                    foreach (Sprite s in scene.sprites.ToList())
                    {
                        if (s.name == "infantry/InfantryRun4" || s.name == "tank/TigerTurretFireOpenFix" || s.name == "tank/TigerBase01")
                        {
                            float d = (this.position - s.position).Length();
                            if (d < 0.3f)
                            {
                                if (s.name == "tank/TigerTurretFireOpenFix" || s.name == "tank/TigerBase01") {
                                    Animated_Sprite noDamage;// adicionar loops
                                    noDamage = new Animated_Sprite(cManager, "BulletMetal", 1, 6, 1 / 9f, true);
                                    noDamage.switchstyle = 0;
                                    scene.AddSprite(noDamage);

                                    noDamage.SetPosition(this.position);
                                    noDamage.Scale(0.6f);
                                    noDamage.SetRotation(rotation);
                                    sandhit = false;
                                }
                                else
                                {
                                    Sprite deadSoldier;
                                    int r = (int)(Game1.RandomNumber(1f, 3f));
                                    if (r == 1)
                                    {
                                        deadSoldier = new Sprite(cManager, "infantry/Dead02-2");
                                    }
                                    else
                                    {
                                        deadSoldier = new Sprite(cManager, "infantry/Dead01-1");
                                    }
                                    deadSoldier.SetPosition(this.position);
                                    deadSoldier.SetRotation(this.rotation);
                                    deadSoldier.Scl(0.5f);
                                    scene.AddSprite(deadSoldier);
                                    s.Destroy();
                                }
                            }
                        }
                    }

                    this.Destroy();
                }

                base.Update(gameTime);
            }

            public override void Draw(GameTime gameTime)
            {

               if (tracer == true)
               {
                    base.Draw(gameTime);
               }
            }

            public override void Destroy(){
                if (sandhit == true) {
                Animated_Sprite explosion;// adicionar loops
                explosion = new Animated_Sprite(cManager, "BulletHitFix", 1, 10,1/9f,true);
                explosion.switchstyle = 0;
                scene.AddSprite(explosion);

                explosion.SetPosition(this.position);
                explosion.Scale(0.6f);
                explosion.SetRotation(rotation);
                }
                
                base.Destroy();
                
            }
        }
    }
