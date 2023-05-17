namespace DevourDev.Utility
{
    public static class RandomUtility
    {
        private static readonly System.Random _rngInst = new();


        public static System.Random CreateRng()
        {
            return new(_rngInst.Next());
        }

        public static double RandomDouble(System.Random rng, double min, double max)
        {
            var mantissa = rng.NextDouble();
            return mantissa * (max - min) + min;
        }

        public static float RandomFloat(System.Random rng, float min, float max)
        {
            float mantissa = (float)rng.NextDouble();
            return mantissa * (max - min) + min;
        }
    }
}
