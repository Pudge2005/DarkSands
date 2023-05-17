using System;
using System.IO;
using UnityEngine;

namespace DevourDev.Utility
{
    public static class NonAllocHelpers
    {
        public const int BuffersSize = 1024;

        private static readonly Collider[] _collidersBuffer = new Collider[BuffersSize];
        private static readonly RaycastHit[] _hitsBuffer = new RaycastHit[BuffersSize];

        private static int _collidersCount;
        private static int _hitsCount;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInit()
        {
            ClearCollidersBuffer();
            ClearHitsBuffer();
        }


        public static ReadOnlySpan<Collider> OverlapSphere(Vector3 position, float radius,
            int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            var pscene = Physics.defaultPhysicsScene;
            int count = pscene.OverlapSphere(position, radius, _collidersBuffer, layerMask, queryTriggerInteraction);

            SetCollidersCount(count);

            return _collidersBuffer.AsSpan(0, count);
        }

        public static ReadOnlySpan<RaycastHit> RayCastAll(Vector3 origin, Vector3 direction,
            float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            var pscene = Physics.defaultPhysicsScene;
            int count = pscene.Raycast(origin, direction, _hitsBuffer, maxDistance, layerMask, queryTriggerInteraction);

            SetHitsCount(count);
            return _hitsBuffer.AsSpan(0, count);
        }


        private static void SetCollidersCount(int count)
        {
            if (_collidersCount > count)
            {
                Array.Clear(_collidersBuffer, count, _collidersCount - count);
            }

            _collidersCount = count;
        }

        private static void SetHitsCount(int count)
        {
            if (_hitsCount > count)
            {
                Array.Clear(_hitsBuffer, count, _hitsCount - count);
            }

            _hitsCount = count;
        }

        private static void ClearHitsBuffer()
        {
            Array.Clear(_hitsBuffer, 0, _hitsCount);
            _hitsCount = 0;
        }

        private static void ClearCollidersBuffer()
        {
            Array.Clear(_collidersBuffer, 0, _collidersCount);
            _collidersCount = 0;
        }
    }

    public static class GeometryHelpers
    {
        public static Quaternion GetRotation(Quaternion source, bool ignoreX, bool ignoreY, bool ignoreZ)
        {
            Vector3 eulers = source.eulerAngles;

            if (ignoreX)
                eulers.x = 0;

            if (ignoreY)
                eulers.y = 0;

            if (ignoreZ)
                eulers.z = 0;

            return Quaternion.Euler(eulers);
        }

        public static Vector3 XyzToXzy(Vector3 v)
        {
            (v.y, v.z) = (v.z, v.y);
            return v;
        }

        public static Vector3 X0Y(Vector2 v)
        {
            return new(v.x, 0, v.y);
        }
    }
}
