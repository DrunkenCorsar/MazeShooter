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
    class Map
    {
        //  Camera_Height = 900, Camera_Width = 1100 Max_Distance_X = 575, Max_Distance_Y = 420,
        const int Max_Zombies_Count = 30, Portals_Count = 5, Zombie_Reload = 200, Rocket_Range = 150, Max_Zombie_Try = 100, Towers_Count = 2;
        double cam_X, cam_Y, center_X, center_Y;
        const double Distance_Mob = 70, Distance_Mob_Bu = 50, Distance_To_Wall = 35, Distance_To_Wall_Z = 40, Distance_Portal_Save = 50, Distance_to_zombie_Attak = 100;
        const int Min_Time_To_Zombie = 100, Max_Time_To_Zombie = 500;
        int height, width, Min_Buff_X, Max_Buff_X, Min_Buff_Y, Max_Buff_Y, ticks, n, m, Time_To_Zombie = 500;
        int Left_Width, Top_Height, Right_Width, Down_Height, Screen_Width, Screen_Height, Camera_Height, Camera_Width, Max_Distance_X, Max_Distance_Y;
        Hero hero;
        bool bullet_to_wall, bullet_to_tower;
        Zombie[] zombies = new Zombie[Max_Zombies_Count];
        int[,] Net;
        int[] Portals_X = new int[Portals_Count];
        int[] Portals_Y = new int[Portals_Count];
        GreedHelp greed;
        Tower[] tower;
        Base base_1, base_2;
        bool win = false, defeat = false;
        public Map(int Height, int Width, int Center_X, int Center_Y,int  left_Width, int top_Height, int right_Width, int down_Height, int screen_width, int screen_height)
        {
            bullet_to_wall = false;
            bullet_to_tower = false;
            center_X = Center_X;
            center_Y = Center_Y;
            Left_Width = left_Width;
            Top_Height = top_Height;
            Down_Height = down_Height;
            Right_Width = right_Width;
            Screen_Height = screen_height;
            Screen_Width = screen_width;
            height = Height;
            width = Width;
            Camera_Height = Screen_Height - Top_Height - Down_Height;
            Camera_Width = Screen_Width - Left_Width - Right_Width;
            Max_Distance_X = Camera_Width / 2;
            Max_Distance_Y = Camera_Height / 2;
            Min_Buff_X = -Left_Width;
            Min_Buff_Y = -Top_Height;
            Max_Buff_X = width - (Screen_Width - Right_Width);
            Max_Buff_Y = height - (Screen_Height - Top_Height);
            tower = new Tower[Towers_Count];
            Random o = new Random();
            for (int i = 0; i < Max_Zombies_Count; i++)
                zombies[i] = new Zombie();
            n = height / 100;
            m = width / 100;
            Net = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (i == 0 || j == 0 || i == n - 1 || j == m - 1)
                    {
                        Net[i, j] = 1;
                    }
                    else Net[i, j] = 0;
                }
            int builded = 0;
            int need = o.Next(3, 10);
            while (builded != need)
            {
                int way = o.Next(1, 5);
                switch(way)
                {
                    case 1:
                        {
                            bool place = false;
                            int x = 0, y = 0;
                            while (!place)
                            {
                                x = o.Next(1, m - 1); y = o.Next(1, n - 1);
                                if (Net[y, x + 1] == 1 && (Net[y, x - 1] + Net[y, x]+ Net[y - 1, x - 1] + Net[y - 1, x] + Net[y + 1, x - 1] + Net[y + 1, x]) == 0) place = true;
                            }
                            int l = o.Next(1, m / 2);
                            if ((x - l) > 0)
                            {
                                for (int i = x - 1; i >= x - l; i--)
                                {
                                    if (Net[y, i - 1] + Net[y + 1, i - 1] + Net[y - 1, i - 1] > 0) place = false;
                                }
                            }
                            else place = false;
                            if (place)
                            {
                                builded++;
                                for (int i = x; i >= x - l; i--) Net[y, i] = 1;
                            }
                        }
                        break;
                    case 2:
                        {
                            bool place = false;
                            int x = 0, y = 0;
                            while (!place)
                            {
                                x = o.Next(1, m - 1); y = o.Next(1, n - 1);
                                if (Net[y, x - 1] == 1 && (Net[y, x + 1] + Net[y, x] + Net[y - 1, x + 1] + Net[y - 1, x] + Net[y + 1, x + 1] + Net[y + 1, x]) == 0) place = true;
                            }
                            int l = o.Next(1, m / 2);
                            if ((x + l) < m - 1)
                            {
                                for (int i = x + 1; i <= x + l; i++)
                                {
                                    if (Net[y, i + 1] + Net[y + 1, i + 1] + Net[y - 1, i + 1] > 0) place = false;
                                }
                            }
                            else place = false;
                            if (place)
                            {
                                builded++;
                                for (int i = x; i <= x + l; i++) Net[y, i] = 1;
                            }
                        }
                        break;
                    case 3:
                        {
                            bool place = false;
                            int x = 0, y = 0;
                            while (!place)
                            {
                                x = o.Next(1, m - 1); y = o.Next(1, n - 1);
                                if (Net[y + 1, x] == 1 && (Net[y, x - 1] + Net[y, x] + Net[y, x + 1] + Net[y - 1, x - 1] + Net[y - 1, x] + Net[y - 1, x + 1]) == 0) place = true;
                            }
                            int l = o.Next(1, n / 2);
                            if ((y - l) > 0)
                            {
                                for (int i = y - 1; i >= y - l; i--)
                                {
                                    if (Net[i - 1, x - 1] + Net[i - 1, x] + Net[i - 1, x + 1] > 0) place = false;
                                }
                            }
                            else place = false;
                            if (place)
                            {
                                builded++;
                                for (int i = y; i >= y - l; i--) Net[i, x] = 1;
                            }
                        }
                        break;
                    case 4:
                        {
                            bool place = false;
                            int x = 0, y = 0;
                            while (!place)
                            {
                                x = o.Next(1, m - 1); y = o.Next(1, n - 1);
                                if (Net[y - 1, x] == 1 && (Net[y, x - 1] + Net[y, x] + Net[y, x + 1] + Net[y + 1, x - 1] + Net[y + 1, x] + Net[y + 1, x + 1]) == 0) place = true;
                            }
                            int l = o.Next(1, n / 2);
                            if ((y + l) < n - 1)
                            {
                                for (int i = y + 1; i <= y + l; i++)
                                {
                                    if (Net[i + 1, x - 1] + Net[i + 1, x] + Net[i + 1, x + 1] > 0) place = false;
                                }
                            }
                            else place = false;
                            if (place)
                            {
                                builded++;
                                for (int i = y; i <= y + l; i++) Net[i, x] = 1;
                            }
                        }
                        break;
                }
            }
            greed = new GreedHelp(Net, n, m, 100);
            for (int i = 0; i < Towers_Count; i++)
            {
                bool build = false;
                while(!build)
                {
                    Random o1 = new Random();
                    int x = o1.Next(1, n - 2), y = o.Next(1, m - 2);
                    if ((Net[x,y] == 0 && Net[x + 1, y] == 0 && Net[x + 1, y + 1] == 0 && Net[x, y + 1] == 0) &&
                        ((Net[x - 1, y] > 0 && Net[x - 1, y + 1] > 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x + 2, y - 1] == 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0 && Net[x + 2, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x, y + 2] == 0)
                        || (Net[x, y - 1] > 0 && Net[x + 1, y - 1] > 0 && Net[x - 1, y] == 0 && Net[x - 1, y + 1] == 0 && Net[x - 1, y + 2] == 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0 && Net[x + 2, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x, y + 2] == 0)
                        || (Net[x + 2, y] > 0 && Net[x + 2, y + 1] > 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x - 1, y - 1] == 0 && Net[x - 1, y] == 0 && Net[x - 1, y + 1] == 0 && Net[x - 1, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x, y + 2] == 0)
                        || (Net[x, y + 2] > 0 && Net[x + 1, y + 2] > 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x + 2, y - 1] == 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0 && Net[x - 1, y - 1] == 0 && Net[x - 1, y] == 0 && Net[x - 1, y + 1] == 0)
                        || (Net[x - 1, y] > 0 && Net[x - 1, y + 1] > 0 && Net[x, y - 1] > 0 && Net[x + 1, y - 1] > 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0 && Net[x + 2, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x, y + 2] == 0)
                        || (Net[x - 1, y] == 0 && Net[x - 1, y + 1] == 0 && Net[x, y - 1] > 0 && Net[x + 1, y - 1] > 0 && Net[x + 2, y] > 0 && Net[x + 2, y + 1] > 0 && Net[x - 1, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x, y + 2] == 0)
                        || (Net[x - 1, y] == 0 && Net[x - 1, y + 1] == 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x + 2, y] > 0 && Net[x + 2, y + 1] > 0 && Net[x - 1, y - 1] == 0 && Net[x + 1, y + 2] > 0 && Net[x, y + 2] > 0)
                        || (Net[x - 1, y] > 0 && Net[x - 1, y + 1] > 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0 && Net[x + 2, y - 1] == 0 && Net[x + 1, y + 2] > 0 && Net[x, y + 2] > 0)
                        || (Net[x - 1, y] > 0 && Net[x - 1, y + 1] > 0 && Net[x, y - 1] > 0 && Net[x + 1, y - 1] > 0 && Net[x, y + 2] > 0 && Net[x + 1, y + 2] > 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0)
                        || (Net[x - 1, y] > 0 && Net[x - 1, y + 1] > 0 && Net[x, y - 1] > 0 && Net[x + 1, y - 1] > 0 && Net[x, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x + 2, y] > 0 && Net[x + 2, y + 1] > 0)
                        || (Net[x - 1, y] == 0 && Net[x - 1, y + 1] == 0 && Net[x, y - 1] > 0 && Net[x + 1, y - 1] > 0 && Net[x, y + 2] > 0 && Net[x + 1, y + 2] > 0 && Net[x + 2, y] > 0 && Net[x + 2, y + 1] > 0)
                        || (Net[x - 1, y] > 0 && Net[x - 1, y + 1] > 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x, y + 2] > 0 && Net[x + 1, y + 2] > 0 && Net[x + 2, y] > 0 && Net[x + 2, y + 1] > 0)
                        || (Net[x - 1, y - 1] == 0 && Net[x, y - 1] == 0 && Net[x + 1, y - 1] == 0 && Net[x + 2, y - 1] == 0 && Net[x + 2, y] == 0 && Net[x + 2, y + 1] == 0 && Net[x + 2, y + 2] == 0 && Net[x + 1, y + 2] == 0 && Net[x, y + 2] == 0 && Net[x - 1, y + 2] == 0 && Net[x - 1, y + 1] == 0 && Net[x - 1, y] == 0)))
                    {
                        tower[i] = new Tower((y + 1) * 100, (x + 1) * 100, greed);
                        Net[x, y] = 5; Net[x + 1, y] = 5; Net[x + 1, y + 1] = 5; Net[x, y + 1] = 5;
                        build = true;
                    }
                }
            }

            for (int i = 0; i < Towers_Count; i++)
            {
                tower[i].Update_Map(greed);
            }

            for (int i = 0; i < Portals_Count; i++)
            {
                bool built = false;
                while (!built)
                {
                    int x = o.Next(0, m - 1), y = o.Next(0, n - 1);
                    if (Net[y,x] == 0 && Net[y + 1, x] == 0 && Net[y, x + 1] == 0 && Net[y + 1, x + 1] == 0)
                    {
                        Portals_X[i] = ((x + 1) * 100);
                        Portals_Y[i] = ((y + 1) * 100);
                        Net[y, x + 1] = 2; Net[y + 1, x] = 2;
                        Net[y, x] = 2; Net[y + 1, x + 1] = 2;
                        built = true;
                    }
                }
            }
            bool state = false; int h_x = 0, h_y = 0;
            while (!state)
            {
                h_x = o.Next(1, m - 1);
                h_y = o.Next(1, n - 1);
                if (Net[h_y, h_x] == 0) state = true;
            }
            state = false;
            int new_x = 0, new_y = 0;
            while (!state)
            {
                int x = o.Next(0, m - 1), y = o.Next(0, n - 1);
                if (Net[y, x] == 0 && Net[y + 1, x] == 0 && Net[y, x + 1] == 0 && Net[y + 1, x + 1] == 0)
                {
                    new_x = ((x + 1) * 100);
                    new_y = ((y + 1) * 100);
                    Net[y, x + 1] = 4; Net[y + 1, x] = 4;
                    Net[y, x] = 4; Net[y + 1, x + 1] = 4;
                    state = true;
                }
            }
            base_1 = new Base(new_x, new_y);
            base_1.Time_Create += 50;
            state = false;
            while (!state)
            {
                int x = o.Next(0, m - 1), y = o.Next(0, n - 1);
                if (Net[y, x] == 0 && Net[y + 1, x] == 0 && Net[y, x + 1] == 0 && Net[y + 1, x + 1] == 0)
                {
                    new_x = ((x + 1) * 100);
                    new_y = ((y + 1) * 100);
                    Net[y, x + 1] = 4; Net[y + 1, x] = 4;
                    Net[y, x] = 4; Net[y + 1, x + 1] = 4;
                    state = true;
                }
            }
            base_2 = new Base(new_x, new_y);
            hero = new Hero((h_x * 100) + 50, (h_y * 100) + 50, height, width, ((Screen_Width - Right_Width) / 100) + 1);
            greed = new GreedHelp(Net, n, m, 100);
        }

        public void Tick()
        {
            MouseState mou = Mouse.GetState();
            double mx = mou.X, my = mou.Y;
            if ((mx - center_X) > Max_Distance_X) my = center_Y + ((Max_Distance_X / (mx - center_X)) * (my - center_Y));
            if ((mx - center_X) < -Max_Distance_X) my = center_Y + ((-Max_Distance_X / (mx - center_X)) * (my - center_Y));
            if ((my - center_Y) > Max_Distance_Y) mx = center_X + ((Max_Distance_Y / (my - center_Y)) * (mx - center_X));
            if ((my - center_Y) < -Max_Distance_Y) mx = center_X + ((-Max_Distance_Y / (my - center_Y)) * (mx - center_X));
            if ((mx - center_X) > Max_Distance_X) mx = center_X + Max_Distance_X;
            if ((mx - center_X) < -Max_Distance_X) mx = center_X - Max_Distance_X;
            if ((my - center_Y) > Max_Distance_Y) my = center_Y + Max_Distance_Y;
            if ((my - center_Y) < -Max_Distance_Y) my = center_Y - Max_Distance_Y;
            cam_X = hero.pos_X - (center_X - (mx - center_X));
            cam_Y = hero.pos_Y - (center_Y - (my - center_Y));
            if (cam_X < Min_Buff_X) cam_X = Min_Buff_X;
            if (cam_Y < Min_Buff_Y) cam_Y = Min_Buff_Y;
            if (cam_X > Max_Buff_X) cam_X = Max_Buff_X;
            if (cam_Y > Max_Buff_Y) cam_Y = Max_Buff_Y;
            if (hero.alive) hero.Move(cam_X, cam_Y);
            base_1.Tick();
            base_2.Tick();
            for (int i = 0; i < Towers_Count; i++)
            {
                for (int j = 0; j < tower[i].Bullets_Count; j++)
                {
                    if (tower[i].Bullets[j].Is_Alive)
                    {
                        tower[i].Bullets[j].AddTime();
                    }
                }
                if (tower[i].Is_Alive && hero.alive)
                {
                    tower[i].Tick(hero.pos_X, hero.pos_Y);
                }
            }
            for (int i = 0; i < Towers_Count; i++)
            {
                for (int j = 0; j < tower[i].Bullets_Count; j++)
                {
                    if (tower[i].Bullets[j].Is_Alive)
                    {
                        for (int i0 = 0; i0 < Max_Zombies_Count; i0++)
                            if (zombies[i0].Alive)
                            {
                                double s1 = 0, z_x = zombies[i0].pos_X, z_y = zombies[i0].pos_Y, h_x = tower[i].Bullets[j].pos_X, h_y = tower[i].Bullets[j].pos_Y;
                                s1 = Math.Sqrt(((z_x - h_x) * (z_x - h_x)) + ((z_y - h_y) * (z_y - h_y)));
                                if (s1 < Distance_Mob_Bu)
                                {
                                    zombies[i0].Hurt(tower[i].Bullets[j].Damage);
                                    tower[i].Bullets[j].Destroy();
                                }
                            }
                        int xus = Convert.ToInt32(Math.Floor(tower[i].Bullets[j].dou_X)) / 100, yus = Convert.ToInt32(Math.Floor(tower[i].Bullets[j].dou_Y)) / 100;
                        if (!(Net[yus, xus] == 1 || Net[yus, xus] == 5) && tower[i].Bullets[j].Is_Inside) tower[i].Bullets[j].Is_Inside = false;
                        if ((Net[yus, xus] == 1 || Net[yus, xus] == 5) && !tower[i].Bullets[j].Is_Inside)
                        {
                            tower[i].Bullets[j].Destroy();
                            bullet_to_wall = true;
                        }
                        if (hero.alive)
                        {
                            double s1 = 0, z_x = hero.pos_X, z_y = hero.pos_Y, h_x = tower[i].Bullets[j].pos_X, h_y = tower[i].Bullets[j].pos_Y;
                            s1 = Math.Sqrt(((z_x - h_x) * (z_x - h_x)) + ((z_y - h_y) * (z_y - h_y)));
                            if (s1 < Distance_Mob_Bu)
                            {
                                hero.Hurted(tower[i].Bullets[j].Damage);
                                tower[i].Bullets[j].Destroy();
                            }
                        }
                    }
                }
            }
            if (base_1.Ready)
            {
                double s = Math.Sqrt(((hero.pos_X - base_1.pos_X) * (hero.pos_X - base_1.pos_X)) + ((hero.pos_Y - base_1.pos_Y) * (hero.pos_Y - base_1.pos_Y)));
                if (s < Distance_Mob)
                {
                    hero.AddItems(base_1.Item_0, base_1.Item_1, base_1.Item_2, base_1.Item_3, base_1.Health);
                    base_1.Take_Items();
                }
            }
            if (base_2.Ready)
            {
                double s = Math.Sqrt(((hero.pos_X - base_2.pos_X) * (hero.pos_X - base_2.pos_X)) + ((hero.pos_Y - base_2.pos_Y) * (hero.pos_Y - base_2.pos_Y)));
                if (s < Distance_Mob)
                {
                    hero.AddItems(base_2.Item_0, base_2.Item_1, base_2.Item_2, base_2.Item_3, base_2.Health);
                    base_2.Take_Items();
                }
            }
            ticks++;
            if (ticks >= Time_To_Zombie)
            {
                ticks -= Time_To_Zombie;
                if (Time_To_Zombie > Min_Time_To_Zombie) Time_To_Zombie--;
                int n = -1;
                for (int i = 0; i < Max_Zombies_Count; i++)
                {
                    if (!zombies[i].Alive && !zombies[i].Body)
                    {
                        if (n == -1) n = i;
                        /*
                        Random o = new Random();
                        int port = o.Next(0, Portals_Count);
                        zombies[i].Born(Portals_X[port], Portals_Y[port], o.Next(1, 4), hero.pos_X, hero.pos_Y, o.Next(5, 20), width, height);
                        break; */
                    }
                }
                for (int i = 0; i < Max_Zombie_Try; i++)
                {
                    Random o = new Random();
                    int port = o.Next(0, Portals_Count);
                    double x = Portals_X[port], y = Portals_Y[port];
                    double x1 = hero.doub_X, y1 = hero.doub_Y;
                    double s = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                    bool save = true;
                    if (s > Distance_Portal_Save)
                    {
                        for (int j = 0; j < Max_Zombies_Count; j++)
                        {
                            if (zombies[j].Alive)
                            {
                                x1 = zombies[j].doub_X; y1 = zombies[j].doub_Y;
                                s = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                                if (s <= Distance_Portal_Save) save = false;
                            }
                        }
                    }
                    else save = false;
                    if (save && n != -1)
                    {
                        zombies[n].Born(Portals_X[port], Portals_Y[port], o.Next(1, 4), hero.pos_X, hero.pos_Y, o.Next((((Max_Time_To_Zombie - Time_To_Zombie) / 2) + 20) / 4, ((Max_Time_To_Zombie - Time_To_Zombie) / 2) + 20), width, height);
                        break;
                    }
                }
            }
            for (int i = 0; i < Max_Zombies_Count; i++)
                if (zombies[i].Alive && !win)
                {
                    bool walk = false;
                    double T_x = 0, T_y = 0;
                    if (greed.IsPerfectlyClear(zombies[i].doub_X, zombies[i].doub_Y, hero.doub_X, hero.doub_Y))
                    {
                        walk = true;
                        T_x = hero.pos_X;
                        T_y = hero.pos_Y;
                    }
                    if (!walk && zombies[i].Check)
                    {
                        zombies[i].Check = false;
                        greed.Calculate(zombies[i].doub_X, zombies[i].doub_Y, hero.doub_X, hero.doub_Y, out T_x, out T_y, out walk);
                    }
                    if (walk)
                    {
                        zombies[i].Attak(Convert.ToInt32(Math.Round(T_x)), Convert.ToInt32(Math.Round(T_y)));
                    }
                    else
                    {
                        if (zombies[i].staying) zombies[i].Stay();
                        else
                        {
                            if (zombies[i].Find)
                            {
                                zombies[i].Find = false;
                                greed.Find_Place(zombies[i].doub_X, zombies[i].doub_Y, out T_x, out T_y, out walk);
                                if (!walk) zombies[i].Stay();
                                else
                                {
                                    zombies[i].Walk(Convert.ToInt32(Math.Floor(T_x)), Convert.ToInt32(Math.Floor(T_y)));
                                    zombies[i].attaking = false;
                                }
                            }
                            else zombies[i].Walk();
                        }
                    }
                }
                else if (zombies[i].Body) zombies[i].Lie();
            bool Win_0 = true;
            for (int i = 0; i < Max_Towers; i++)
            {
                if (tower[i].Is_Alive) Win_0 = false;
            }
            if (Win_0) win = true;
            if (!hero.alive && !defeat) defeat = true;
            for (int i = 0; i < Max_Zombies_Count; i++)
            {
                if (zombies[i].Alive)
                {
                    double s = 0;
                    for (int j = 0; j < Max_Zombies_Count; j++)
                    {
                        if (zombies[j].Alive && j != i)
                        {
                            s = Math.Sqrt(((zombies[i].pos_X - zombies[j].pos_X) * (zombies[i].pos_X - zombies[j].pos_X)) + ((zombies[i].pos_Y - zombies[j].pos_Y) * (zombies[i].pos_Y - zombies[j].pos_Y)));
                            if (s < Distance_Mob && s != 0)
                            {
                                int hero_X = Convert.ToInt32(Math.Round((double)((zombies[j].pos_X + zombies[i].pos_X) / 2) - ((Distance_Mob / s) * ((zombies[j].pos_X - zombies[i].pos_X) / 2))));
                                int hero_Y = Convert.ToInt32(Math.Round((double)((zombies[j].pos_Y + zombies[i].pos_Y) / 2) - ((Distance_Mob / s) * ((zombies[j].pos_Y - zombies[i].pos_Y) / 2))));
                                int z_X = Convert.ToInt32(Math.Round((double)((zombies[j].pos_X + zombies[i].pos_X) / 2) + ((Distance_Mob / s) * ((zombies[j].pos_X - zombies[i].pos_X) / 2))));
                                int z_Y = Convert.ToInt32(Math.Round((double)((zombies[j].pos_Y + zombies[i].pos_Y) / 2) + ((Distance_Mob / s) * ((zombies[j].pos_Y - zombies[i].pos_Y) / 2))));
                                zombies[i].pos_X = hero_X; zombies[i].pos_Y = hero_Y; zombies[j].pos_X = z_X; zombies[j].pos_Y = z_Y;
                            }
                        }
                    }
                    s = Math.Sqrt(((zombies[i].pos_X - hero.pos_X) * (zombies[i].pos_X - hero.pos_X)) + ((zombies[i].pos_Y - hero.pos_Y) * (zombies[i].pos_Y - hero.pos_Y)));
                    if (s < Distance_Mob)
                    {
                        if (hero.alive)
                        {
                            int hero_X = Convert.ToInt32(Math.Round((double)((zombies[i].pos_X + hero.pos_X) / 2) - ((Distance_Mob / s) * ((zombies[i].pos_X - hero.pos_X) / 2))));
                            int hero_Y = Convert.ToInt32(Math.Round((double)((zombies[i].pos_Y + hero.pos_Y) / 2) - ((Distance_Mob / s) * ((zombies[i].pos_Y - hero.pos_Y) / 2))));
                            int z_X = Convert.ToInt32(Math.Round((double)((zombies[i].pos_X + hero.pos_X) / 2) + ((Distance_Mob / s) * ((zombies[i].pos_X - hero.pos_X) / 2))));
                            int z_Y = Convert.ToInt32(Math.Round((double)((zombies[i].pos_Y + hero.pos_Y) / 2) + ((Distance_Mob / s) * ((zombies[i].pos_Y - hero.pos_Y) / 2))));
                            hero.pos_X = hero_X; hero.pos_Y = hero_Y; zombies[i].pos_X = z_X; zombies[i].pos_Y = z_Y;
                        }
                        else
                        {
                            int z_X = Convert.ToInt32(Math.Round((double)((zombies[i].pos_X + hero.pos_X) / 2) + ((Distance_Mob / s) * ((zombies[i].pos_X - hero.pos_X) / 2))));
                            int z_Y = Convert.ToInt32(Math.Round((double)((zombies[i].pos_Y + hero.pos_Y) / 2) + ((Distance_Mob / s) * ((zombies[i].pos_Y - hero.pos_Y) / 2))));
                            zombies[i].pos_X = z_X; zombies[i].pos_Y = z_Y;
                        }
                    }
                    if (s < Distance_to_zombie_Attak && zombies[i].Boxing)
                    {
                        zombies[i].BoxBox();
                        hero.Hurted(zombies[i].Dmg);
                    }
                }
            }
            for (int i = 0; i < hero.Bullets_Count; i++)
            {
                if (hero.bullets[i].Is_Alive)
                {
                    for (int j = 0; j < Max_Zombies_Count; j++)
                        if (zombies[j].Alive)
                        {
                            double s1 = 0, z_x = zombies[j].pos_X, z_y = zombies[j].pos_Y, h_x = hero.bullets[i].pos_X, h_y = hero.bullets[i].pos_Y;
                            s1 = Math.Sqrt(((z_x - h_x) * (z_x - h_x)) + ((z_y - h_y) * (z_y - h_y)));
                            if (s1 < Distance_Mob_Bu)
                            {
                                zombies[j].Hurt(hero.bullets[i].Damage);
                                if (!zombies[j].Alive) hero.AddRank();
                                hero.bullets[i].Destroy();
                            }
                        }
                    int xus = Convert.ToInt32(Math.Floor(hero.bullets[i].dou_X)) / 100, yus = Convert.ToInt32(Math.Floor(hero.bullets[i].dou_Y)) / 100;
                    if (Net[yus, xus] == 1)
                    {
                        bullet_to_wall = true;
                        hero.bullets[i].Destroy();
                    }
                    if (Net[yus, xus] == 5)
                    {
                        bool first = true;
                        for (int j = 0; j < Towers_Count; j++)
                        {
                            if (first && tower[j].Is_Alive && Math.Abs(tower[j].pos_X - hero.bullets[i].pos_X) <= 100 && Math.Abs(tower[j].pos_Y - hero.bullets[i].pos_Y) <= 100)
                            {
                                bullet_to_tower = true;
                                tower[j].Hurt(hero.bullets[i].Damage);
                                if (!tower[j].Is_Alive)
                                {
                                    Net[tower[j].pos_Y / 100, tower[j].pos_X / 100] = 3;
                                    Net[(tower[j].pos_Y / 100) - 1, tower[j].pos_X / 100] = 3;
                                    Net[(tower[j].pos_Y / 100) - 1, (tower[j].pos_X / 100) - 1] = 3;
                                    Net[tower[j].pos_Y / 100, (tower[j].pos_X / 100) - 1] = 3;
                                    greed = new GreedHelp(Net, n, m, 100);
                                    for (int i0 = 0; i0 < Towers_Count; i0++)
                                    {
                                        if (tower[i0].Is_Alive) tower[i0].Update_Map(greed);
                                    }
                                }
                                first = false;
                            }
                        }
                        hero.bullets[i].Destroy();
                    }
                }
            }
            bool always = true;
            if (always)
            {
                double S = 0;
                double x = hero.doub_X, y = hero.doub_Y; int y0 = Convert.ToInt32(Math.Floor(y)) / 100, x0 = Convert.ToInt32(Math.Floor(x)) / 100;
                bool w1 = false, w2 = false, w3 = false, w4 = false, w5 = false, w6 = false, w7 = false, w8 = false;
                if (Net[y0 - 1, x0 - 1] == 1 || Net[y0 - 1, x0 - 1] == 5) w1 = true;
                if (Net[y0 - 1, x0 + 1] == 1 || Net[y0 - 1, x0 + 1] == 5) w2 = true;
                if (Net[y0 + 1, x0 + 1] == 1 || Net[y0 + 1, x0 + 1] == 5) w3 = true;
                if (Net[y0 + 1, x0 - 1] == 1 || Net[y0 + 1, x0 + 1] == 5) w4 = true;
                if (Net[y0, x0 - 1] == 1 || Net[y0, x0 - 1] == 5) w5 = true;
                if (Net[y0 - 1, x0] == 1 || Net[y0 - 1, x0] == 5) w6 = true;
                if (Net[y0, x0 + 1] == 1 || Net[y0, x0 + 1] == 5) w7 = true;
                if (Net[y0 + 1, x0] == 1 || Net[y0 + 1, x0] == 5) w8 = true;
                double x1 = x0 * 100, y1 = y0 * 100, x2 = x1 + 100, y2 = y1 + 100;
                if (w1)
                {
                    S = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                    if (S < Distance_To_Wall)
                    {
                        x = x1 + ((Distance_To_Wall / S) * (x - x1));
                        y = y1 + ((Distance_To_Wall / S) * (y - y1));
                    }
                }
                if (w2)
                {
                    S = Math.Sqrt(((x - x2) * (x - x2)) + ((y - y1) * (y - y1)));
                    if (S < Distance_To_Wall)
                    {
                        x = x2 + ((Distance_To_Wall / S) * (x - x2));
                        y = y1 + ((Distance_To_Wall / S) * (y - y1));
                    }
                }
                if (w3)
                {
                    S = Math.Sqrt(((x - x2) * (x - x2)) + ((y - y2) * (y - y2)));
                    if (S < Distance_To_Wall)
                    {
                        x = x2 + ((Distance_To_Wall / S) * (x - x2));
                        y = y2 + ((Distance_To_Wall / S) * (y - y2));
                    }
                }
                if (w4)
                {
                    S = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y2) * (y - y2)));
                    if (S < Distance_To_Wall)
                    {
                        x = x1 + ((Distance_To_Wall / S) * (x - x1));
                        y = y2 + ((Distance_To_Wall / S) * (y - y2));
                    }
                }
                if (w5)
                {
                    S = x - x1;
                    if (S < Distance_To_Wall) x = x1 + Distance_To_Wall;
                }
                if (w6)
                {
                    S = y - y1;
                    if (S < Distance_To_Wall) y = y1 + Distance_To_Wall;
                }
                if (w7)
                {
                    S = x2 - x;
                    if (S < Distance_To_Wall) x = x2 - Distance_To_Wall;
                }
                if (w8)
                {
                    S = y2 - y;
                    if (S < Distance_To_Wall) y = y2 - Distance_To_Wall;
                }
                hero.Place(x, y);
            }
            for(int i = 0; i < Max_Zombies_Count; i++)
            {
                if (zombies[i].Alive)
                {
                    double S = 0;
                    double x = zombies[i].doub_X, y = zombies[i].doub_Y; int y0 = Convert.ToInt32(Math.Floor(y)) / 100, x0 = Convert.ToInt32(Math.Floor(x)) / 100;
                    bool w1 = false, w2 = false, w3 = false, w4 = false, w5 = false, w6 = false, w7 = false, w8 = false;
                    if (Net[y0 - 1, x0 - 1] == 1 || Net[y0 - 1, x0 - 1] == 5) w1 = true;
                    if (Net[y0 - 1, x0 + 1] == 1 || Net[y0 - 1, x0 + 1] == 5) w2 = true;
                    if (Net[y0 + 1, x0 + 1] == 1 || Net[y0 + 1, x0 + 1] == 5) w3 = true;
                    if (Net[y0 + 1, x0 - 1] == 1 || Net[y0 + 1, x0 + 1] == 5) w4 = true;
                    if (Net[y0, x0 - 1] == 1 || Net[y0, x0 - 1] == 5) w5 = true;
                    if (Net[y0 - 1, x0] == 1 || Net[y0 - 1, x0] == 5) w6 = true;
                    if (Net[y0, x0 + 1] == 1 || Net[y0, x0 + 1] == 5) w7 = true;
                    if (Net[y0 + 1, x0] == 1 || Net[y0 + 1, x0] == 5) w8 = true;
                    double x1 = x0 * 100, y1 = y0 * 100, x2 = x1 + 100, y2 = y1 + 100;
                    if (w1)
                    {
                        S = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                        if (S < Distance_To_Wall_Z)
                        {
                            x = x1 + ((Distance_To_Wall_Z / S) * (x - x1));
                            y = y1 + ((Distance_To_Wall_Z / S) * (y - y1));
                        }
                    }
                    if (w2)
                    {
                        S = Math.Sqrt(((x - x2) * (x - x2)) + ((y - y1) * (y - y1)));
                        if (S < Distance_To_Wall_Z)
                        {
                            x = x2 + ((Distance_To_Wall_Z / S) * (x - x2));
                            y = y1 + ((Distance_To_Wall_Z / S) * (y - y1));
                        }
                    }
                    if (w3)
                    {
                        S = Math.Sqrt(((x - x2) * (x - x2)) + ((y - y2) * (y - y2)));
                        if (S < Distance_To_Wall_Z)
                        {
                            x = x2 + ((Distance_To_Wall_Z / S) * (x - x2));
                            y = y2 + ((Distance_To_Wall_Z / S) * (y - y2));
                        }
                    }
                    if (w4)
                    {
                        S = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y2) * (y - y2)));
                        if (S < Distance_To_Wall_Z)
                        {
                            x = x1 + ((Distance_To_Wall_Z / S) * (x - x1));
                            y = y2 + ((Distance_To_Wall_Z / S) * (y - y2));
                        }
                    }
                    if (w5)
                    {
                        S = x - x1;
                        if (S < Distance_To_Wall_Z) x = x1 + Distance_To_Wall_Z;
                    }
                    if (w6)
                    {
                        S = y - y1;
                        if (S < Distance_To_Wall_Z) y = y1 + Distance_To_Wall_Z;
                    }
                    if (w7)
                    {
                        S = x2 - x;
                        if (S < Distance_To_Wall_Z) x = x2 - Distance_To_Wall_Z;
                    }
                    if (w8)
                    {
                        S = y2 - y;
                        if (S < Distance_To_Wall_Z) y = y2 - Distance_To_Wall_Z;
                    }
                    zombies[i].Place(x, y);
                }
            }
            for (int i = 0; i < hero.max_Rockets; i++)
            {
                if (hero.Rockets[i].alive)
                {
                    double x = hero.Rockets[i].dou_X, y = hero.Rockets[i].dou_Y;
                    for (int j = 0; j < Max_Zombies_Count; j++)
                    {
                        if (zombies[j].Alive)
                        {
                            double x1 = zombies[j].doub_X, y1 = zombies[j].doub_Y;
                            double s = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                            if (s < Distance_Mob_Bu)
                            {
                                hero.Rockets[i].Explose();
                                hero.Explotion_sound = true;
                                for (int l = 0; l < Max_Zombies_Count; l++)
                                {
                                    if (zombies[l].Alive)
                                    {
                                        x1 = zombies[l].doub_X; y1 = zombies[l].doub_Y;
                                        s = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                                        if (s < Rocket_Range)
                                        {
                                            zombies[l].Alive = false;
                                            zombies[l].Body = true;
                                        }
                                        int x2 = Convert.ToInt32(Math.Floor(x)) / 100, y2 = Convert.ToInt32(Math.Floor(y)) / 100;
                                        for (int i1 = y2 - 1; i1 <= y2 + 1; i1++)
                                        {
                                            for (int j1 = x2 - 1; j1 <= x2 + 1; j1++)
                                            {
                                                if (i1 > 0 && i1 < n - 1 && j1 > 0 && j1 < m - 1)
                                                {
                                                    if (Net[i1, j1] == 1) Net[i1, j1] = 3;
                                                }
                                            }
                                        }
                                        greed = new GreedHelp(Net, n, m, 100);
                                    }
                                }
                            }
                        }
                    }
                    int x3 = Convert.ToInt32(Math.Floor(x)) / 100, y3 = Convert.ToInt32(Math.Floor(y)) / 100;
                    if (y3 >= 0 && y3 < n && x3 >= 0 && x3 < m)
                    {
                        if (Net[y3, x3] == 1 || Net[y3, x3] == 5)
                        {
                            if (Net[y3, x3] == 5)
                            {
                                bool first = true;
                                for (int j = 0; j < Towers_Count; j++)
                                {
                                    if (first && tower[j].Is_Alive && Math.Abs(tower[j].pos_X - hero.Rockets[i].pos_X) <= 100 && Math.Abs(tower[j].pos_Y - hero.Rockets[i].pos_Y) <= 100)
                                    {
                                        tower[j].Hurt(hero.Rockets[i].Damage);
                                        if (!tower[j].Is_Alive)
                                        {
                                            Net[tower[j].pos_Y / 100, tower[j].pos_X / 100] = 3;
                                            Net[(tower[j].pos_Y / 100) - 1, tower[j].pos_X / 100] = 3;
                                            Net[(tower[j].pos_Y / 100) - 1, (tower[j].pos_X / 100) - 1] = 3;
                                            Net[tower[j].pos_Y / 100, (tower[j].pos_X / 100) - 1] = 3;
                                            greed = new GreedHelp(Net, n, m, 100);
                                            for (int i0 = 0; i0 < Towers_Count; i0++)
                                            {
                                                if (tower[i0].Is_Alive) tower[i0].Update_Map(greed);
                                            }
                                        }
                                        first = false;
                                    }
                                }
                            }
                            hero.Rockets[i].Explose();
                            hero.Explotion_sound = true;
                            for (int l = 0; l < Max_Zombies_Count; l++)
                            {
                                if (zombies[l].Alive)
                                {
                                    double x1 = zombies[l].doub_X, y1 = zombies[l].doub_Y;
                                    double s = Math.Sqrt(((x - x1) * (x - x1)) + ((y - y1) * (y - y1)));
                                    if (s < Rocket_Range)
                                    {
                                        zombies[l].Alive = false;
                                        zombies[l].Body = true;
                                    }
                                    int x2 = Convert.ToInt32(Math.Floor(x)) / 100, y2 = Convert.ToInt32(Math.Floor(y)) / 100;
                                    for (int i1 = y2 - 1; i1 <= y2 + 1; i1++)
                                    {
                                        for (int j1 = x2 - 1; j1 <= x2 + 1; j1++)
                                        {
                                            if (i1 > 0 && i1 < n - 1 && j1 > 0 && j1 < m - 1)
                                            {
                                                if (Net[i1, j1] == 1) Net[i1, j1] = 3;
                                            }
                                        }
                                    }
                                    greed = new GreedHelp(Net, n, m, 100);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Tower[] Towers
        {
            get { return tower; }
        }

        public int Max_Towers
        {
            get { return Towers_Count; }
        }
        public int Buff_X
        {
            set { }
            get { return Convert.ToInt32(Math.Round(cam_X)); }
        }

        public int Buff_Y
        {
            set { }
            get { return Convert.ToInt32(Math.Round(cam_Y)); }
        }

        public Base Base_1
        {
            get { return base_1; }
        }

        public Base Base_2
        {
            get { return base_2; }
        }

        public bool Sound_Bullet_To_Wall
        {
            get { return bullet_to_wall; }
            set { bullet_to_wall = value; }
        }

        public bool Sound_Bullet_To_Tower
        {
            get { return bullet_to_tower; }
            set { bullet_to_tower = value; }
        }

        public Zombie[] zombie
        {
            set { }
            get { return zombies; }
        }
        public int Max_Zombies
        {
            set { }
            get { return Max_Zombies_Count; }
        }

        public int Portals
        {
            set { }
            get { return Portals_Count; }
        }

        public bool Is_Defeat
        {
            get { return defeat; }
            set { defeat = value; }
        }

        public bool Is_Win
        {
            get { return win; }
            set { win = value; }
        }
        public int[,] NET
        {
            set { }
            get { return Net; }
        }

        public int M
        {
            set { }
            get { return m; }
        }

        public int N
        {
            set { }
            get { return n; }
        }

        public int[] portals_X
        {
            set { }
            get { return Portals_X; }
        }

        public int[] portals_Y
        {
            set { }
            get { return Portals_Y; }
        }

        public Hero Main_Hero
        {
            set { }
            get { return hero; }
        }
    }
}
