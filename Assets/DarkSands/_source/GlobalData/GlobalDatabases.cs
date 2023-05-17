using DevourDev.Utility;
using UnityEngine;

namespace DarkSands.Global
{
    [DefaultExecutionOrder(-10000)]
    public sealed class GlobalDatabases : Singleton<GlobalDatabases>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeSingleton()
        {
            InitializeSingletonBeforeSplashScreen();
        }
    }
}
