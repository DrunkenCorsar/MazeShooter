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
    class Zombie
    {
        const int Wall_Width = 133, Min_Width = 0, Min_Height = 0;
        const int Max_Time_Lie = 500, Hurtet_Time = 10, Time_To_Find = 150, Max_Time = 300, Max_Time_ToBox = 300, Max_time_hitting = 50;
        int Max_Width, Max_Height, timeAll, time_check, damage, time_toBox, time_hitting;
        double X, Y, vel, tar_X, tar_Y, hp, rotation, time_lie, time_Hurt;
        bool alive, body, hurt, is_walking, is_stay, is_attak, check_enemy, find, Boxed, hitting;
        public Zombie()
        {
            alive = false;
        }

        public void Born(int pos_X, int pos_Y, double Vel, int Tar_X, int Tar_Y, int HP, int max_width, int max_height)
        {
            Boxed = true;
            hitting = false;
            Random o = new Random();
            damage = o.Next(1, 15);
            Max_Height = max_height;
            Max_Width = max_width;
            X = pos_X; Y = pos_Y; vel = Vel; hp = HP;
            tar_X = Tar_X; tar_Y = Tar_Y;
            alive = true; body = false; hurt = false;
            double r;
            if (X - tar_X != 0)
            {
                if (tar_X > X) r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)));
                else r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)) - MathHelper.Pi);
            }
            else
            {
                if (tar_Y < Y) r = MathHelper.Pi;
                else r = 0;
            }
            time_lie = Max_Time_Lie;
            rotation = r;
            is_stay = true;
            is_walking = false;
            is_attak = false;
        }

        public void Attak(int Tar_X, int Tar_Y)
        {
            if (hitting)
            {
                time_hitting--;
                if (time_hitting <= 0) hitting = false;
            }
            if (!Boxed)
            {
                time_toBox--;
                if (time_toBox <= 0) Boxed = true;
            }
            if (!is_attak) is_attak = true;
            tar_X = Tar_X; tar_Y = Tar_Y;
            double S = Math.Sqrt(((tar_X - X) * (tar_X - X)) + ((Tar_Y - Y) * (Tar_Y - Y)));
            if (hurt)
            {
                X += vel * ((tar_X - X) / S) / 2;
                Y += vel * ((tar_Y - Y) / S) / 2;
            }
            else
            {
                X += vel * ((tar_X - X) / S);
                Y += vel * ((tar_Y - Y) / S);
            }
            if (X < Min_Width + Wall_Width) { X = Min_Width + Wall_Width; }
            if (X > Max_Width - Wall_Width) { X = Max_Width - Wall_Width; }
            if (Y < Min_Height + Wall_Width) { Y = Min_Height + Wall_Width; }
            if (Y > Max_Height - Wall_Width) { Y = Max_Height - Wall_Width; }
            double r;
            if (X - tar_X != 0)
            {
                if (tar_X > X) r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)));
                else r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)) - MathHelper.Pi);
            }
            else
            {
                if (tar_Y < Y) r = MathHelper.Pi;
                else r = 0;
            }
            rotation = r;
            if (hurt)
            {
                time_Hurt--;
                if (time_Hurt <= 0)
                {
                    hurt = false;
                    time_Hurt = 0;
                }
            }
            if (is_walking)
            {
                is_walking = false; //is_stay = true;
            }
            if (S < 10)
            {
                is_attak = false;
                is_stay = true;
            }
        }

        public void BoxBox()
        {
            Boxed = false;
            hitting = true;
            time_hitting = Max_time_hitting;
            time_toBox = Max_Time_ToBox;
        }

        public void Attak()
        {
            if (hitting)
            {
                time_hitting--;
                if (time_hitting <= 0) hitting = false;
            }
            if (!Boxed)
            {
                time_toBox--;
                if (time_toBox <= 0) Boxed = true;
            }
            if (!is_attak) is_attak = true;
            double S = Math.Sqrt(((tar_X - X) * (tar_X - X)) + ((tar_X - Y) * (tar_Y - Y)));
            if (hurt)
            {
                X += vel * ((tar_X - X) / S) / 2;
                Y += vel * ((tar_Y - Y) / S) / 2;
            }
            else
            {
                X += vel * ((tar_X - X) / S);
                Y += vel * ((tar_Y - Y) / S);
            }
            if (X < Min_Width + Wall_Width) { X = Min_Width + Wall_Width; }
            if (X > Max_Width - Wall_Width) { X = Max_Width - Wall_Width; }
            if (Y < Min_Height + Wall_Width) { Y = Min_Height + Wall_Width; }
            if (Y > Max_Height - Wall_Width) { Y = Max_Height - Wall_Width; }
            double r;
            if (X - tar_X != 0)
            {
                if (tar_X > X) r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)));
                else r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)) - MathHelper.Pi);
            }
            else
            {
                if (tar_Y < Y) r = MathHelper.Pi;
                else r = 0;
            }
            rotation = r;
            if (hurt)
            {
                time_Hurt--;
                if (time_Hurt <= 0)
                {
                    hurt = false;
                    time_Hurt = 0;
                }
            }
            if (is_walking)
            {
                is_walking = false; //is_stay = true;
            }
            if (S < 10)
            {
                is_attak = false;
                is_stay = true;
            }
        }

        public void Stay()
        {
            if (hitting)
            {
                time_hitting--;
                if (time_hitting <= 0) hitting = false;
            }
            if (!Boxed)
            {
                time_toBox--;
                if (time_toBox <= 0) Boxed = true;
            }
            if (is_attak) is_attak = false;
            if (X < Min_Width + Wall_Width) { X = Min_Width + Wall_Width; }
            if (X > Max_Width - Wall_Width) { X = Max_Width - Wall_Width; }
            if (Y < Min_Height + Wall_Width) { Y = Min_Height + Wall_Width; }
            if (Y > Max_Height - Wall_Width) { Y = Max_Height - Wall_Width; }
            if (hurt)
            {
                time_Hurt--;
                if (time_Hurt <= 0)
                {
                    hurt = false;
                    time_Hurt = 0;
                }
            }
            timeAll++;
            if (timeAll > Max_Time)
            {
                timeAll = 0;
                check_enemy = true;
                is_stay = false;
                find = true;
                is_walking = true;
            }
        }

        public void Walk(int Tar_X, int Tar_Y)
        {
            if (hitting)
            {
                time_hitting--;
                if (time_hitting <= 0) hitting = false;
            }
            if (!Boxed)
            {
                time_toBox--;
                if (time_toBox <= 0) Boxed = true;
            }
            tar_X = Tar_X; tar_Y = Tar_Y;
            double S = Math.Sqrt(((tar_X - X) * (tar_X - X)) + ((Tar_Y - Y) * (Tar_Y - Y)));
            if (hurt)
            {
                X += vel * ((tar_X - X) / S) / 2;
                Y += vel * ((tar_Y - Y) / S) / 2;
            }
            else
            {
                X += vel * ((tar_X - X) / S);
                Y += vel * ((tar_Y - Y) / S);
            }
            if (X < Min_Width + Wall_Width) { X = Min_Width + Wall_Width; }
            if (X > Max_Width - Wall_Width) { X = Max_Width - Wall_Width; }
            if (Y < Min_Height + Wall_Width) { Y = Min_Height + Wall_Width; }
            if (Y > Max_Height - Wall_Width) { Y = Max_Height - Wall_Width; }
            double r;
            if (X - tar_X != 0)
            {
                if (tar_X > X) r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)));
                else r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)) - MathHelper.Pi);
            }
            else
            {
                if (tar_Y < Y) r = MathHelper.Pi;
                else r = 0;
            }
            rotation = r;
            if (hurt)
            {
                time_Hurt--;
                if (time_Hurt <= 0)
                {
                    hurt = false;
                    time_Hurt = 0;
                }
            }
            if (S < 10)
            {
                is_stay = true;
                is_walking = false;
            }
        }

        public void Walk()
        {
            if (hitting)
            {
                time_hitting--;
                if (time_hitting <= 0) hitting = false;
            }
            if (!Boxed)
            {
                time_toBox--;
                if (time_toBox <= 0) Boxed = true;
            }
            double S = Math.Sqrt(((tar_X - X) * (tar_X - X)) + ((tar_Y - Y) * (tar_Y - Y)));
            if (hurt)
            {
                X += vel * ((tar_X - X) / S) / 2;
                Y += vel * ((tar_Y - Y) / S) / 2;
            }
            else
            {
                X += vel * ((tar_X - X) / S);
                Y += vel * ((tar_Y - Y) / S);
            }
            if (X < Min_Width + Wall_Width) { X = Min_Width + Wall_Width; }
            if (X > Max_Width - Wall_Width) { X = Max_Width - Wall_Width; }
            if (Y < Min_Height + Wall_Width) { Y = Min_Height + Wall_Width; }
            if (Y > Max_Height - Wall_Width) { Y = Max_Height - Wall_Width; }
            double r;
            if (X - tar_X != 0)
            {
                if (tar_X > X) r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)));
                else r = (float)(Math.Atan((Y - tar_Y) / (X - tar_X)) - MathHelper.Pi);
            }
            else
            {
                if (tar_Y < Y) r = MathHelper.Pi;
                else r = 0;
            }
            rotation = r;
            if (hurt)
            {
                time_Hurt--;
                if (time_Hurt <= 0)
                {
                    hurt = false;
                    time_Hurt = 0;
                }
            }
            if (S < 10)
            {
                is_stay = true;
                is_walking = false;
            }
        }

        public void Lie()
        {
            time_lie--;
            if (time_lie <= 0) body = false;
        }

        public bool Check
        {
            set { check_enemy = value; }
            get { return check_enemy; }
        }
        public void Hurt(int Damage)
        {
            hp -= Damage;
            if (hp < 0)
            {
                alive = false;
                body = true;
            }
            else
            {
                hurt = true; time_Hurt = Hurtet_Time;
            }
        }

        public void Place(double new_X, double new_Y)
        {
            X = new_X;
            Y = new_Y;
        }
        public bool Alive
        {
            set { alive = value; }
            get { return alive; }
        }

        public bool Hitting
        {
            set { hitting = value; }
            get { return hitting; }
        }

        public bool Body
        {
            set { body = value; }
            get { return body; }
        }
        public bool Boxing
        {
            set {  }
            get { return Boxed; }
        }

        public bool staying
        {
            set { is_stay = value; }
            get { return is_stay; }
        }

        public bool attaking
        {
            set { is_attak = value; }
            get { return is_attak; }
        }
        public bool Find
        {
            set { find = value; }
            get { return find; }
        }
        public bool walking
        {
            set { is_walking = value; }
            get { return is_walking; }
        }


        public bool Hurtet
        {
            set { }
            get { return hurt; }
        }

        public int pos_X
        {
            set { X = value; }
            get { return Convert.ToInt32(Math.Round(X)); }
        }

        public int Dmg
        {
            set { }
            get { return damage; }
        }

        public int pos_Y
        {
            set { Y = value; }
            get { return Convert.ToInt32(Math.Round(Y)); }
        }

        public int TAR_X
        {
            set { tar_X = value; }
            get { return Convert.ToInt32(Math.Round(tar_X)); }
        }

        public int TAR_Y
        {
            set { tar_Y = value; }
            get { return Convert.ToInt32(Math.Round(tar_Y)); }
        }
        public double doub_X
        {
            set {  }
            get { return X; }
        }

        public double doub_Y
        {
            set { }
            get { return Y; }
        }

        public float Rotate
        {
            set { }
            get { return (float)rotation; }
        }
    }
}
