using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    class Jogador:Animated_Sprite
    {
        private enum SoldierStatus
        {
            Player,
            Patrolling,
            Walking,
            Attacking,
            Dead
        }

       // private Animated_Sprite mp40smoke;
        private Sprite ironsight;
        private int deltaScrollWheelValue = 0;
        private int currentScrollWheelValue = 0;
        private float range = 1.2f;
        private int mainMagAmmo;
        private int mainMagazines;
        private int weaponcapacity;
        private Random randomSMG;

        private Vector2 pos;
        MouseState mstate = Mouse.GetState();
        private Vector2 soldierDirection;
        private float turnValue;
        private float soldRot;
        private float stopspeed;
        private SoldierStatus soldierS = SoldierStatus.Patrolling;
        //AI 
        public float patrolRadius = 3f; // começa a andar
        public float attackRadius = 1.2f; // começa a disparar
        private Sprite target;
        private float velocidade = 0.4f;
        private float fireInterval = 1f;
        private float fireCounter = 0f;
        //AI work
        private float rot;
        private bool npc;

        ContentManager content;
        //Collider collider;


        KeyboardState state = Keyboard.GetState();

        public Jogador(ContentManager content, Sprite target, bool npc)
            : base(content, "infantry/InfantryRun4", 1, 5, 1f/7f,false)
        {
            this.Scl(0.25f);
            this.soldierDirection = new Vector2((float)Math.Sin(soldRot),
                 (float)Math.Cos(soldRot));
            this.ironsight = new Sprite(cManager, "infantry/Mp40_256");
            this.ironsight.Scl(0.4f);
            this.ironsight.SetRotation((float)Math.PI / 4);

            this.EnableCollisions();

            this.mainMagAmmo = 30;
            this.weaponcapacity = 30;
            this.mainMagazines = 3;
            this.randomSMG = new Random();
            mstate = Mouse.GetState();
            soldierS = SoldierStatus.Patrolling;
            this.npc = npc;

            // AI prof
            this.target = target;
            this.content = content;
            if (npc == false)
            {
                soldierS = SoldierStatus.Player;
            }

        }



        public override void Draw(GameTime gameTime)
        {
            if (state.IsKeyDown(Keys.V))
                {
                    ironsight.Draw(gameTime);
                }

            base.Draw(gameTime);

        }
        public override void SetScene(Scene s)
        {
            this.scene = s;
            ironsight.SetScene(s);


        }
        public override void Destroy()
        {
            //ironsight.Destroy();
            base.Destroy();
                
        }

        public override void Update(GameTime gameTime)
        {
            // AI PROF

            if (soldierS == SoldierStatus.Patrolling || soldierS == SoldierStatus.Walking || soldierS == SoldierStatus.Attacking)
            {

                fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 direction = target.position - this.position;
  
                float sqDistance = direction.LengthSquared();
                if (this.soldierS == SoldierStatus.Patrolling && (target.position - this.position).LengthSquared() < patrolRadius * patrolRadius){
                    this.soldierS = SoldierStatus.Walking;
                }

                else if (this.soldierS == SoldierStatus.Walking)
                {
                    if (sqDistance < attackRadius * attackRadius)
                        soldierS = SoldierStatus.Attacking;
                    else if (sqDistance > patrolRadius * patrolRadius)
                        soldierS = SoldierStatus.Patrolling;
                    else
                    {
                        this.switchstyle = 1;
                        direction.Normalize();
                        this.LookAt(direction);
                        Vector2 oldPosition = this.position;
                        this.SetPosition(this.position + direction * (float)gameTime.ElapsedGameTime.TotalSeconds * this.velocidade);
                        //if (Collides())
                            //this.position = oldPosition;

                    }
                }

                else if (this.soldierS == SoldierStatus.Attacking)
                {
                    if (sqDistance > patrolRadius * patrolRadius) soldierS = SoldierStatus.Patrolling;
                    else if (sqDistance > attackRadius * attackRadius) soldierS = SoldierStatus.Walking;
                    else
                    {
                        this.switchstyle = 3;
                        if (fireCounter > fireInterval)
                        {
                            fireCounter = 0f;
                            Bullet bullet = new Bullet(cManager, this.position, (float)Math.Atan2(direction.X, direction.Y), range, true);
                            direction.Normalize();
                            this.LookAt(direction);
                            bullet.LookAt(direction);
                            int r = (int)(Game1.RandomNumber(1, 4));
                            Som.firesmg(r, 0.2f);
                            scene.AddSprite(bullet);

                        }

                    }
                }

            } // Patrolling


            if (soldierS == SoldierStatus.Player)
            {
                this.fireInterval = 0.2f;
                mstate = Mouse.GetState();
                state = Keyboard.GetState();
                deltaScrollWheelValue = mstate.ScrollWheelValue - currentScrollWheelValue;
                currentScrollWheelValue += deltaScrollWheelValue;

                Point mpos = mstate.Position;
                Vector2 tpos = Camera.WorldPoint2Pixels(position);
                float a = (float)mpos.Y - tpos.Y; // busca a posição Y(coordenadas da camara Pixeis) do rato e subtrai a posição do tank(camara centro)
                float l = (float)mpos.X - tpos.X;

                float rot = (float)Math.Atan2(a, l);
                rot += (float)Math.PI / 2f;
                this.SetRotation(rot);

                ironsight.SetPosition(this.position + new Vector2((float)Math.Sin(rot) * (range + 20 / Camera.ratio),
                 (float)Math.Cos(rot) * (range + 20 / Camera.ratio)));
                ironsight.SetRotation(rot);

                if (deltaScrollWheelValue > 0)
                {
                    if (range < 3f)
                    {
                        range += 0.2f;

                    }
                    else
                    {
                        range = 3f;
                    }
                }
                else if (deltaScrollWheelValue < 0)
                {
                    if (range > 0.3f)
                    {
                        range -= 0.2f;
                    }
                    else
                    {
                        range = 0.3f;
                    }
                }
                if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S))
                {
                    if (state.IsKeyDown(Keys.W))
                    {
                        velocidade += 0.001f;
                        if (velocidade > 0.02f)
                            velocidade = 0.02f;


                    }
                    else if (state.IsKeyDown(Keys.S))
                    {
                        velocidade -= 0.001f;
                        if (velocidade < -0.01f)
                            velocidade = -0.01f;
                        this.switchstyle = 1;

                    }
                    this.switchstyle = 1;
                    this.soldierDirection = new Vector2((float)Math.Sin(rot),
              (float)Math.Cos(rot));
                    this.position = this.position + soldierDirection * velocidade;
                }
                if ((state.IsKeyUp(Keys.S)) && (state.IsKeyUp(Keys.W)))
                {

                    if (velocidade > 0) // Travões caso a velocidade ainda esteja superior a 0
                    {
                        velocidade -= stopspeed; // Travões reduz a velocidade mais rápido que o atrito


                    }
                    else if (velocidade < 0) // caso a velocidade seja 0 ou menor que zero/ esteja parado ou em marcha atras
                    {
                        velocidade -= stopspeed;
                    }
                    this.switchstyle = 3;
                    velocidade = 0;

                }

                if (state.IsKeyDown(Keys.A))
                {
                    velocidade += 0.01f;
                    if (velocidade > 0.02f)
                        velocidade = 0.02f;
                    this.switchstyle = 1;
                    this.soldierDirection = new Vector2(-(float)Math.Cos(rot),
                 (float)Math.Sin(rot));
                    this.position = this.position + soldierDirection * velocidade;

                }
                if (state.IsKeyDown(Keys.D))
                {
                    velocidade += 0.01f;
                    if (velocidade > 0.02f)
                        velocidade = 0.02f;
                    this.switchstyle = 1;
                    this.soldierDirection = new Vector2((float)Math.Cos(rot),
                -(float)Math.Sin(rot));
                    this.position = this.position + soldierDirection * velocidade;

                }


                fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (mainMagAmmo > 0)
                {
                    if (fireCounter >= fireInterval &&
                        mstate.LeftButton == ButtonState.Pressed)// &&  (mstate.RightButton == ButtonState.Pressed))//state.IsKeyDown(Keys.V))
                    {


                        pos = this.position + new Vector2((float)Math.Cos(rot - Math.PI / 2) * 25 / Camera.ratio,
                            (float)Math.Sin(rot - Math.PI / 2) * -25 / Camera.ratio);
                        this.origemf = 1;
                        this.destinof = 7;
                        Animated_Sprite fireSmoke;
                        fireSmoke = new Animated_Sprite(content, "infantry/MP40SmokeS3", 1, 5, 1f / 7f, true);
                        fireSmoke.position = pos;
                        fireSmoke.SetRotation(rot);
                        fireSmoke.Scl(0.8f);
                        fireSmoke.switchstyle = 0;
                        scene.AddSprite(fireSmoke);

                        Bullet bullet = new Bullet(cManager, pos, rot, range, true);
                        scene.AddSprite(bullet);

                        fireCounter = 0f;
                        int r = (int)(Game1.RandomNumber(1, 4));
                        Som.firesmg(r, 0.2f);
                        mainMagAmmo -= 1;

                    }
                }
                else
                {
                    if (state.IsKeyDown(Keys.R) && (mainMagazines > 0))
                    {
                        Som.firesmg(5, 0.2f);
                        mainMagAmmo = weaponcapacity;
                        mainMagazines -= 1;
                    }
                    else if (mstate.LeftButton == ButtonState.Pressed && fireCounter >= fireInterval)
                    {
                        Som.firesmg(4, 0.2f);
                        fireCounter = 0f;
                    }
                }
                stopspeed = velocidade / 2;
                ironsight.Update(gameTime);
                Camera.SetTarget(this.position);
            }
            //this.SetRotation(rot);
            //this.position = this.position + soldierDirection * velocidade;



            base.Update(gameTime);
        }
    }
}