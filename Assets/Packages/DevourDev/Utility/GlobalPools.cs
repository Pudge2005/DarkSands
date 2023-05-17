using System;

namespace DevourDev.Utility
{
    public static class GlobalPools
    {
        private static StringBuildersPool _stringBuilders;


        public static StringBuildersPool StringBuildersPool
        {
            get
            {
                _stringBuilders ??= new();

                return _stringBuilders;
            }
        }
    }
}
