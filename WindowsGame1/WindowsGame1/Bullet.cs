using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{

    class Bullet
    {
        const int Bullet_Live_Time = 200;
        double X, Y, vx, vy, time, rotate;
        bool alive = false, inside;
        int type, damage;

        public void Shot(double pos_X, double pos_Y, double vel, double Rotation, double Hero_VX, double Hero_VY, int Type)
        {
            type = Type;
            inside = true;
            X = pos_X; Y = pos_Y;
            rotate = Rotation;
            vx = Math.Cos(rotate) * vel; vx += Hero_VX;
            vy = Math.Sin(rotate) * vel; vy += Hero_VY;
            alive = true; time = 0;
            Random o = new Random();
            switch (type)
            {
                case 0: damage = o.Next(4, 7); break;
                case 1: damage = o.Next(8, 12); break;
                case 2: damage = o.Next(70, 111); break;
                case 3: damage = o.Next(12, 20); break;
            }
        }

        public Bullet()
        {
            X = 0; Y = 0;
            vx = 0; vy = 0;
            alive = false;
            time = 0;
            rotate = 0;
        }

        public void AddTime()
        {
            time += 1;
            if (time > Bullet_Live_Time) this.Destroy();
            else
            {
                X += vx;
                Y += vy;
            }
        }

        public void Destroy()
        {
            alive = false;
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
        public int Damage
        {
            set { }
            get { return damage; }
        }

        public int Type
        {
            set { }
            get { return type; }
        }

        public float Rotation
        {
            set { }
            get { return (float)rotate; }
        }

        public bool Is_Alive
        {
            set
            {
            }
            get
            {
                return alive;
            }
        }

        public bool Is_Inside
        {
            set
            {
                inside = value;
            }
            get
            {
                return inside;
            }
        }
    }
}
