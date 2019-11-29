using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class Tower
    {
        const int Max_Bullets = 100, Shot_Time = 20, Reload_Time = 300, Max_Shots = 20, Max_HP = 900, Min_Angle = -5, Max_Angle = 6;
        const int Cannon_Length = 90, Max_Bullet_Velocity = 30, Min_Bullet_Velocity = 25, Max_Time_To_Hurt = 10;
        int X, Y, Time_To_Shot, Shots, HP, Time_To_Hurt;
        double rotation;
        bool alive, noise, hurt;
        Bullet[] bulls;
        GreedHelp greed;
        public Tower(int new_X, int new_Y, GreedHelp new_Greed)
        {
            hurt = false;
            noise = false;
            greed = new_Greed;
            rotation = 0;
            Time_To_Shot = Reload_Time;
            Shots = Max_Shots;
            X = new_X;
            Y = new_Y;
            alive = true;
            HP = Max_HP;
            bulls = new Bullet[Max_Bullets];
            for (int i = 0; i < Max_Bullets; i++)
                bulls[i] = new Bullet();
        }

        public void Update_Map(GreedHelp new_greed)
        {
            greed = new_greed;
        }

        public void Tick(int T_X, int T_Y)
        {
            if (hurt)
            {
                Time_To_Hurt--;
                if (Time_To_Hurt <= 0) hurt = false;
            }
            if ((T_X != X && T_Y != Y) && greed.IsTowerClear(X, Y, T_X, T_Y))
            {
                double x1 = X, y1 = Y, x2 = T_X, y2 = T_Y;
                if (x1 - x2 != 0)
                {
                    if (x2 > x1) rotation = (float)(Math.Atan((y1 - y2) / (x1 - x2)) + MathHelper.PiOver2);
                    else rotation = (float)(Math.Atan((y1 - y2) / (x1 - x2)) - MathHelper.PiOver2);
                }
                else
                {
                    if (y2 < y1) rotation = 0;
                    else rotation = MathHelper.Pi;
                }
            }

            if (Time_To_Shot <= 0)
            {
                if ((T_X != X && T_Y != Y) && greed.IsTowerClear(X, Y, T_X, T_Y))
                {
                    bool shoot = false;
                    for (int i = 0; i < Max_Bullets; i++)
                    {
                        if (!bulls[i].Is_Alive)
                        {
                            Random o = new Random();
                            double r = rotation + (o.Next(Min_Angle, Max_Angle) / (double)100) - MathHelper.PiOver2;
                            double x = X + (Cannon_Length * Math.Cos(r));
                            double y = Y + (Cannon_Length * Math.Sin(r));
                            bulls[i].Shot(x, y, o.Next(Min_Bullet_Velocity, Max_Bullet_Velocity + 1), r, 0, 0, 3);
                            shoot = true;
                            break;
                        }
                    }
                    if (shoot)
                    {
                        Shots--;
                        noise = true;
                        if (Shots <= 0)
                        {
                            Shots = Max_Shots;
                            Time_To_Shot = Reload_Time;
                        }
                        else
                            Time_To_Shot = Shot_Time;
                    }
                }
            }
            if (Time_To_Shot > 0) Time_To_Shot--;
        }

        public void Hurt (int Damage)
        {
            HP -= Damage;
            if (HP <= 0) alive = false;
            if (alive)
            {
                Time_To_Hurt = Max_Time_To_Hurt;
                hurt = true;
            }
        }

        public Bullet[] Bullets
        {
            get { return bulls; }
        }

        public bool Is_Alive
        {
            get { return alive; }
        }

        public bool Hurted
        {
            get { return hurt; }
        }

        public bool Shot_Noise
        {
            get { return noise; }
            set { noise = value; }
        }

        public int Bullets_Count
        {
            get { return Max_Bullets; }
        }

        public int pos_X
        {
            get { return X; }
        }

        public int pos_Y
        {
            get { return Y; }
        }

        public float Rotate
        {
            get { return (float)rotation; }
        }
    }
}
