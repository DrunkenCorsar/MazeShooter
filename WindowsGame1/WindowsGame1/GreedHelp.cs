using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class GreedHelp
    {
        const double range = 30;
        int[,] a; int n, m, step;
        public GreedHelp(int[,] Arr, int N, int M, int Step)
        {
            a = Arr; n = N; m = M; step = Step;
        }

        public void Find_Place(double X1, double Y1, out double T_x, out double T_y, out bool walk)
        {
            walk = false;
            double x2 = X1, y2 = Y1;
            for (int i = 0; i < 500; i++)
            {
                Random o = new Random();
                int n1 = o.Next(1, n - 1), m1 = o.Next(1, m - 1);
                if (a[n1, m1] == 0 && this.IsPerfectlyClear(X1, Y1, (m1 * step) + 50, (n1 * step) + 50))
                {
                    walk = true;
                    x2 = (m1 * step) + 50;
                    y2 = (n1 * step) + 50;
                    break;
                }
            }
            if (!walk)
            {
                for (int i = 1; i < n - 1; i++)
                    for (int j = 1; j < m - 1; j++)
                        if (a[i, j] == 0 && this.IsPerfectlyClear(X1, Y1, (j * step) + 50, (i * step) + 50))
                        {
                            walk = true;
                            x2 = (j * step) + 50;
                            y2 = (i * step) + 50;
                            break;
                        }
            }
            T_x = x2; T_y = y2;
        }

        public bool IsTowerClear(double X1, double Y1, double X2, double Y2)
        {
            if (IsLittleClear(X1 - 100, Y1 - 100, X2, Y2)
                || IsLittleClear(X1 - 100, Y1, X2, Y2)
                || IsLittleClear(X1, Y1 - 100, X2, Y2)
                || IsLittleClear(X1, Y1, X2, Y2)) return true;
            else return false;
        }
        public void Calculate(double X1, double Y1, double X2, double Y2, out double T_x, out double T_y, out bool walk)
        {
            T_x = 0; T_y = 0;
            walk = false;
            if (this.IsPerfectlyClear(X1, Y1, X2, Y2))
            {
                T_x = X2; T_y = Y2;
                walk = true;
            }
            if (!walk)
            {
                double s0 = 0, ax0 = 0, ay0 = 0;
                for (int i0 = 0; i0 < n; i0++)
                    for (int j0 = 0; j0 < m; j0++)
                    {
                        if (a[i0, j0] != 1)
                        {
                            double s, ax, ay;
                            ax = (j0 * step) + (step / 2);
                            ay = (i0 * step) + (step / 2);
                            if (this.IsPerfectlyClear(X1, Y1, ax, ay) && this.IsPerfectlyClear(ax, ay, X2, Y2))
                            {
                                s = Math.Sqrt(((ax - X1) * (ax - X1)) + ((ay - Y1) * (ay - Y1)));
                                s += Math.Sqrt(((X2 - ax) * (X2 - ax)) + ((Y2 - ay) * (Y2 - ay)));
                                if (s < s0 || !walk)
                                {
                                    walk = true;
                                    s0 = s;
                                    ax0 = ax;
                                    ay0 = ay;
                                }
                            }
                        }
                    }
                T_x = ax0;
                T_y = ay0;
            }
        }
        public bool IsClear(double X1, double Y1, double X2, double Y2)
        {
            bool clear = true;
            double X_min = 0, Y_min = 0, X_max = 0, Y_max = 0;
            if (X1 < X2)
            {
                X_min = X1;
                X_max = X2;
                Y_min = Y1;
                Y_max = Y2;
            }
            else
            {
                X_max = X1;
                X_min = X2;
                Y_max = Y1;
                Y_min = Y2;
            }
            double X_c = X_min, Y_c = Y_min;
            int x0 = (Convert.ToInt32(Math.Floor(X_min)) / step) + 1;
            int x1 = Convert.ToInt32(Math.Floor(X_max)) / step;
            int y0 = (Convert.ToInt32(Math.Floor(Y_min)) / step) + 1;
            int y1 = Convert.ToInt32(Math.Floor(Y_max)) / step;
            int xc = 0, yc = 0;
            if ((x0 >= 0 && x0 < m) && (x1 >= 0 && x1 < m) && (x1 - x0 >= 0) && (X1 != X2))
            {
                for (int i = x0; i <= x1; i++)
                {
                    Y_c += ((i * step) - X_c) * ((Y_max - Y_min) / (X_max - X_min));
                    X_c = (i * step);
                    xc = Convert.ToInt32(Math.Floor(X_c)) / step;
                    yc = Convert.ToInt32(Math.Floor(Y_c)) / step;
                    if (a[yc, xc] == 1 || a[yc, xc] == 5) clear = false;
                }
            }
            if (Y1 < Y2)
            {
                X_min = X1;
                X_max = X2;
                Y_min = Y1;
                Y_max = Y2;
            }
            else
            {
                X_max = X1;
                X_min = X2;
                Y_max = Y1;
                Y_min = Y2;
            }
            X_c = X_min; Y_c = Y_min;
            x0 = (Convert.ToInt32(Math.Floor(X_min)) / step) + 1;
            x1 = Convert.ToInt32(Math.Floor(X_max)) / step;
            y0 = (Convert.ToInt32(Math.Floor(Y_min)) / step) + 1;
            y1 = Convert.ToInt32(Math.Floor(Y_max)) / step;
            if ((y0 >= 0 && y0 < n) && (y1 >= 0 && y1 < n) && (y1 - y0 >= 0) && (Y1 != Y2))
            {
                for (int i = y0; i <= y1; i++)
                {
                    X_c += ((i * step) - Y_c) * ((X_max - X_min) / (Y_max - Y_min));
                    Y_c = (i * step);
                    xc = Convert.ToInt32(Math.Floor(X_c)) / step;
                    yc = Convert.ToInt32(Math.Floor(Y_c)) / step;
                    if (a[yc, xc] == 1 || a[yc, xc] == 5) clear = false;
                }
            }
            return clear;
        }

        public bool IsPerfectlyClear(double X1, double Y1, double X2, double Y2)
        {
            bool clear = true;
            double SS = Math.Sqrt(((X2 - X1) * (X2 - X1)) + ((Y2 - Y1) * (Y2 - Y1)));
            double dx = (1 / (SS / (Y2 - Y1))) * range;
            double dy = (1 / (SS / (X2 - X1))) * range;
            X1 -= dx; X2 -= dx;
            Y1 -= dy; Y2 -= dy;
            for (int j = 0; j < 3; j++)
            {
                if (!this.IsClear(X1, Y1, X2, Y2)) clear = false;
                X1 += dx; X2 += dx;
                Y1 += dy; Y2 += dy;
            }
            return clear;
        }

        public bool IsLittleClear(double X1, double Y1, double X2, double Y2)
        {
            bool clear = false;
            double SS = Math.Sqrt(((X2 - X1) * (X2 - X1)) + ((Y2 - Y1) * (Y2 - Y1)));
            double dx = (1 / (SS / (Y2 - Y1))) * range;
            double dy = (1 / (SS / (X2 - X1))) * range;
            X1 -= dx; X2 -= dx;
            Y1 -= dy; Y2 -= dy;
            for (int j = 0; j < 3; j++)
            {
                if (this.IsClear(X1, Y1, X2, Y2)) clear = true;
                X1 += dx; X2 += dx;
                Y1 += dy; Y2 += dy;
            }
            return clear;
        }
    }
}
