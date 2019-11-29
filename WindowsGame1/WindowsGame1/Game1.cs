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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int Max_Rockets = 10, Max_Songs = 10;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D back_0, back_1, hero_0, hero_1, hero_2, hero_3, target, bullet, hero_move_0, hero_move_1, hero_move_2, hero_move_3, stone;
        Texture2D zombie, zombie_dead, zombie_hurt, zombie_staying, portal, explotion, rocket, stone2, base_texture;
        Texture2D Left, Right, Top, Down, Corner_1, Corner_2, Corner_3, Corner_4, zombie_hitting, hero_hurted, tower_hurted;
        Texture2D wearpon_0, wearpon_1, wearpon_2, wearpon_3, panel, empty_panel, panel_hp, hero_dead, tower_0, tower_1;
        Texture2D Bullets_0_0, Bullets_0, Bullets_1_0, Bullets_1, Bullets_2_0, Bullets_2, Bullets_3_0, Bullets_3, Victory, Defeat;
        Texture2D[] stars = new Texture2D[Stars_Count];
        SoundEffect shot_1, reload_1, empty_1, shot_2, reload_2, empty_2, shot_3, reload_3, empty_3, explotion_1, rocket_fly, shot_tower, find_ammo;
        SoundEffect bullet_to_wall, bullet_to_tower;
        Song[] songs = new Song[Max_Songs];
        int Selected_Song = -1;
        double Time_Song = 0;
        KeyboardState keys;
        Song All_Songs;
        string check = "";
        SpriteFont bullets_count, bullets_new;
        MouseState mouse;
        Rectangle frame_0;
        Rectangle[] frame_1 = new Rectangle[Max_Rockets];
        const int Song_count = 1, map_width = 3000, map_height = 2000, Max_Timer = 1000, Stars_Count = 10;
        //int center_X = 704, center_Y = 540;
        int center_X, center_Y;
        Map map;
        int frame_n_0 = 0, frame_count_0 = 7, frame_width_0 = 111, frame_height_0 = 111, Screen_Width, Screen_Height;
        int frame_count_1 = 81, frame_width_1 = 200, frame_height_1 = 200, max_frame_n_1 = 9;
        double frame_time_0 = 0.2, animation_time_0 = 0;
        double frame_time_1 = 0.03;
        int[] frame_n_1 = new int[Max_Rockets], frame_m_1 = new int[Max_Rockets];
        double[] animation_time_1 = new double[Max_Rockets];
        bool[] animation_1 = new bool[Max_Rockets];
        bool IsDefeat = false, IsWin = false;
        int Timer_Win = 0, Timer_Defeat = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; 
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;  
            graphics.IsFullScreen = true; 
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            for (int i = 0; i < Stars_Count; i++) stars[i] = Content.Load<Texture2D>("Star_" + Convert.ToString(i));
            Right = Content.Load<Texture2D>("Right");
            Left = Content.Load<Texture2D>("Left");
            Top = Content.Load<Texture2D>("Top");
            Down = Content.Load<Texture2D>("Down");
            tower_hurted = Content.Load<Texture2D>("Tower_Hurted");
            tower_0 = Content.Load<Texture2D>("Tower_0");
            tower_1 = Content.Load<Texture2D>("Tower_1");
            Corner_1 = Content.Load<Texture2D>("1_corner");
            Corner_2 = Content.Load<Texture2D>("2_corner");
            Corner_3 = Content.Load<Texture2D>("3_corner");
            Corner_4 = Content.Load<Texture2D>("4_corner");
            base_texture = Content.Load<Texture2D>("Base");
            zombie_hitting = Content.Load<Texture2D>("Zombie_Hitting");
            hero_hurted = Content.Load<Texture2D>("Hero_Hurted");
            back_1 = Content.Load<Texture2D>("back_1");
            hero_dead = Content.Load<Texture2D>("Dead_Hero");
            back_0 = Content.Load<Texture2D>("back_0");
            hero_0 = Content.Load<Texture2D>("hero_0");
            hero_1 = Content.Load<Texture2D>("hero_1");
            hero_2 = Content.Load<Texture2D>("hero_2");
            hero_3 = Content.Load<Texture2D>("hero_3");
            empty_panel = Content.Load<Texture2D>("Empty_Scale");
            panel_hp = Content.Load<Texture2D>("Scale_HP");
            Victory = Content.Load<Texture2D>("Victory");
            Defeat = Content.Load<Texture2D>("Defeat");
            wearpon_0 = Content.Load<Texture2D>("Wearpon_0");
            wearpon_1 = Content.Load<Texture2D>("Wearpon_1");
            wearpon_2 = Content.Load<Texture2D>("Wearpon_2");
            wearpon_3 = Content.Load<Texture2D>("Wearpon_3");
            Bullets_0 = Content.Load<Texture2D>("Bullets_0");
            panel = Content.Load<Texture2D>("Panel");
            Bullets_0_0 = Content.Load<Texture2D>("Bullets_0_0");
            Bullets_1 = Content.Load<Texture2D>("Bullets_1");
            Bullets_1_0 = Content.Load<Texture2D>("Bullets_1_0");
            Bullets_2 = Content.Load<Texture2D>("Bullets_2");
            Bullets_2_0 = Content.Load<Texture2D>("Bullets_2_0");
            Bullets_3 = Content.Load<Texture2D>("Bullets_3");
            Bullets_3_0 = Content.Load<Texture2D>("Bullets_3_0");
            bullets_new = Content.Load<SpriteFont>("bullets_new");
            target = Content.Load<Texture2D>("target");
            bullet = Content.Load<Texture2D>("bullet");
            portal = Content.Load<Texture2D>("Portal");
            zombie = Content.Load<Texture2D>("Zombie");
            rocket = Content.Load<Texture2D>("Rocket");
            explotion = Content.Load<Texture2D>("Explotion");
            zombie_hurt = Content.Load<Texture2D>("Zombie_Hurt");
            zombie_dead = Content.Load<Texture2D>("Zombie_Dead");
            zombie_staying = Content.Load<Texture2D>("Zombie_Stay");
            hero_move_0 = Content.Load<Texture2D>("hero_move_0");
            hero_move_1 = Content.Load<Texture2D>("hero_move_1");
            hero_move_2 = Content.Load<Texture2D>("hero_move_2");
            hero_move_3 = Content.Load<Texture2D>("hero_move_3");
            stone = Content.Load<Texture2D>("Stone_1");
            stone2 = Content.Load<Texture2D>("Stone_2");
            shot_1 = Content.Load<SoundEffect>("Shot_1");
            reload_1 = Content.Load<SoundEffect>("Reload_1");
            empty_1 = Content.Load<SoundEffect>("Empty_1");
            shot_2 = Content.Load<SoundEffect>("Shot_2");
            reload_2 = Content.Load<SoundEffect>("Reload_2");
            empty_2 = Content.Load<SoundEffect>("Empty_2");
            shot_3 = Content.Load<SoundEffect>("Shot_3");
            reload_3 = Content.Load<SoundEffect>("Reload_3");
            empty_3 = Content.Load<SoundEffect>("Empty_3");
            find_ammo = Content.Load<SoundEffect>("Find_Ammo");
            shot_tower = Content.Load<SoundEffect>("Shot_Tower");
            explotion_1 = Content.Load<SoundEffect>("Explotion_1");
            rocket_fly = Content.Load<SoundEffect>("Rocket_Fly");
            bullet_to_wall = Content.Load<SoundEffect>("Bullet_To_Wall");
            bullet_to_tower = Content.Load<SoundEffect>("Bullet_To_Tower");
            All_Songs = Content.Load<Song>("All_Songs");
            for (int i = 0; i < Max_Songs; i++)
            {
                songs[i] = Content.Load<Song>("Song_" + Convert.ToString(i + 1));
            }
            bullets_count = Content.Load<SpriteFont>("bullets_count");
            Screen_Height = graphics.PreferredBackBufferHeight;
            Screen_Width = graphics.PreferredBackBufferWidth;
            center_X = ((Screen_Width - Right.Width - Left.Width) / 2) + Left.Width;
            center_Y = ((Screen_Height - Down.Height - Top.Height) / 2) + Top.Height;
            map = new Map(map_height, map_width, center_X, center_Y, Left.Width, Top.Height, Right.Width, Down.Height, Screen_Width, Screen_Height);
            //MediaPlayer.Play(song_1);
            //MediaPlayer.Volume = 1;
            MediaPlayer.Volume = (float)0.5;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Time_Song == 0 || ((float)gameTime.TotalGameTime.TotalSeconds - Time_Song > (float)songs[Selected_Song].Duration.TotalSeconds))
            {
                if (Time_Song == 0) Time_Song = 0.01;
                else Time_Song = (float)gameTime.TotalGameTime.TotalSeconds;
                Random o = new Random();
                Selected_Song = o.Next(0, Max_Songs);
                MediaPlayer.Play(songs[Selected_Song]);
            }
            //check = "Song\"" + songs[Selected_Song].Name + "\" " + "Dur." + Convert.ToString((float)songs[Selected_Song].Duration.TotalSeconds) + " GT." + Convert.ToString((float)gameTime.TotalGameTime.TotalSeconds);
            animation_time_0 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animation_time_0 > frame_time_0)
            {
                animation_time_0 -= frame_time_0;
                frame_n_0++;
                if (frame_n_0 >= frame_count_0) frame_n_0 = 0; 
            }
            frame_0 = new Rectangle(frame_width_0 * frame_n_0, 0, frame_width_0, frame_height_0);
            for (int i = 0; i < Max_Rockets; i++)
            {
                if (animation_1[i])
                {
                    animation_time_1[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (animation_time_1[i] > frame_time_1)
                    {
                        animation_time_1[i] -= frame_time_1;
                        frame_n_1[i]++;
                        if (frame_n_1[i] >= frame_count_1) frame_n_1[i] = 0;
                    }
                    if (frame_n_1[i] >= max_frame_n_1)
                    {
                        frame_n_1[i] -= max_frame_n_1;
                        frame_m_1[i]++;
                        if (frame_m_1[i] >= max_frame_n_1)
                        {
                            animation_1[i] = false;
                            map.Main_Hero.Rockets[i].Explosion = false;
                            animation_time_1[i] = 0;
                            frame_n_1[i] = 0;
                            frame_m_1[i] = 0;
                        }
                    }
                    frame_1[i] = new Rectangle(frame_width_1 * frame_n_1[i], frame_height_1 * frame_m_1[i], frame_width_1, frame_height_1);
                }
            }
            if (map.Is_Win && !IsWin && !IsDefeat)
            {
                IsWin = true;
            }
            if (map.Is_Defeat && !IsDefeat && !IsWin)
            {
                IsDefeat = true;
            }
            if (IsWin) Timer_Win++;
            if (IsDefeat) Timer_Defeat++;
            if (Timer_Defeat >= Max_Timer || Timer_Win >= Max_Timer) this.Exit();
            keys = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (keys.IsKeyDown(Keys.Escape)) this.Exit();
            map.Tick();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            int buff_x = map.Buff_X, buff_y = map.Buff_Y;
            spriteBatch.Begin();
            for (int i = 0; i < (map_width / back_1.Width) + 1; i++)
                for (int j = 0; j < (map_height / back_1.Height) + 1; j++)
                    spriteBatch.Draw(back_1, new Vector2((i * back_1.Width) - buff_x, (j * back_1.Height) - buff_y), Color.White);
            int[,] Net = map.NET;
            for (int i = 0; i < map.N; i++)
            {
                for (int j = 0; j < map.M; j++)
                {
                    if (Net[i, j] == 3)
                        spriteBatch.Draw(stone2, new Vector2((j * stone.Width) - buff_x, (i * stone.Height) - buff_y), Color.White);
                }
            }
            spriteBatch.Draw(base_texture, new Vector2(map.Base_1.pos_X - buff_x - (base_texture.Width / 2), map.Base_1.pos_Y - buff_y - (base_texture.Height / 2)), Color.White);
            if (map.Base_1.Ready) switch(map.Base_1.Type)
                {
                    case 0: spriteBatch.Draw(Bullets_0, new Vector2(map.Base_1.pos_X - buff_x - (Bullets_0.Width / 2), map.Base_1.pos_Y - buff_y - (Bullets_0.Height / 2)), Color.White); break;
                    case 1: spriteBatch.Draw(Bullets_1, new Vector2(map.Base_1.pos_X - buff_x - (Bullets_1.Width / 2), map.Base_1.pos_Y - buff_y - (Bullets_1.Height / 2)), Color.White); break;
                    case 2: spriteBatch.Draw(Bullets_2, new Vector2(map.Base_1.pos_X - buff_x - (Bullets_2.Width / 2), map.Base_1.pos_Y - buff_y - (Bullets_2.Height / 2)), Color.White); break;
                    case 3: spriteBatch.Draw(Bullets_3, new Vector2(map.Base_1.pos_X - buff_x - (Bullets_3.Width / 2), map.Base_1.pos_Y - buff_y - (Bullets_3.Height / 2)), Color.White); break;
                }
            spriteBatch.Draw(base_texture, new Vector2(map.Base_2.pos_X - buff_x - (base_texture.Width / 2), map.Base_2.pos_Y - buff_y - (base_texture.Height / 2)), Color.White);
            if (map.Base_2.Ready) switch (map.Base_2.Type)
                {
                    case 0: spriteBatch.Draw(Bullets_0, new Vector2(map.Base_2.pos_X - buff_x - (Bullets_0.Width / 2), map.Base_2.pos_Y - buff_y - (Bullets_0.Height / 2)), Color.White); break;
                    case 1: spriteBatch.Draw(Bullets_1, new Vector2(map.Base_2.pos_X - buff_x - (Bullets_1.Width / 2), map.Base_2.pos_Y - buff_y - (Bullets_1.Height / 2)), Color.White); break;
                    case 2: spriteBatch.Draw(Bullets_2, new Vector2(map.Base_2.pos_X - buff_x - (Bullets_2.Width / 2), map.Base_2.pos_Y - buff_y - (Bullets_2.Height / 2)), Color.White); break;
                    case 3: spriteBatch.Draw(Bullets_3, new Vector2(map.Base_2.pos_X - buff_x - (Bullets_3.Width / 2), map.Base_2.pos_Y - buff_y - (Bullets_3.Height / 2)), Color.White); break;
                }
            for (int i = 0; i < map.Max_Zombies; i++)
                if (map.zombie[i].Body) spriteBatch.Draw(zombie_dead, new Rectangle(map.zombie[i].pos_X - buff_x, map.zombie[i].pos_Y - buff_y, zombie.Width, zombie.Height), null, Color.White, map.zombie[i].Rotate, new Vector2(zombie.Width / 2, zombie.Height / 2), SpriteEffects.None, 0);
            if (!map.Main_Hero.alive) spriteBatch.Draw(hero_dead, new Vector2(map.Main_Hero.pos_X - buff_x - (hero_dead.Width / 2), map.Main_Hero.pos_Y - buff_y - (hero_dead.Height / 2)), Color.White);
            for (int i = 0; i < map.Max_Towers; i++)
            {
                if (map.Towers[i].Is_Alive)
                {
                    if (map.Towers[i].Hurted)
                        spriteBatch.Draw(tower_hurted, new Vector2(map.Towers[i].pos_X - buff_x - (tower_hurted.Width / 2), map.Towers[i].pos_Y - buff_y - (tower_hurted.Height / 2)), Color.White);
                    else spriteBatch.Draw(tower_0, new Vector2(map.Towers[i].pos_X - buff_x - (tower_0.Width / 2), map.Towers[i].pos_Y - buff_y - (tower_0.Height / 2)), Color.White);
                }
            }
            for (int i = 0; i < map.Max_Towers; i++)
            {
                for (int j = 0; j < map.Towers[i].Bullets_Count; j++)
                {
                    if (map.Towers[i].Bullets[j].Is_Alive)
                    {
                        spriteBatch.Draw(bullet, new Rectangle(map.Towers[i].Bullets[j].pos_X - buff_x, map.Towers[i].Bullets[j].pos_Y - buff_y, bullet.Width, bullet.Height), null, Color.White, map.Towers[i].Bullets[j].Rotation, new Vector2(bullet.Width / 2, bullet.Height / 2), SpriteEffects.None, 0);
                    }
                }
            }
            for (int i = 0; i < map.Max_Towers; i++)
            {
                if (map.Towers[i].Is_Alive)
                {
                    spriteBatch.Draw(tower_1, new Rectangle(map.Towers[i].pos_X - buff_x, map.Towers[i].pos_Y - buff_y, tower_1.Width, tower_1.Height), null, Color.White, map.Towers[i].Rotate, new Vector2(tower_1.Width / 2, tower_1.Height / 2), SpriteEffects.None, 0);
                }
            }
                for (int i = 0; i < map.Portals; i++)
                spriteBatch.Draw(portal, new Vector2(map.portals_X[i] - (portal.Width / 2) - buff_x, map.portals_Y[i] - (portal.Height / 2) - buff_y), Color.White);
            for (int i = 0; i < map.Main_Hero.max_Rockets; i++)
            {
                if (map.Main_Hero.Rockets[i].alive)
                    spriteBatch.Draw(rocket, new Rectangle(map.Main_Hero.Rockets[i].pos_X - buff_x, map.Main_Hero.Rockets[i].pos_Y - buff_y, rocket.Width, rocket.Height), null, Color.White, map.Main_Hero.Rockets[i].Rotation, new Vector2(rocket.Width / 2, rocket.Height / 2), SpriteEffects.None, 0);
            }
            for (int i = 0; i < map.Main_Hero.Bullets_Count; i++)
                if (map.Main_Hero.bullets[i].Is_Alive) spriteBatch.Draw(bullet, new Rectangle(map.Main_Hero.bullets[i].pos_X - buff_x, map.Main_Hero.bullets[i].pos_Y - buff_y, bullet.Width, bullet.Height), null, Color.White, map.Main_Hero.bullets[i].Rotation, new Vector2(bullet.Width / 2, bullet.Height / 2), SpriteEffects.None, 0);
            if (map.Main_Hero.alive)
            {
                switch (map.Main_Hero.Selected_Wearpon)
                {
                    case 0:
                        {
                            if (map.Main_Hero.Walking) spriteBatch.Draw(hero_move_0, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), frame_0, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                            else spriteBatch.Draw(hero_0, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), null, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                        }
                        break;
                    case 1:
                        {
                            if (map.Main_Hero.Walking) spriteBatch.Draw(hero_move_1, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), frame_0, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                            spriteBatch.Draw(hero_1, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), null, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                        }
                        break;
                    case 2:
                        {
                            if (map.Main_Hero.Walking) spriteBatch.Draw(hero_move_2, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), frame_0, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                            else spriteBatch.Draw(hero_2, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), null, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                        }
                        break;
                    case 3:
                        {
                            if (map.Main_Hero.Walking) spriteBatch.Draw(hero_move_3, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), frame_0, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                            else spriteBatch.Draw(hero_3, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), null, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
                        }
                        break;
                }
                if (map.Main_Hero.Hurt) spriteBatch.Draw(hero_hurted, new Rectangle(map.Main_Hero.pos_X - buff_x, map.Main_Hero.pos_Y - buff_y, hero_0.Width, hero_0.Height), null, Color.White, map.Main_Hero.rotate, new Vector2(hero_0.Width / 2, hero_0.Height / 2), SpriteEffects.None, 0);
            }
            for (int i = 0; i < map.Max_Zombies; i++)
            {
                if (map.zombie[i].Alive)
                {
                    if (map.zombie[i].Hurtet) spriteBatch.Draw(zombie_hurt, new Rectangle(map.zombie[i].pos_X - buff_x, map.zombie[i].pos_Y - buff_y, zombie.Width, zombie.Height), null, Color.White, map.zombie[i].Rotate, new Vector2(zombie.Width / 2, zombie.Height / 2), SpriteEffects.None, 0);
                    else
                    {
                        if (map.zombie[i].Hitting) spriteBatch.Draw(zombie_hitting, new Rectangle(map.zombie[i].pos_X - buff_x, map.zombie[i].pos_Y - buff_y, zombie.Width, zombie.Height), null, Color.White, map.zombie[i].Rotate, new Vector2(zombie.Width / 2, zombie.Height / 2), SpriteEffects.None, 0);
                        else if (map.zombie[i].attaking) spriteBatch.Draw(zombie, new Rectangle(map.zombie[i].pos_X - buff_x, map.zombie[i].pos_Y - buff_y, zombie.Width, zombie.Height), null, Color.White, map.zombie[i].Rotate, new Vector2(zombie.Width / 2, zombie.Height / 2), SpriteEffects.None, 0);
                        else spriteBatch.Draw(zombie_staying, new Rectangle(map.zombie[i].pos_X - buff_x, map.zombie[i].pos_Y - buff_y, zombie.Width, zombie.Height), null, Color.White, map.zombie[i].Rotate, new Vector2(zombie.Width / 2, zombie.Height / 2), SpriteEffects.None, 0);
                    }
                }
            }
            for (int i = 0; i < map.N; i++)
            {
                for (int j = 0; j < map.M; j++)
                {
                    if (Net[i, j] == 1)
                        spriteBatch.Draw(stone, new Vector2((j * stone.Width) - buff_x, (i * stone.Height) - buff_y), Color.White);
                }
            }
            for (int i = 0; i < Max_Rockets; i++)
            {
                if (map.Main_Hero.Rockets[i].Explosion)
                {
                    map.Main_Hero.Rockets[i].Explosion = false;
                    animation_1[i] = true;
                }
                if (animation_1[i])
                    spriteBatch.Draw(explotion, new Vector2(map.Main_Hero.Rockets[i].pos_X - buff_x - (frame_height_1 / 2), map.Main_Hero.Rockets[i].pos_Y - buff_y - (frame_width_1 / 2)), frame_1[i], Color.White);
            }
            //spriteBatch.Draw(back_0, new Vector2(0, 0), Color.White);
            for (int i = 0; i < Screen_Width / Top.Width; i++)
                spriteBatch.Draw(Top, new Vector2(i * Top.Width, 0), Color.White);
            for (int i = 0; i < Screen_Height / Left.Height; i++)
                spriteBatch.Draw(Left, new Vector2(0, i * Left.Height), Color.White);
            for (int i = 0; i < Screen_Height / Right.Height; i++)
                spriteBatch.Draw(Right, new Vector2(Screen_Width - Right.Width, i * Right.Height), Color.White);
            for (int i = 0; i < Screen_Width / Down.Width; i++)
                spriteBatch.Draw(Down, new Vector2(i * Down.Width, Screen_Height - Down.Height), Color.White);
            spriteBatch.Draw(Corner_1, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(Corner_2, new Vector2(Screen_Width - Corner_2.Width, 0), Color.White);
            spriteBatch.Draw(Corner_3, new Vector2(0, Screen_Height - Corner_3.Height), Color.White);
            spriteBatch.Draw(Corner_4, new Vector2(Screen_Width - Corner_4.Width, Screen_Height - Corner_4.Height), Color.White);
            if (map.Main_Hero.Reloading) spriteBatch.DrawString(bullets_count, "Re.", new Vector2(Screen_Width - 230, 200), Color.Black);
            else spriteBatch.DrawString(bullets_count, "x" + Convert.ToString(map.Main_Hero.Current_Bullets_Count), new Vector2(Screen_Width - 230, 200), Color.Black);
            switch (map.Main_Hero.Selected_Wearpon)
            {
                case 0:
                    spriteBatch.Draw(Bullets_0_0, new Vector2(Screen_Width - 430, 150), Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(Bullets_1_0, new Vector2(Screen_Width - 430, 150), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(Bullets_2_0, new Vector2(Screen_Width - 430, 150), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(Bullets_3_0, new Vector2(Screen_Width - 430, 150), Color.White);
                    break;
            }
            spriteBatch.Draw(Bullets_0, new Vector2(Screen_Width - 450, 25), Color.White);
            spriteBatch.DrawString(bullets_new, Convert.ToString(map.Main_Hero.Case_0), new Vector2(Screen_Width - 400, 25), Color.Black);
            spriteBatch.Draw(Bullets_1, new Vector2(Screen_Width - 350, 25), Color.White);
            spriteBatch.DrawString(bullets_new, Convert.ToString(map.Main_Hero.Case_1), new Vector2(Screen_Width - 300, 25), Color.Black);
            spriteBatch.Draw(Bullets_2, new Vector2(Screen_Width - 250, 25), Color.White);
            spriteBatch.DrawString(bullets_new, Convert.ToString(map.Main_Hero.Case_2), new Vector2(Screen_Width - 200, 25), Color.Black);
            spriteBatch.Draw(Bullets_3, new Vector2(Screen_Width - 150, 25), Color.White);
            spriteBatch.DrawString(bullets_new, Convert.ToString(map.Main_Hero.Case_3), new Vector2(Screen_Width - 100, 25), Color.Black);
            if (Screen_Height >= 900)
            {
                spriteBatch.Draw(panel, new Vector2(Screen_Width - 500, Screen_Height - 475), Color.White);
                switch (map.Main_Hero.Selected_Wearpon)
                {
                    case 0:
                        spriteBatch.Draw(wearpon_0, new Vector2(Screen_Width - 500, Screen_Height - 475), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(wearpon_1, new Vector2(Screen_Width - 500, Screen_Height - 475), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(wearpon_2, new Vector2(Screen_Width - 500, Screen_Height - 475), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(wearpon_3, new Vector2(Screen_Width - 500, Screen_Height - 475), Color.White);
                        break;
                }
            }
            else if (Screen_Height >= 600)
                switch (map.Main_Hero.Selected_Wearpon)
                {
                    case 0:
                        spriteBatch.Draw(wearpon_0, new Vector2(Screen_Width - 500, Screen_Height - 425), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(wearpon_1, new Vector2(Screen_Width - 500, Screen_Height - 425), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(wearpon_2, new Vector2(Screen_Width - 500, Screen_Height - 425), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(wearpon_3, new Vector2(Screen_Width - 500, Screen_Height - 425), Color.White);
                        break;
                }
            spriteBatch.Draw(empty_panel, new Vector2(Screen_Width - 450, Screen_Height - 75), Color.White);
            if (map.Main_Hero.HitPoints > 0)
            spriteBatch.Draw(panel_hp, new Rectangle(Screen_Width - 450 + 39, Screen_Height - 75 + 11, (panel_hp.Width * map.Main_Hero.HitPoints) / map.Main_Hero.Max_HitPoints, panel_hp.Height), new Rectangle(0, 0, (panel_hp.Width * map.Main_Hero.HitPoints) / map.Main_Hero.Max_HitPoints, panel_hp.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            for (int i = 0; i < map.Main_Hero.Rank_Max; i++)
            {
                if (map.Main_Hero.Ranks[i] > 0) spriteBatch.Draw(stars[map.Main_Hero.Ranks[i]], new Vector2(100 * i, Screen_Height - 100), Color.White);
            }
            spriteBatch.Draw(target, new Vector2(mouse.X - (target.Width / 2), mouse.Y - (target.Height / 2)), Color.White);
            //spriteBatch.DrawString(bullets_new, check, new Vector2(100, 100), Color.White);
            if (IsWin && Timer_Win > Max_Timer / 10)
            {
                int width_0, height_0, center_X, center_Y;
                center_X = ((Screen_Width - Left.Width - Right.Width) / 2) + Left.Width;
                center_Y = ((Screen_Height - Top.Height - Down.Height) / 2) + Top.Height;
                width_0 = (Screen_Width - Left.Width - Right.Width) / 2;
                height_0 = Convert.ToInt32(Math.Round(Victory.Height * ((double)width_0 / (double)Victory.Width)));
                spriteBatch.Draw(Victory, new Rectangle(center_X - (width_0 / 2), center_Y - (height_0 / 2), width_0, height_0), new Rectangle(0, 0, Victory.Width, Victory.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            if (IsDefeat && Timer_Defeat > Max_Timer / 10)
            {
                int width_0, height_0, center_X, center_Y;
                center_X = ((Screen_Width - Left.Width - Right.Width) / 2) + Left.Width;
                center_Y = ((Screen_Height - Top.Height - Down.Height) / 2) + Top.Height;
                width_0 = (Screen_Width - Left.Width - Right.Width) / 2;
                height_0 = Convert.ToInt32(Math.Round(Defeat.Height * ((double)width_0 / (double)Defeat.Width)));
                spriteBatch.Draw(Defeat, new Rectangle(center_X - (width_0 / 2), center_Y - (height_0 / 2), width_0, height_0), new Rectangle(0, 0, Defeat.Width, Defeat.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            spriteBatch.End();
            if (map.Main_Hero.Empty)
            {
                map.Main_Hero.Empty = false;
                switch (map.Main_Hero.Selected_Wearpon)
                {
                    case 0:
                        empty_1.Play();
                        break;
                    case 1:
                        empty_2.Play();
                        break;
                    case 2:
                        empty_3.Play();
                        break;
                }
            }
            if (map.Main_Hero.Shot)
            {
                map.Main_Hero.Shot = false;
                switch (map.Main_Hero.Selected_Wearpon)
                {
                    case 0:
                        shot_1.Play();
                        break;
                    case 1:
                        shot_2.Play();
                        break;
                    case 2:
                        shot_3.Play();
                        break;
                }
            }
            if (map.Main_Hero.Reload)
            {
                map.Main_Hero.Reload = false;
                switch (map.Main_Hero.Selected_Wearpon)
                {
                    case 0:
                        reload_1.Play();
                        break;
                    case 1:
                        reload_2.Play();
                        break;
                    case 2:
                        reload_3.Play();
                        break;
                }
            }
            if (map.Main_Hero.Rocket_sound)
            {
                rocket_fly.Play();
                map.Main_Hero.Rocket_sound = false;
            }
            if (map.Main_Hero.Explotion_sound)
            {
                explotion_1.Play();
                map.Main_Hero.Explotion_sound = false;
            }
            if (map.Base_1.Find_Item)
            {
                map.Base_1.Find_Item = false;
                find_ammo.Play();
            }
            if (map.Base_2.Find_Item)
            {
                map.Base_2.Find_Item = false;
                find_ammo.Play();
            }
            if (map.Sound_Bullet_To_Wall)
            {
                //bullet_to_wall.Play();
                map.Sound_Bullet_To_Wall = false;
            }
            if (map.Sound_Bullet_To_Tower)
            {
                bullet_to_tower.Play();
                map.Sound_Bullet_To_Tower = false;
            }
            for (int i = 0; i < map.Max_Towers; i++)
            if (map.Towers[i].Shot_Noise)
            {
                    shot_tower.Play();
                    map.Towers[i].Shot_Noise = false;
            }
            base.Draw(gameTime);
        }
    }
}
