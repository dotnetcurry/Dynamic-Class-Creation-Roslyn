using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PerformanceMeasurementLibrary
{
    public static class SteadyFunctions
    {
        private static bool AreWeDone(List<double> executionTimes, int k, double CoV)
        {
            if (executionTimes.Count < k)
                return false;
            double summation = 0;
            var mean = Statistics.GetMean(executionTimes, k);
            int lenExecutionTimes = executionTimes.Count;
            for (int i = lenExecutionTimes - k; i < lenExecutionTimes; i++)
                summation += Math.Pow(executionTimes[i] - mean, 2);
            var stdDeviation = Math.Sqrt(summation / k);

            return (stdDeviation / mean) < CoV;
        }

        public static double RunAsSteady(Action command, int maxNumberIterations, int k, double CoV)
        {
            var timer = new Stopwatch();
            List<double> executionTimes = new List<double>();

            for (int i = 0; i < maxNumberIterations + 1; i++)
            {
                timer.Start();
                command();
                timer.Stop();
                var executionTime = timer.ElapsedMilliseconds;
                timer.Reset();

                executionTimes.Add(executionTime);
                if (AreWeDone(executionTimes, k, CoV))
                    break;
            }

            return Statistics.GetMean(executionTimes, k);
        }
    }
}
