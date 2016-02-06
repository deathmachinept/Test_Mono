using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Afrika_Korps
{
    public static class Som
    {

        public static SoundEffect mp40fire;
        public static SoundEffect mp40fire1;
        public static SoundEffect mp40fire2;
        public static SoundEffect s88fire;
        public static SoundEffect s88fire1;
        public static SoundEffect s88fire2;

        public static SoundEffect smgdryfire;
        public static SoundEffect smgreload;
        public static SoundEffect sonseffects;

        public static SoundEffect mg34fire;
        public static SoundEffect mg34fire1;
        public static SoundEffect mg34reload;
        public static SoundEffect mgdryfire;

        public static SoundEffect tigerEngineStart;
        public static SoundEffect tigerEngineStop;
        public static SoundEffect tigerEngineLoop;
        public static SoundEffect tigerCommanderOpen;
        public static SoundEffect tigerCommanderClose;
        public static SoundEffectInstance EngineLoop;
        public static SoundEffectInstance mg34Loop;
        private static ContentManager content;
        public static Hashtable sons = new Hashtable(); 


        public static void LoadContent(ContentManager contents)
        {

            content = contents;

            mp40fire = content.Load<SoundEffect>("sound/Infantry/Weapons/mp40_fire021");
            mp40fire1 = content.Load<SoundEffect>("sound/Infantry/Weapons/mp40_fire021");
            mp40fire2 = content.Load<SoundEffect>("sound/Infantry/Weapons/mp40_fire031");
            smgdryfire = content.Load<SoundEffect>("sound/Infantry/Weapons/dryfire_smg");
            smgreload = content.Load<SoundEffect>("sound/Infantry/Weapons/adpcmMp40ReloadRealMultiMono");

            mg34fire = content.Load<SoundEffect>("sound/Infantry/Weapons/mg34_p_fire_end011");
            mg34fire1 = content.Load<SoundEffect>("sound/Infantry/Weapons/mg34_p_fire_loop011");
            mg34reload = content.Load<SoundEffect>("sound/Infantry/Weapons/MG34_ReloadHidden");
            mgdryfire = content.Load<SoundEffect>("sound/Infantry/Weapons/dryfire_smg");
            mg34Loop = mg34fire1.CreateInstance();

            s88fire = content.Load<SoundEffect>("sound/Tank/Gun/88mm_fire011");
            s88fire1 = content.Load<SoundEffect>("sound/Tank/Gun/88mm_fire021");
            s88fire2 = content.Load<SoundEffect>("sound/Tank/Gun/88mm_fire031");

            tigerEngineStart = content.Load<SoundEffect>("sound/Tank/Movement/tiger_engine_start");
            tigerEngineStop = content.Load<SoundEffect>("sound/Tank/Movement/tiger_engine_stop");
            tigerEngineLoop = content.Load<SoundEffect>("sound/Tank/Movement/tiger_engine_loop01");
            tigerCommanderOpen = content.Load<SoundEffect>("sound/Tank/Movement/tiger_commanderhatch_open");
            tigerCommanderClose = content.Load<SoundEffect>("sound/Tank/Movement/tiger_commanderhatch_close");
            EngineLoop = tigerEngineLoop.CreateInstance();


        }


        public static void firesmg(int random, float volume)
        {
            if(random == 1){
              mp40fire.Play(volume,0f,0f);
            }else if(random == 2){
              mp40fire1.Play(volume,0f,0f);
            }else if(random == 3){
                mp40fire2.Play(volume, 0f, 0f);
            }else if(random == 4){
              smgdryfire.Play(volume, 0f, 0f);
            }
            else if (random == 5)
            {
              smgreload.Play(volume, 0f, 0f);
            }

        }

        public static void firelmg(int random, float volume, bool loop)
        {
            if (random == 1)
            {
                mg34Loop.Volume = volume;
                if (loop)
                {
                    mg34Loop.IsLooped = loop;
                    mg34Loop.Play();
                }
                else if (loop == false)
                {
                    mg34Loop.Stop();
                    mg34Loop.IsLooped = loop;
                    mg34fire.Play();
                }
            }
            else if (random == 2)
            {
                mgdryfire.Play(volume, 0f, 0f);
            }
            else if (random == 3)
            {
                smgreload.Play(volume, 0f, 0f);
            }


 

        }

        public static void fire88(int random, float volume)
        {
            if (random == 1)
            {
                s88fire.Play(volume, 0f, 0f);
            }
            else if (random == 2)
            {
                s88fire.Play(volume, 0f, 0f);
            }
            else if (random == 3)
            {
                s88fire.Play(volume, 0f, 0f);
            }
        }

        public static void tigerMovement(int random, float volume, bool loop)
        {
            if (random == 1)
            {
                tigerEngineStart.Play(volume, 0f, 0f);
            }
            else if (random == 2)
            {
                tigerEngineStop.Play(volume, 0f, 0f);
            }
            else if (random == 3)
            {
                EngineLoop.Volume = volume;
                if (loop)
                {
                    EngineLoop.IsLooped = loop;
                    EngineLoop.Play();
                }
                else if (loop == false)
                {
                    EngineLoop.IsLooped = loop;
                    EngineLoop.Stop();
                }
 
            }
        }


    }
}
