using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceMeasurementLibrary
{
    public static class Statistics
    {
        public static double GetMean(IEnumerable<double> values)
        {
            return values.Average();
        }
        public static double GetMean(IEnumerable<double> values, int k)
        {
            var meanValues = values.TakeLast(k);
            return meanValues.Average();
        }
        public static double GetStandardDeviation(IEnumerable<double> valuesEn)
        {
            var values = valuesEn.ToArray();
            double avg = GetMean(values);
            double sum = values.Sum(v => (v - avg) * (v - avg));
            double denominator = values.Count() - 1;
            return denominator > 0.0 ? Math.Sqrt(sum / denominator) : -1;
        }
    }
}
