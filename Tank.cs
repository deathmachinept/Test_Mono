using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    class Tank : Sprite
    {
        enum EngineStatus
        {
            on,
            off,
            fire,
            damaged,
            destroyed,
        };

        enum TankMode
        {
            Player,
            OnFoot,
            Patrolling,
            Walking,
            Attacking
        }
        private Animated_Sprite turret;
        private Sprite gunsight;
        public bool ai;
        private int hatchesOpen;
        private bool clicked;

        private float fireInterval = 2f;
        private float fireCounter = 0f;
        private float mgfireInterval = 0.1f;
        private float mgfireCounter = 0f;
        private Vector2 centroRotacao;
        private float tankEixo = 127f;
        private float velocidadeMax;
        private float reverseMax;
        private float velocidade;
        private int velocidadePositiva;
        private float turnValue;
        private float tankRot;
        private float distancia;
        private int municaoAP;
        private int municaoHE;
        private int municaoCoax;
        private float range;
        private Vector2 pos;
        Vector2 posicaofinal;
        private int deltaScrollWheelValue = 0;
        private int currentScrollWheelValue = 0;
        private int temporarizador;
        private int temporarizadorH;
        private int temporarizadorSomIdle;
        private int contadortracer;
        EngineStatus status;
        TankMode mode;
        MouseState mstate = Mouse.GetState();
        private bool mgLoop;
        private int nrowsAni;
        private int ncolsAni;
        private float softZoom;
        private int activeZoom;
        private Vector2 impacto;
        private Vector2 TigerSmokePos;
        private bool mainCannonAP;

        public float patrolRadius = 5f; // começa a andar
        public float attackRadius = 2f; // começa a disparar

        private Sprite Commander;

        private Sprite target;

        private Vector2 tankDirection;
        public Tank(ContentManager content, string imagem, string tank)
            : base(content, imagem)
        {
            #region If Tiger/Sherman = Contruct
            if (tank == "tiger")
            {
                this.turret = new Animated_Sprite(cManager, "tank/TigerTurretFireOpenFix", 2, 8, 1f / 12f, false);
                this.Scl(1f);
                this.turret.Scl(0.55f);
                turret.origem = new Vector2(56f, 197f);
                this.gunsight = new Sprite(content, "tank/Tiger GunSight-01");
                this.gunsight.SetRotation((float)Math.PI / 4);
                municaoAP = 40;
                municaoHE = 20;
                municaoCoax = 800;
                nrowsAni = 2;
                ncolsAni = 8;
                velocidadeMax = 0.03f;
                reverseMax = -0.02f;
            }
            else if (tank == "sherman")
            {
                this.turret = new Animated_Sprite(cManager, "tank/ShermanTurretBig", 2, 7, 1f / 12f, false);
                this.Scl(1f);
                this.turret.Scl(0.5f);
                turret.origem =new Vector2(57f,134f);
                this.gunsight = new Sprite(content, "tank/Tiger GunSight-01");
                this.gunsight.SetRotation((float)Math.PI / 4);
                municaoAP = 33;
                municaoHE = 33;
                municaoCoax = 800;
                nrowsAni = 2;
                ncolsAni = 7;
                velocidadeMax = 0.04f;
                reverseMax = -0.02f;
            }
            #endregion
            contadortracer = 0;
            temporarizador = 0;
            temporarizadorH = 0;
            status = EngineStatus.off;
            this.EnableCollisions();
            mgLoop = false;
            mainCannonAP = true;
            this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                 (float)Math.Cos(tankRot));
            distancia = this.origem.Y - tankEixo;
            this.hatchesOpen = 0;
            this.clicked = false;

            temporarizadorSomIdle = -1;
            this.turret.SetRotation((float)Math.PI / 4);
            velocidade = 0f;
            velocidadePositiva = 0;
            turnValue = 0f;
            range = 2;

            ai = true;

            softZoom = 7 ;
            activeZoom = 0;

            impacto = Vector2.Zero;

            Commander = new Sprite(cManager, "infantry/CommanderHatch");
            Commander.position = new Vector2(56f, 197f);
            Commander.Scl(0.2f);
            //Commander.position = turret.centroRot;
            mode = TankMode.Player;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);//porque este base.draw
            turret.Draw(gameTime);
            gunsight.Draw(gameTime);
        }

        public override void SetScene(Scene s)
        {
            this.scene = s;
            turret.SetScene(s);
            gunsight.SetScene(s);
        }


        public override void Update(GameTime gameTime)
        {

            #region NPC
            if (mode == TankMode.Patrolling || mode == TankMode.Walking || mode == TankMode.Attacking)
            {

                fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 direction = target.position - this.position;

                float sqDistance = direction.LengthSquared();
                if (this.mode == TankMode.Patrolling && (target.position - this.position).LengthSquared() < patrolRadius * patrolRadius)
                {
                    this.mode = TankMode.Walking;
                }

                else if (this.mode == TankMode.Walking)
                {
                    if (sqDistance < attackRadius * attackRadius)
                        mode = TankMode.Attacking;
                    else if (sqDistance > patrolRadius * patrolRadius)
                        mode = TankMode.Patrolling;
                    else
                    {
                        //this.switchstyle = 1;
                        direction.Normalize();
                        this.LookAt(direction);
                        Vector2 oldPosition = this.position;
                        this.SetPosition(this.position + direction * (float)gameTime.ElapsedGameTime.TotalSeconds * this.velocidade);
                        //if (Collides())
                        //this.position = oldPosition;

                    }
                }

                else if (this.mode == TankMode.Attacking)
                {
                    if (sqDistance > patrolRadius * patrolRadius) mode = TankMode.Patrolling;
                    else if (sqDistance > attackRadius * attackRadius) mode = TankMode.Walking;
                    else
                    {
                        //this.switchstyle = 3;
                        if (fireCounter > fireInterval)
                        {
                            fireCounter = 0f;
                            Bullet bullet = new Bullet(cManager, this.position, (float)Math.Atan2(direction.X, direction.Y), range, true);
                            direction.Normalize();
                            this.LookAt(direction);
                            bullet.LookAt(direction);
                            scene.AddSprite(bullet);
                        }

                    }
                }

            }
            #endregion

            if (mode == TankMode.Player)
            {
            mstate = Mouse.GetState();

            KeyboardState state = Keyboard.GetState();

            deltaScrollWheelValue = mstate.ScrollWheelValue - currentScrollWheelValue;
            currentScrollWheelValue += deltaScrollWheelValue;

            Point mpos = mstate.Position;



            Vector2 tpos = Camera.WorldPoint2Pixels(position);
            float a = (float)mpos.Y - tpos.Y; // busca a posição Y(coordenadas da camara Pixeis) do rato e subtrai a posição do tank(camara centro)
            float l = (float)mpos.X - tpos.X;


            float rot = (float)Math.Atan2(a, l);
            rot += (float)Math.PI / 2f;

           

            turret.SetRotation(rot);
            gunsight.SetRotation(rot);
            Commander.SetRotation(rot);

            Sprite other;
            Vector2 colPosition;

                if (deltaScrollWheelValue > 0)
                {
                    if (range < 4.8f)
                    {
                        range += 0.2f;
                    }
                    else { 
                    range = 4.8f;
                    }
                }
                else if (deltaScrollWheelValue < 0)
                {
                    if (range > 0.5f)
                    {
                        range -= 0.2f;
                    }
                    else
                    {
                        range = 0.5f; 
                    }
                }

                if (scene.collides(this, out other, out colPosition))
                { //this. sprite  out sprite colidida colPosition
                    this.velocidade -= 0.01f;
                   // Console.WriteLine(other.name);
                }
                gunsight.SetPosition(this.turret.position + new Vector2((float)Math.Sin(rot) * (range + 120f / Camera.ratio),
        (float)Math.Cos(rot) * (range + 120f / Camera.ratio)));
            
            fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireCounter >= fireInterval &&
                mstate.LeftButton == ButtonState.Pressed)
            {
                int r = (int)(Game1.RandomNumber(1f, 4f));
                pos = this.turret.position + new Vector2((float)Math.Cos(rot - Math.PI / 2) * 150 / Camera.ratio,
                    (float)Math.Sin(rot - Math.PI / 2) * -150 / Camera.ratio);
                TigerSmokePos = this.turret.position + new Vector2((float)Math.Cos(rot - Math.PI / 2) * 220 / Camera.ratio,
    (float)Math.Sin(rot - Math.PI / 2) * -220 / Camera.ratio);

                Bullet bullet = new Bullet(cManager, pos, rot,range,true);
                scene.AddSprite(bullet);
                this.turret.switchstyle = 2;
                this.turret.origemf = 1;
                this.turret.destinof = ncolsAni;
                fireCounter = 0f;

                Animated_Sprite TigerFireSmoke;
                TigerFireSmoke = new Animated_Sprite(cManager, "TigerFireSheetTEST", 1, 8, 1f / 6.2f, true);
                TigerFireSmoke.Scl(1.2f);
                TigerFireSmoke.position = TigerSmokePos;
                TigerFireSmoke.SetRotation(rot);
                TigerFireSmoke.switchstyle = 0;
                scene.AddSprite(TigerFireSmoke);
                municaoAP -= 1;
                UI.apValue = municaoAP.ToString();
                Som.fire88(r, 0.4f);


            }
            mgfireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
             if ((mgfireCounter >= mgfireInterval &&
                 state.IsKeyDown(Keys.Space)) && municaoCoax>0)
             {
                 mgLoop = true;
                 Vector2 mgpos = this.turret.position;

                 float x1 = (float)Math.Cos(Math.PI / 2 - rot) * 65f / Camera.ratio;
                 float y1 = (float)Math.Sin(Math.PI / 2 - rot) * 65f / Camera.ratio;
                 float x2 = (float)Math.Sin(Math.PI / 2 - rot) * 15f / Camera.ratio;
                 float y2 = (float)Math.Cos(Math.PI / 2 - rot) * -15f / Camera.ratio;

                 //float x1 = (float)Math.Cos(Math.PI / 2 - rot)* 65f/ Camera.ratio;
                 //float y1 = (float)Math.Cos(Math.PI / 2 - rot) * 65f / Camera.ratio;
                 //float x2 = (float)Math.Sin(Math.PI / 2 - rot)* 15f / Camera.ratio;
                 //float y2 = (float)Math.Sin(Math.PI / 2 - rot) * .15f / Camera.ratio;

                 mgpos.X =(mgpos.X + x1+x2 );
                 mgpos.Y =(mgpos.Y + y1+y2 );
                 float mgrange = range + 0.6f;

                 pos = mgpos + new Vector2((float)Math.Cos(rot - Math.PI / 2) * 150 / Camera.ratio,
    (float)Math.Sin(rot - Math.PI / 2) * -150 / Camera.ratio);

                 Bullet mgbullet = new Bullet(cManager, mgpos, rot,mgrange, contadortracer%7==0);
                 mgbullet.velocity = 20;
                 contadortracer++;
                 scene.AddSprite(mgbullet);
                 mgfireCounter = 0f;

                 Som.firelmg(1, 0.3f,true);

                 Animated_Sprite fireSmoke;
                 fireSmoke = new Animated_Sprite(cManager, "infantry/MP40SmokeS3", 1, 5, 1f / 7f, true);
                 fireSmoke.Scl(0.6f);
                 fireSmoke.position = mgpos;
                 fireSmoke.SetRotation(rot);
                 fireSmoke.switchstyle = 0;
                 scene.AddSprite(fireSmoke);
                 municaoCoax = municaoCoax -1;
                 UI.ammoValue = municaoCoax.ToString();

             }
             if (state.IsKeyUp(Keys.Space) && mgLoop == true)
             {
                 mgLoop = false;
                 Som.firelmg(1, 0.3f,mgLoop);
             }

             #region temporarizador de Hatch
             if (temporarizadorH < 0)
             {
                 temporarizadorH = 0;
             }
             else if (temporarizadorH > 0)
             {
                 temporarizadorH -= 1;
             }
             #endregion



             if (state.IsKeyDown(Keys.O) && temporarizadorH == 0)
             {
                 temporarizadorH += 120;

                 if (hatchesOpen == 0)
                    {
                        this.turret.row = 1;
                        this.turret.changeFrameStart();
                        this.hatchesOpen = 1;
                        Som.tigerCommanderOpen.Play(0.7f, 0, 0);
                        activeZoom = 2;
                        clicked = true;


                    }
                    else if (hatchesOpen == 1)
                    {
                        clicked = true;
                        this.turret.row = 0;
                        this.turret.changeFrameStart();
                        this.hatchesOpen = 0;
                        activeZoom = 1;
                        Som.tigerCommanderClose.Play(0.7f,0,0);
                    }


             }

             #region Zoom by openning Hatch
             if (activeZoom == 2 && softZoom < 9)
             {
                 if (activeZoom == 2 && softZoom >= 9)
                     {
                         Camera.SetWorldWidth(9);
                         activeZoom = 0;
                     }else{
                 softZoom += 0.04f;
                 Camera.SetWorldWidth(softZoom);
                 }
             }
             else if (activeZoom == 1 && softZoom > 7)
             {

                 if (activeZoom == 1 && softZoom <= 7)
                 {
                     Camera.SetWorldWidth(7);
                     activeZoom = 0;
                 }
                 else {
                 softZoom -= 0.05f;
                 Camera.SetWorldWidth(softZoom);
                 }
             }
             #endregion



             if (state.IsKeyDown(Keys.E) && status == EngineStatus.on && temporarizador == 0f) // Shut down Engine
             {
                     status = EngineStatus.off;
                     temporarizador += 30;
                     temporarizadorSomIdle = 80;
                     Som.tigerMovement(2, 0.5f,false);

 

             }
            else if (state.IsKeyDown(Keys.E) && status == EngineStatus.off && temporarizador == 0f) // turn on engine
             {
                 status = EngineStatus.on;
                 temporarizador += 30;
                 temporarizadorSomIdle = 180;
                 Som.tigerMovement(1, 0.5f,false);

             }

            if (temporarizadorSomIdle == 0 && (status == EngineStatus.on || status == EngineStatus.off)) {
                if(status == EngineStatus.on)
                                 Som.tigerMovement(3, 0.3f, true);
                else if (status == EngineStatus.off)
                                 Som.tigerMovement(3, 0.2f, false);
                temporarizadorSomIdle = -1;
            }
            else if (temporarizadorSomIdle > 0)
            {
                temporarizadorSomIdle -= 1;
            }

            if (temporarizador >= 1)
            {
                temporarizador -= 1;
            }
            else if (temporarizador < 0)
            {
                temporarizador = 0;
            }

            if (state.IsKeyDown(Keys.W) && status == EngineStatus.on)
                {
                    
                    if (velocidade < 0) // se estiver andar para tras trava o tank
                    {
                        this.velocidade += 0.001f;

                    }
                    else if (velocidade >= 0) // caso a velocidade seja 0 ou maior que zero está parado ou andar
                    {
                        velocidadePositiva = 1; // declara que tem uma velocidade positiva
                        velocidade += 0.0002f; // aumenta com menor velocidade
                        if (velocidade >= velocidadeMax) // se a velocidade for maior ou igual a velocidade do tank
                        {
                            velocidade = velocidadeMax; // então põe a velocidade ao maximo permitido
                        }
                    }

                }
            else if (state.IsKeyDown(Keys.S) && status == EngineStatus.on)
                {
                    if (velocidade > 0) // Travões caso a velocidade ainda esteja superior a 0
                    {
                        velocidade += -0.001f; // Travões reduz a velocidade mais rápido que o atrito
                    }
                    else if (velocidade <= 0) // caso a velocidade seja 0 ou menor que zero/ esteja parado ou em marcha atras
                    {
                        velocidadePositiva = -1; // então o tank tem velocidade negativa
                        velocidade += -0.0001f; //acrescenta valor negativo, aumentando a velocidade (marcha atrás)
                        if (velocidade <= reverseMax) // igual a velocidade negativa a maxima velocidade permitida
                        {
                            velocidade = reverseMax;
                        }
                    }
                }
           

                if (((state.IsKeyUp(Keys.W)) && (state.IsKeyUp(Keys.S))) || status == EngineStatus.off) // nao input de velocidade pelo jogador, atrito do terreno
                {
                    if (velocidadePositiva == 1) //Atrito Reduz velocidade positiva
                    { //se a 
                        velocidadePositiva = 1;
                        velocidade += -0.0003f;
                        if (velocidade < 0)
                        {
                            velocidade = 0;
                            velocidadePositiva = 0;
                        }
                    }
                    else if (velocidadePositiva == -1) //caso esteja a andar para trás
                    {
                        velocidadePositiva = -1;
                        velocidade += 0.0003f; //atrito
                        if (velocidade > 0) // se neste instante a velocidade ultrapassar 0 então ele para
                        {
                            velocidade = 0;
                            velocidadePositiva = 0;
                        }
                    }

                }
                if (state.IsKeyDown(Keys.A) && velocidade != 0)
                {

                    tankRot += -0.01f;//valor de incremento
                    turnValue += -0.0001f;
                    this.SetRotation(tankRot);
                    this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                     (float)Math.Cos(tankRot));
                }
                if (state.IsKeyDown(Keys.D) && velocidade != 0)
                {
                    tankRot += 0.01f;//valor de incremento
                    turnValue += 0.0001f;
                    this.SetRotation(tankRot);
                    //turret.position 
                    this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                     (float)Math.Cos(tankRot));

                }

                this.position = this.position + tankDirection * velocidade;
                
                posicaofinal = new Vector2(-(float)Math.Cos(tankRot - Math.PI / 2) * distancia / Camera.ratio, (float)Math.Sin(tankRot - Math.PI / 2) * distancia / Camera.ratio);


                turret.position = this.position - posicaofinal;

                this.Commander.position = this.turret.position;
                float cx1 = (float)Math.Cos(Math.PI / 2 - rot) * -10f / Camera.ratio;
                float cy1 = (float)Math.Sin(Math.PI / 2 - rot) * -10f / Camera.ratio;
                float cx2 = (float)Math.Sin(Math.PI / 2 - rot) * -15f / Camera.ratio;
                float cy2 = (float)Math.Cos(Math.PI / 2 - rot) * 15f / Camera.ratio;

                this.Commander.position.X = (Commander.position.X + cx1 + cx2);
                this.Commander.position.Y = (Commander.position.Y + cy1 + cy2);

                if (hatchesOpen == 1 && clicked == true)
                {
                    clicked = false;
                    scene.AddSprite(Commander);
                }
                else if (hatchesOpen == 0 && clicked == true)
                {
                    clicked = false;
                    this.Commander.Destroy();
                    hatchesOpen = 0;

                }
                turret.Update(gameTime);
                gunsight.Update(gameTime);
                Camera.SetTarget(this.position);

                }

            //Scene.DrawString(myUiFont, municaoCoax, new Vector2((largura / 2) + 130, altura - 90), Color.Red);

                Commander.Update(gameTime);
                base.Update(gameTime);
            
        }
    }
}
