using System.IO;

namespace DevourDev.Utility
{
    [System.Serializable]
    public struct MinMaxDouble
    {
        public double Min;
        public double Max;


        public MinMaxDouble(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }
}
