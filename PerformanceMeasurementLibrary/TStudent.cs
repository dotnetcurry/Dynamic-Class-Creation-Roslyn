using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceMeasurementLibrary
{
    public static class TStudent
    {
        public static double Student(double t, double df)
        {
            // for large int df or double df

            // adapted from ACM algorithm 395
            // returns 2-tail probability

            double n = df; // to sync with ACM parameter name
            double a, b, y;

            t = t * t;
            y = t / n;
            b = y + 1.0;
            if (y > 1.0E-6) y = Math.Log(b);
            a = n - 0.5;
            b = 48.0 * a * a;
            y = a * y;

            y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
              (0.8 * y * y + 100.0 + b) +
                y + 3.0) / b + 1.0) * Math.Sqrt(y);
            return 2.0 * Gauss(-y);
        } // Student (double df)

        public static double Student(double t, int df)
        {
            // adapted from ACM algorithm 395
            // for small int df
            int n = df; // to sync with ACM parameter name
            double a, b, y, z;

            z = 1.0;
            t = t * t;
            y = t / n;
            b = 1.0 + y;

            if (n >= 20 && t < n || n > 200) // large df
            {
                double x = 1.0 * n; // make df a double
                return Student(t, x); // double version
            }

            if (n < 20 && t < 4.0)
            {
                a = Math.Sqrt(y);
                y = Math.Sqrt(y);
                if (n == 1)
                    a = 0.0;
            }
            else
            {
                a = Math.Sqrt(b);
                y = a * n;
                for (int j = 2; a != z; j += 2)
                {
                    z = a;
                    y = y * (j - 1) / (b * j);
                    a = a + y / (n + j);
                }
                n = n + 2;
                z = y = 0.0;
                a = -a;
            }

            int sanityCt = 0;
            while (true && sanityCt < 10000)
            {
                ++sanityCt;
                n = n - 2;
                if (n > 1)
                {
                    a = (n - 1) / (b * n) * a + y;
                    continue;
                }

                if (n == 0)
                    a = a / Math.Sqrt(b);
                else // n == 1
                    a = (Math.Atan(y) + a / b) * 0.63661977236; // 2/Pi

                return z - a;
            }

            return -1.0; // error
        } // Student (int df)

        public static double Gauss(double z)
        {
            // input = z-value (-inf to +inf)
            // output = p under Normal curve from -inf to z
            // e.g., if z = 0.0, function returns 0.5000
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called 'z' in 209
            double w; // 209 scratch variable

            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {
                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                      + 0.006549791214) * y - 0.010557625006) * y
                      + 0.011630447319) * y - 0.009279453341) * y
                      + 0.005353579108) * y - 0.002141268741) * y
                      + 0.000535310849) * y + 0.999936657524;
                }
            }

            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        } // Gauss()

    }
}
