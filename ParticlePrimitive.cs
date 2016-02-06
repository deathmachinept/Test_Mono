using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    class ParticlePrimitive:Sprite
    {
      private float kLifeSpanRandomness = 0.4f;
        private float kSizeChangeRandomness = 0.5f;
        private float kSizeRandomness = 0.3f;
        private float kSpeedRandomness = 0.1f;
        // Number of updates before a particle disappear
        private int mLifeSpan;
        // How fast does the particle changes size
        private float mSizeChangeRate;
        private Vector2 mVelocityDir;

        public ParticlePrimitive(ContentManager content, Vector2 position, float size, int lifeSpan) :
            base(content,"Tracer")
            {
            mLifeSpan =(int)(lifeSpan * Game1.RandomNumber(-kLifeSpanRandomness,
            kLifeSpanRandomness));
            mVelocityDir.X = Game1.RandomNumber(-0.5f, 0.5f);
            mVelocityDir.Y = Game1.RandomNumber(-0.5f, 0.5f);
            mVelocityDir.Normalize();
            //mSpeed = Game1.RandomNumber(kSpeedRandomness);
            mSizeChangeRate = Game1.RandomNumber(kSizeChangeRandomness);
           // pixelSize.X *= Game1.RandomNumber(1f - kSizeRandomness, 1 + kSizeRandomness);
           // pixelSize.Y = mSize.X;
            }
        /*
        public override void Update()
        {
            //base.Update();
            //BASE
            /*
            protected void InitGameObject() {
            mVelocityDir = Vector2.Zero;
            mSpeed = 0f;
            }
             Update()
             mPosition += (mVelocityDir * mSpeed);
             */

        /*
            mLifeSpan--; // Continue to approach expiration
            // Change its size
            mSize.X += mSizeChangeRate;
            mSize.Y += mSizeChangeRate;
            // Change the tintcolor randomly
            Byte[] b = new Byte[3];
            Game1.seed.NextBytes(b);
            mTintColor.R += b[0];
            mTintColor.G += b[1];
            mTintColor.B += b[2];
        }*/
    }
}
