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
    class Rocket
    {
        const int Max_Vel = 30;
        const double Acc = 0.2;
        int Time_To_Live = 1000, damage;
        double a_x, a_y, vel, vel_x, vel_y, rotate, X, Y;
        bool explotion, Alive, anim;
        public Rocket(double X0, double Y0, double Rotation)
        {
            X = X0; Y = Y0;
            rotate = Rotation;
            Random o = new Random();
            damage = o.Next(470, 530);
            a_x = Acc * Math.Cos(rotate);
            a_y = Acc * Math.Sin(rotate);
            Alive = true; explotion = false;
            vel_x = 0; vel_y = 0; vel = 0;
        }

        public Rocket()
        {
        }
        public void Fly()
        {
            vel_x += a_x;
            vel_y += a_y;
            double vel0 = Math.Sqrt((a_x * a_x) + (a_y * a_y));
            if (vel0 > Max_Vel)
            {
                vel_x = Math.Cos(rotate) * vel;
                vel_y = Math.Sin(rotate) * vel;
            }
            X += vel_x;
            Y += vel_y;
            Time_To_Live--;
            if (Time_To_Live <= 0) this.Destroy();
        }

        public void Destroy()
        {
            Alive = false;
        }

        public void Explose()
        {
            Alive = false;
            explotion = true;
        }

        public bool Explosion
        {
            set
            { explotion = value; }
            get
            { return explotion; }
        }

        public int pos_X
        {
            set { }
            get { return Convert.ToInt32(Math.Round(X)); }
        }

        public int pos_Y
        {
            set { }
            get { return Convert.ToInt32(Math.Round(Y)); }
        }

        public int Damage
        {
            get { return damage; }
        }
        public double dou_X
        {
            set { }
            get { return X; }
        }

        public double dou_Y
        {
            set { }
            get { return Y; }
        }
        public float Rotation
        {
            set { }
            get { return (float)rotate + MathHelper.PiOver2; }
        }
        public bool Anim
        {
            set { anim = value; }
            get { return anim; }
        }

        public bool alive
        {
            set { }
            get { return Alive; }
        }
    }
}
