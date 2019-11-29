using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class Hero
    {
        const int Wall_Width = 133, Max_Turn = 3000, Bullet_Count = 100, Min_Width = 0, Min_Height = 0, Precition = 100, Max_Time_Hurt = 20;
        int[] Reload_Turn = new int[] { 10, 30, 200, 200 };
        const int Bullets_In_0 = 30, Bullets_In_1 = 6, Bullets_In_2 = 10, Bullets_In_3 = 3, Max_Rockets = 10, Reload_Time_0 = 240, Reload_Time_1 = 60, Reload_Time_2 = 340, Reload_Time_3 = 400, Max_HP = 300, Max_Rank = 10;
        const double Accuracy_0 = 0.10, Accuracy_1 = 0.05, Accuracy_2 = 0, Accuracy_3 = 0.07, Starting_Velocity = 1, Acceleration_Step = 0.3;
        const double Bullet_Speed_0 = 20, Bullet_Speed_1 = 40, Bullet_Speed_2 = 50;
        const double Cannon_Distance_0 = 60, Cannon_Angle_0 = -1.2, Cannon_Distance_1 = 40, Cannon_Angle_1 = -MathHelper.PiOver2, Cannon_Distance_2 = 35, Cannon_Angle_2 = -MathHelper.PiOver2, Cannon_Distance_3 = 45, Cannon_Angle_3 = -1;
        int Max_Width, Max_Height, rank;
        int bullets_in_case_0, bullets_in_case_1, bullets_in_case_2, bullets_in_case_3, HP, time_hurt, Awards_Count;
        bool IsReady, empty_0, reloading_0;
        bool shot, reload, empty, Alive, rocket_fly_sound, explotion_sound, hurt;
        double X, Y, vel, c, turn_0, turn_1, turn_2, turn_3, vx, vy, buff_X, buff_Y, Max_Velocity;
        int exp_time, bull_c_0, bull_c_1, bull_c_2, bull_c_3, bull_c, Selected_Item;
        float r;
        int[] awards;
        Bullet[] bull = new Bullet[Bullet_Count];
        Rocket[] rockets = new Rocket[Max_Rockets];
        public Hero(double Position_X, double Position_Y, int Max_H, int Max_W, int Awards_Count_1)
        {
            rank = 1;
            X = Position_X;
            Y = Position_Y;
            Awards_Count = Awards_Count_1;
            awards = new int[Awards_Count];
            for (int i = 0; i < Awards_Count; i++) awards[i] = 0;
            Max_Height = Max_H;
            Max_Width = Max_W;
            HP = Max_HP;
            vel = Starting_Velocity;
            c = 0; Selected_Item = 0;
            for (int i = 0; i < Bullets_Count; i++)
            {
                bull[i] = new Bullet();
            }
            for (int i = 0; i < Max_Rockets; i++)
            {
                rockets[i] = new Rocket();
            }
            IsReady = true; Alive = true;
            bull_c_0 = Bullets_In_0;
            bull_c_1 = Bullets_In_1;
            bull_c_2 = Bullets_In_2;
            bull_c_3 = 0;
            bull_c = Bullets_In_0;
            bullets_in_case_0 = 40;
            bullets_in_case_1 = 20;
            bullets_in_case_2 = 2;
            bullets_in_case_3 = 0;
        }

        public void Hurted(int damage)
        {
            HP -= damage;
            if (HP <= 0) Alive = false;
            else
            {
                time_hurt = Max_Time_Hurt;
                hurt = true;
            }
        }

        public void AddItems(int item_0, int item_1, int item_2, int item_3, int Add_HP)
        {
            HP += Add_HP;
            if (HP > Max_HP) HP = Max_HP;
            bullets_in_case_0 += item_0;
            bullets_in_case_1 += item_1;
            bullets_in_case_2 += item_2;
            bullets_in_case_3 += item_3;
        }

        public void Move(double cam_X, double cam_Y)
        {
            double turn = 0;
            switch(Selected_Item)
            {
                case 0:
                    {
                        turn = turn_0;
                        Max_Velocity = 5;
                    } break;
                case 1:
                    {
                        turn = turn_1;
                        Max_Velocity = 6;
                    }
                    break;
                case 2:
                    {
                        turn = turn_2;
                        Max_Velocity = 3;
                    }
                    break;
                case 3:
                    {
                        turn = turn_3;
                        Max_Velocity = 1;
                    }
                    break;
            }
            buff_X = cam_X; buff_Y = cam_Y;
            KeyboardState key = Keyboard.GetState();
            double v = vel + c; turn++; vx = 0; vy = 0;
            if (key.IsKeyDown(Keys.F) && key.IsKeyDown(Keys.E) && key.IsKeyDown(Keys.LeftShift)) HP = Max_HP;
            if (key.IsKeyDown(Keys.M) && key.IsKeyDown(Keys.O) && key.IsKeyDown(Keys.LeftShift))
            {
                bullets_in_case_0 += Bullets_In_0;
                bullets_in_case_1 += Bullets_In_1;
                bullets_in_case_2 += Bullets_In_2;
                bullets_in_case_3 += Bullets_In_3;
            }
            if (key.IsKeyDown(Keys.L) && key.IsKeyDown(Keys.O) && key.IsKeyDown(Keys.LeftShift))
            {
                bull_c_0 = Bullets_In_0;
                bull_c_1 = Bullets_In_1;
                bull_c_2 = Bullets_In_2;
                bull_c_3 = Bullets_In_3;
                switch(Selected_Item)
                {
                    case 0: bull_c = Bullets_In_0; break;
                    case 1: bull_c = Bullets_In_1; break;
                    case 2: bull_c = Bullets_In_2; break;
                    case 3: bull_c = Bullets_In_3; break;
                }
            }
            if ((key.IsKeyDown(Keys.A) && (key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.S))) || (key.IsKeyDown(Keys.D) && (key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.S))))
                v = v / Math.Sqrt(2);
            if (!(key.IsKeyDown(Keys.A) || key.IsKeyDown(Keys.S) || key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.D)))
                c = 0;
            else
            {
                if (key.IsKeyDown(Keys.A)) { X -= v; vx -= v; }
                if (key.IsKeyDown(Keys.D)) { X += v; vx += v; }
                if (key.IsKeyDown(Keys.W)) { Y -= v; vy -= v; }
                if (key.IsKeyDown(Keys.S)) { Y += v; vy += v; }
                if (c < Max_Velocity) c += Acceleration_Step;
                if (c > Max_Velocity) c = Max_Velocity;
                if (hurt && c >= (Max_Velocity / 2)) c = Max_Velocity / 2;
            }
            if (X < Min_Width + Wall_Width) { X = Min_Width + Wall_Width; vx = 0; }
            if (X > Max_Width - Wall_Width) { X = Max_Width - Wall_Width; vx = 0; }
            if (Y < Min_Height + Wall_Width) { Y = Min_Height + Wall_Width; vy = 0; }
            if (Y > Max_Height - Wall_Width) { Y = Max_Height - Wall_Width; vy = 0; }
            MouseState m = Mouse.GetState();
            if (turn == Max_Turn) turn = 0;
            if (((turn % Reload_Turn[Selected_Item] == 0) || (IsReady)) && bull_c > 0 && !reloading_0)
                if (m.LeftButton == ButtonState.Pressed)
                {
                    bull_c--; shot = true;
                    if (IsReady)
                    {
                        IsReady = false;
                        turn = 0;
                    }
                    int n = -1;
                    if (Selected_Item != 3)
                    for (int i = 0; i < Bullet_Count; i++)
                    {
                        if (bull[i].Is_Alive == false) n = i;
                    }
                    else
                        for (int i = 0; i < Max_Rockets; i++)
                        {
                            if (rockets[i].alive == false) n = i;
                        }
                    if (n != -1)
                    {
                        double Accuracy = 0, Bullet_Speed = 0;
                        switch (Selected_Item)
                        {
                            case 0:
                                {
                                    Accuracy = Accuracy_0;
                                    Bullet_Speed = Bullet_Speed_0;
                                }
                                break;
                            case 1:
                                {
                                    Accuracy = Accuracy_1;
                                    Bullet_Speed = Bullet_Speed_1;
                                }
                                break;
                            case 2:
                                {
                                    Accuracy = Accuracy_2;
                                    Bullet_Speed = Bullet_Speed_2;
                                }
                                break;
                            case 3:
                                {
                                    Accuracy = Accuracy_3;
                                }
                                break;
                        }
                        Random o = new Random();
                        double r0 = -MathHelper.PiOver2 + r + (o.Next(-Convert.ToInt32(Math.Round(Accuracy * Precition)), (Convert.ToInt32(Math.Round(Accuracy * Precition)) + 1)) / (double)Precition);
                        double X0 = 0, Y0 = 0;
                        switch (Selected_Item)
                        {
                            case 0:
                                {
                                    X0 = X + (Cannon_Distance_0 * Math.Cos(r + Cannon_Angle_0));
                                    Y0 = Y + (Cannon_Distance_0 * Math.Sin(r + Cannon_Angle_0));
                                }
                                break;
                            case 1:
                                {
                                    X0 = X + (Cannon_Distance_1 * Math.Cos(r + Cannon_Angle_1));
                                    Y0 = Y + (Cannon_Distance_1 * Math.Sin(r + Cannon_Angle_1));
                                }
                                break;
                            case 2:
                                {
                                    X0 = X + (Cannon_Distance_2 * Math.Cos(r + Cannon_Angle_2));
                                    Y0 = Y + (Cannon_Distance_2 * Math.Sin(r + Cannon_Angle_2));
                                }
                                break;
                            case 3:
                                {
                                    X0 = X + (Cannon_Distance_3 * Math.Cos(r + Cannon_Angle_3));
                                    Y0 = Y + (Cannon_Distance_3 * Math.Sin(r + Cannon_Angle_3));
                                }
                                break;
                        }
                        if (Selected_Item != 3) bull[n].Shot(X0, Y0, Bullet_Speed, r0, vx, vy, Selected_Item);
                        else
                        {
                            rockets[n] = new Rocket(X0, Y0, r0);
                            rocket_fly_sound = true;
                        }
                    }
                }
                else
                    IsReady = true;
            if (m.LeftButton == ButtonState.Pressed && bull_c == 0 && !reloading_0 && !empty_0)
            { empty = true; empty_0 = true; }
            if (m.LeftButton != ButtonState.Pressed) empty_0 = false;
            int Bullets_In_Collar = 0, Reload_Time = 0;
            switch(Selected_Item)
            {
                case 0:
                    {
                        Bullets_In_Collar = Bullets_In_0;
                        Reload_Time = Reload_Time_0;
                    }
                    break;
                case 1:
                    {
                        Bullets_In_Collar = Bullets_In_1;
                        Reload_Time = Reload_Time_1;
                    }
                    break;
                case 2:
                    {
                        Bullets_In_Collar = Bullets_In_2;
                        Reload_Time = Reload_Time_2;
                    }
                    break;
                case 3:
                    {
                        Bullets_In_Collar = Bullets_In_3;
                        Reload_Time = Reload_Time_3;
                    }
                    break;
            }
            int bullets_in_case = 0;
            switch (Selected_Item)
            {
                case 0: bullets_in_case = bullets_in_case_0; break;
                case 1: bullets_in_case = bullets_in_case_1; break;
                case 2: bullets_in_case = bullets_in_case_2; break;
                case 3: bullets_in_case = bullets_in_case_3; break;
            }
            if (key.IsKeyDown(Keys.R) && !reloading_0 && bull_c < Bullets_In_Collar && bullets_in_case > 0)
            {
                reload = true;
                reloading_0 = true;
            }
            if (reloading_0) exp_time++;
            if (exp_time >= Reload_Time)
            {
                exp_time = 0;
                IsReady = true;
                reloading_0 = false;
                if (bullets_in_case >= Bullets_In_Collar - bull_c)
                {
                    bullets_in_case -= Bullets_In_Collar - bull_c;
                    bull_c = Bullets_In_Collar;
                }
                else
                {
                    bull_c += bullets_in_case;
                    bullets_in_case = 0;
                }
                switch (Selected_Item)
                {
                    case 0: bullets_in_case_0 = bullets_in_case; break;
                    case 1: bullets_in_case_1 = bullets_in_case; break;
                    case 2: bullets_in_case_2 = bullets_in_case; break;
                    case 3: bullets_in_case_3 = bullets_in_case; break;
                }
            }
            for (int i = 0; i < Bullet_Count; i++)
            {
                if (bull[i].Is_Alive == true) bull[i].AddTime();
            }
            switch (Selected_Item)
            {
                case 0: turn_0 = turn; break;
                case 1: turn_1 = turn; break;
                case 2: turn_2 = turn; break;
                case 3: turn_3 = turn; break;
            }
            if (key.IsKeyDown(Keys.D1) && !reloading_0)
            {
                switch(Selected_Item)
                {
                    case 0:
                        bull_c_0 = bull_c;
                        break;
                    case 1:
                        bull_c_1 = bull_c;
                        break;
                    case 2:
                        bull_c_2 = bull_c;
                        break;
                    case 3:
                        bull_c_3 = bull_c;
                        break;
                }
                Selected_Item = 0; bull_c = bull_c_0;
            }
            if (key.IsKeyDown(Keys.D2) && !reloading_0)
            {
                switch (Selected_Item)
                {
                    case 0:
                        bull_c_0 = bull_c;
                        break;
                    case 1:
                        bull_c_1 = bull_c;
                        break;
                    case 2:
                        bull_c_2 = bull_c;
                        break;
                    case 3:
                        bull_c_3 = bull_c;
                        break;
                }
                Selected_Item = 1; bull_c = bull_c_1;
            }
            if (key.IsKeyDown(Keys.D3) && !reloading_0)
            {
                switch (Selected_Item)
                {
                    case 0:
                        bull_c_0 = bull_c;
                        break;
                    case 1:
                        bull_c_1 = bull_c;
                        break;
                    case 2:
                        bull_c_2 = bull_c;
                        break;
                    case 3:
                        bull_c_3 = bull_c;
                        break;
                }
                Selected_Item = 2; bull_c = bull_c_2;
            }
            if (key.IsKeyDown(Keys.D4) && !reloading_0)
            {
                switch (Selected_Item)
                {
                    case 0:
                        bull_c_0 = bull_c;
                        break;
                    case 1:
                        bull_c_1 = bull_c;
                        break;
                    case 2:
                        bull_c_2 = bull_c;
                        break;
                    case 3:
                        bull_c_3 = bull_c;
                        break;
                }
                Selected_Item = 3; bull_c = bull_c_3;
            }
            for (int i = 0; i < Max_Rockets; i++)
                if (rockets[i].alive) rockets[i].Fly();
            if (hurt)
            {
                time_hurt--;
                if (time_hurt <= 0) hurt = false;
            }
        }

        public void Place(double new_X, double new_Y)
        {
            X = new_X; Y = new_Y;
        }

        public void AddRank()
        {
            bool added = false;
            for (int i = 0; i < Awards_Count; i++)
            {
                if (awards[i] < rank)
                {
                    awards[i]++;
                    added = true;
                    break;
                }
            }
            if (!added && rank < Max_Rank)
            {
                rank++;
                for (int i = 0; i < Awards_Count; i++)
                {
                    if (awards[i] < rank)
                    {
                        awards[i]++;
                        added = true;
                        break;
                    }
                }
            }
        }

        public int[] Ranks
        {
            get
            {
                return awards;
            }
        }

        public int Rank_Max
        {
            get
            {
                return Awards_Count;
            }
        }
        public bool Shot
        {
            set { shot = value; } 
            get
            {
                return shot;
            }
        }

        public bool Reload
        {
            set { reload = value; }
            get
            {
                return reload;
            }
        }

        public bool Hurt
        {
            set { }
            get
            {
                return hurt;
            }
        }

        public int HitPoints
        {
            get { return HP; }
            set { HP = value; }
        }

        public int Max_HitPoints
        {
            get { return Max_HP; }
            set { }
        }
        public bool Explotion_sound
        {
            set { explotion_sound = value; }
            get
            {
                return explotion_sound;
            }
        }

        public bool Rocket_sound
        {
            set { rocket_fly_sound = value; }
            get
            {
                return rocket_fly_sound;
            }
        }

        public bool Reloading
        {
            set { }
            get
            {
                return reloading_0;
            }
        }

        public int Case_0
        {
            get { return bullets_in_case_0; }
            set { }
        }

        public int Case_1
        {
            get { return bullets_in_case_1; }
            set { }
        }

        public int Case_2
        {
            get { return bullets_in_case_2; }
            set { }
        }

        public int Case_3
        {
            get { return bullets_in_case_3; }
            set { }
        }

        public bool Empty
        {
            set { empty = value; }
            get
            {
                return empty;
            }
        }

        public int Bullets_Count
        {
            set
            { }
            get
            { return Bullet_Count;  }
        }

        public int Selected_Wearpon
        {
            set
            { }
            get
            { return Selected_Item; }
        }

        public int Current_Bullets_Count
        {
            set
            { }
            get
            { return bull_c; }
        }

        public Bullet[] bullets
        {
            set { }
            get { return bull; }
        }
        public int pos_X
        {
            set
            {
                X = value;
            }
            get
            {
                return Convert.ToInt32(Math.Round(X));
            }
        }

        public double doub_X
        {
            set
            {
            }
            get
            {
                return X;
            }
        }

        public double doub_Y
        {
            set
            {
            }
            get
            {
                return Y;
            }
        }

        public Bullet Bullet(int i)
        {
            return bullets[i];
        }
        
        public bool alive
        {
            set { }
            get { return Alive; }
        }
        public bool Walking
        {
            set { }
            get
            {
                if (vx == 0 && vy == 0) return false;
                else return true;
            }
        }

        public int pos_Y
        {
            set
            {
                Y = value;
            }
            get
            {
                return Convert.ToInt32(Math.Round(Y));
            }
        }


        public int tar_X
        {
            set
            {
            }
            get
            {
                MouseState m = Mouse.GetState();
                return m.X;
            }
        }

        public int tar_Y
        {
            set
            {
            }
            get
            {
                MouseState m = Mouse.GetState();
                return m.Y;
            }
        }
        public int max_Rockets
        {
            set { }
            get { return Max_Rockets; }
        }

        public Rocket[] Rockets
        {
            set { }
            get { return rockets; }
        }
        public float rotate
        {
            set
            {
            }
            get
            {
                MouseState m = Mouse.GetState();
                double X0 = m.X + buff_X;
                double Y0 = m.Y + buff_Y;
                if (X - X0 != 0)
                {
                    if (X0 > X) r = (float)(Math.Atan((Y - Y0)/(X - X0)) + MathHelper.PiOver2);
                    else r = (float)(Math.Atan((Y - Y0) / (X - X0)) - MathHelper.PiOver2);
                }
                else
                {
                    if (Y0 < Y) r = 0;
                    else r = MathHelper.Pi;
                }
                return r;
            }
        }
    }
}
