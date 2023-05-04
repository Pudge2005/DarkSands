using System;
using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    [DefaultExecutionOrder(ExecutionOrder)]
    public sealed class TwoDThreeDManager : MonoBehaviour
    {
        public const int ExecutionOrder = 1000;

        private const int _capacity = 10240;

        private static TwoDThreeDManager _instance;
        private static int _size;
        private static TwoDThreeDComponent[] _items;
        private static Transform _cameraTr;


        internal static TwoDThreeDManager Instance => _instance;

        internal static Quaternion CameraRotation { get; private set; }


        private static Transform CameraTransform
        {
            get
            {
                if (_cameraTr == null)
                {
                    var cam = Camera.main;

                    if (cam == null)
                        return null;

                    _cameraTr = cam.transform;
                }

                return _cameraTr;
            }
        }


        internal static event System.Action<TwoDThreeDManager> SingletonInited;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInit()
        {
            _instance = null;
            _size = 0;
            _items = new TwoDThreeDComponent[_capacity];
            _cameraTr = null;

            CameraRotation = Quaternion.identity;
            SingletonInited = null;
        }

        private static void ClearArray()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                _instance.DisposeSingleton();
            }

            _instance = this;
            SingletonInited?.Invoke(this);
        }


        private void Update()
        {
            var camTr = CameraTransform;

            if (camTr == null)
                return;

            var rot = camTr.rotation;


            if (CameraRotation == rot)
                return;


            CameraRotation = rot;

            var len = _size;
            var span = _items.AsSpan(0, len);

            for (int i = len - 1; i >= 0; i--)
            {
                var cmp = span[i];

                if (cmp == null)
                {
                    span[i] = span[--len];
                    span[len + 1] = null;

                    continue;
                }

                cmp.OnCameraRotChanged(rot);
            }

            _size = len;
        }

        internal void Register(TwoDThreeDComponent component)
        {
            component._index = _size;
            _items[_size++] = component;
        }

        internal void Unregister(TwoDThreeDComponent component)
        {
            if (component._index < 0)
            {
                Debug.LogError("unexpected behaviour");
                return;
            }

            UnregisterInternal(component._index);
            component._index = -1;
        }

        private void UnregisterInternal(int index)
        {
            _items[index] = _items[--_size];
            _items[_size + 1] = null;
        }

        private void DisposeSingleton()
        {
            Destroy(gameObject);
            ClearArray();
            _cameraTr = null;
            CameraRotation = Quaternion.identity;
        }


    }
}
