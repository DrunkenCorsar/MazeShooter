using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Base
    {
        int time, time_to_Create, type;
        int item_0, item_1, item_2, item_3, health;
        int X, Y;
        bool ready, find;
        public Base(int x, int y)
        {
            X = x; Y = y;
            Random o = new Random();
            time_to_Create = o.Next(1900, 3600);
            time = 0;
            find = false;
        }

        public void Tick()
        {
            if (!ready)
            {
                time++;
                if (time >= time_to_Create)
                {
                    time = 0; ready = true;
                    Random o = new Random();
                    type = o.Next(0, 4);
                    item_0 = o.Next(5, 16);
                    item_1 = o.Next(2, 7);
                    item_2 = 1;
                    item_3 = 0;
                    switch(type)
                    {
                        case 0:
                            item_0 += o.Next(20, 50);
                            break;
                        case 1:
                            item_1 += o.Next(10, 30);
                            break;
                        case 2:
                            item_2 += o.Next(1, 4);
                            break;
                        case 3:
                            item_3 += 1;
                            break;
                    }
                    health = o.Next(5, 30);
                }
            }
        }

        public void Take_Items()
        {
            ready = false;
            Random o = new Random();
            time_to_Create = o.Next(3100, 5800);
            time = 0;
            find = true;
        }

        public bool Ready
        {
            get { return ready; }
        }

        public int Item_0
        {
            get { return item_0; }
        }

        public int Type
        {
            get { return type; }
        }

        public int Item_1
        {
            get { return item_1; }
        }

        public int Item_2
        {
            get { return item_2; }
        }

        public int Item_3
        {
            get { return item_3; }
        }

        public bool Find_Item
        {
            get { return find; }
            set { find = value; }
        }

        public int Time_Create
        {
            set { time_to_Create = value; }
            get { return time_to_Create; }
        }

        public int pos_X
        {
            get { return X; }
        }

        public int pos_Y
        {
            get { return Y; }
        }

        public int Health
        {
            get { return health; }
        }
    }
}
