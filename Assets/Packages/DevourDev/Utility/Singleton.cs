using UnityEngine;

namespace DevourDev.Utility
{
#if false //add ! before false to see syntax highlighting
    [DefaultExecutionOrder(-10000)]
    public sealed class ExampleSingletonClass : Singleton<ExampleSingletonClass>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeSingleton()
        {
            InitializeSingletonBeforeSplashScreen();
        }


        protected override void Awake()
        {
            base.Awake();

            if (!IsSingleton)
            {
                Debug.Log("I have been aborted");
                return;
            }

            Debug.Log("I'm alive!");
        }


        protected override void OnDestroy()
        {
            Debug.Log("I'm gonna die B)");

            base.OnDestroy();
        }
    }
#endif

    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public delegate void SingletonChangedDelegate(T previous, T current);


        [SerializeField] private bool _dontDestroyOnLoad;
        [SerializeField] private bool _destroyNewEntities;


        private static T _instance;


        //todo:
        //protected virtual bool RaiseSingletonEventFromDerivedClass => false;

        protected bool IsCrossScenes => _dontDestroyOnLoad;
        protected bool IsSingleton => (T)this == _instance;



        /// <summary>
        /// We should always subscribe to <see cref="SingletonChanged"></see>.
        /// If <see cref="Instance"/> is not null, we should manually call our event handler method
        /// with args: previous singleton: null, current singleton: <see cref="Instance"/>.
        /// </summary>
        public static T Instance => _instance;


        /// <summary>
        /// Invokes when singleton initializes or changes.
        /// First arg: previous singleton (usually null),
        /// second arg: current singleton.
        /// On changing SingletonA to SingletonB
        /// 1 event will be risen.
        /// !!!This event lifetime is equal to application's one,
        /// so every subscriber should worry about unsubscribing
        /// (most likely in OnDestroy)!!!
        /// </summary>
        public static event SingletonChangedDelegate SingletonChanged;


        /// <summary>
        /// since RuntimeInitializeOnLoadMethod is not calling
        /// on generic classes, every derived class (defining generic)
        /// should call this method from self static method with
        /// RuntimeInitializeOnLoadMethod attribute and 
        /// RuntimeInitializeLoadType.BeforeSplashScreen option
        /// </summary>
        protected static void InitializeSingletonBeforeSplashScreen()
        {
            SingletonChanged = null;    
        }


        protected virtual void Awake()
        {
            bool singletonExists = _instance != null;

            if (!singletonExists)
            {
                InitSingleton();
                return;
            }

            //singletone exists

            if (_destroyNewEntities)
            {
                Destroy(gameObject);
                return;
            }

            var oldSingleton = _instance;

            InitSingleton();
            Destroy(oldSingleton.gameObject);

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;

            if (_instance == this)
            {
                DisposeSingleton();
            }
        }


        private void InitSingleton()
        {
            var prev = _instance;
            _instance = (T)this;

            SingletonChanged?.Invoke(prev, _instance);
        }


        /// <summary>
        /// rises event of changing singleton to NULL
        /// </summary>
        private void DisposeSingleton()
        {
            if (_instance != (T)this)
                throw new System.Exception("unauthoritative access");

            _instance = null;

            SingletonChanged?.Invoke((T)this, null);
        }
    }
}