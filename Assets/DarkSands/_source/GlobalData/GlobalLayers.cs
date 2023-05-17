using DevourDev.Utility;
using UnityEngine;

namespace DarkSands.Global
{
    [DefaultExecutionOrder(-10000)]
    public sealed class GlobalLayers : Singleton<GlobalLayers>
    {
        [SerializeField] private LayerMask _characters;
        [SerializeField] private LayerMask _interactables;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeSingleton()
        {
            InitializeSingletonBeforeSplashScreen();
        }


        public LayerMask Characters => _characters;
        public LayerMask Interactables => _interactables;
    }
}
