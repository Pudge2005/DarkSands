using DevourDev.Stats;

namespace DevourDev.Unity.Stats
{
    public static class StatsInternalsExposer
    {
        public static void SetDynamicStatData(DynamicStatData dynamicStatData, float max, float cur, bool raiseEvent)
        {
            dynamicStatData.SetInternal(max, cur, raiseEvent);
        }
    }
    public class DynamicStatData : IClampedStatData<StatSo, float>
    {
        private const float _min = 0f;

        private readonly StatSo _stat;
        private float _max;
        private float _cur;


        public DynamicStatData(StatSo stat, float max, float initialCur)
        {
            _stat = stat;
            _max = max;
            _cur = initialCur;
        }


        public StatSo Stat => _stat;
        public float Max => _max;
        public float Min => _min;

        public float Value => _cur;


        /// <summary>
        /// sender, delta
        /// </summary>
        public event System.Action<DynamicStatData, float> OnMaxBoundChanged;

        /// <summary>
        /// sender, rawDelta, safeDelta
        /// </summary>
        public event System.Action<DynamicStatData, float, float> OnCurrentValueChanged;

        /// <summary>
        /// sender, rawDelta, safeDelta
        /// </summary>
        public event System.Action<DynamicStatData, float, float> OnMinReached;



        internal void SetInternal(float max, float cur, bool raiseEvent)
        {
            float maxDelta = max - _max;
            float curDelta = cur - _cur;

            _max = max;
            _cur = cur;

            if (raiseEvent)
            {
                OnMaxBoundChanged?.Invoke(this, maxDelta);
                OnCurrentValueChanged?.Invoke(this, curDelta, curDelta);
            }
        }

        public void SetMaxBound(float v)
        {
            float maxDelta = v - _max;
            _max = v;
            OnMaxBoundChanged?.Invoke(this, maxDelta);
        }

        /// <returns>safe delta</returns>
        public float Set(float v)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
#endif
            float rawDelta = v - _cur;

            bool reachedMin = false;
            bool reachedMax = false;

            if (v >= _max)
            {
                v = _max;
                reachedMax = true;
            }
            else if (v == _min)
            {
                reachedMin = true;
            }

            float safeDelta = v - _cur;
            ChangeCurrent(v, rawDelta, safeDelta, reachedMin, reachedMax);
            return safeDelta;
        }


        public bool CanAdd(float v, out float result)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
#endif

            result = _cur + v;
            return result <= _max;
        }

        public bool TryAdd(float v)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
#endif
            float result = _cur + v;

            if (result > _max)
                return false;

            ChangeCurrent(result, v, v, false, result == _max);
            return true;
        }


        /// <returns>added delta</returns>
        public float AddPossible(float desiredAdding)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(desiredAdding);
#endif
            float result = _cur + desiredAdding;
            bool reachedMax = ClampMax(ref result);
            float added = reachedMax ? _max - _cur : desiredAdding;
            ChangeCurrent(result, desiredAdding, added, false, reachedMax);
            return added;
        }

        /// <returns>added delta</returns>
        public float AddPossible(float desiredAdding, float maxOverride)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(desiredAdding);
            ThrowIfNanOrNegative(maxOverride);
#endif
            float result = _cur + desiredAdding;

            if (maxOverride > _max)
                maxOverride = _max;

            bool reachedMax = ClampMax(ref result, maxOverride) && maxOverride == _max;
            float added = reachedMax ? _max - _cur : desiredAdding;
            ChangeCurrent(result, desiredAdding, added, false, reachedMax);
            return added;
        }

        public bool CanRemove(float v, out float result)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
#endif
            result = _cur - v;
            return result >= _min;
        }

        public bool CanRemove(float v, float minOverride, out float result)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
            ThrowIfNanOrNegative(minOverride);
#endif
            result = _cur - v;
            return result >= minOverride;
        }

        public bool TryRemove(float v)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
#endif

            float result = _cur - v;

            if (result < _min)
                return false;

            ChangeCurrent(result, -v, -v, result == _min, false);
            return true;
        }

        public bool TryRemove(float v, float minOverride)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(v);
            ThrowIfNanOrNegative(minOverride);
#endif

            float result = _cur - v;

            if (result < minOverride)
                return false;

            ChangeCurrent(result, -v, -v, result == _min, false);
            return true;
        }

        /// <returns>safe delta</returns>
        public float RemovePossible(float desiredRemoving)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(desiredRemoving);
#endif
            float result = _cur - desiredRemoving;
            bool reachedMin = ClampMin(ref result);
            float removed = reachedMin ? -_cur : -desiredRemoving;
            ChangeCurrent(result, -desiredRemoving, removed, reachedMin, false);
            return removed;
        }

        /// <returns>safe delta</returns>
        public float RemovePossible(float desiredRemoving, float minOverride)
        {
#if UNITY_EDITOR
            ThrowIfNanOrNegative(desiredRemoving);
            ThrowIfNanOrNegative(minOverride);
#endif
            float result = _cur - desiredRemoving;

            if (minOverride < _min)
                minOverride = _min;

            bool reachedMin = ClampMin(ref result, minOverride) && minOverride == _min;
            float removed = reachedMin ? -_cur : -desiredRemoving;
            ChangeCurrent(result, -desiredRemoving, removed, reachedMin, false);
            return removed;
        }


        private void ChangeCurrent(float newCurrent, float rawDelta, float safeDelta,
            bool raiseReachedMinEvent, bool raiseReachedMaxEvent)
        {
            _cur = newCurrent;
            OnCurrentValueChanged?.Invoke(this, rawDelta, safeDelta);

            if (raiseReachedMinEvent)
                OnMinReached?.Invoke(this, rawDelta, safeDelta);
        }

        private bool ClampMin(ref float v, float min)
        {
            if (v <= min)
            {
                v = min;
                return true;
            }

            return false;
        }
        private bool ClampMin(ref float v)
        {
            if (v <= _min)
            {
                v = _min;
                return true;
            }

            return false;
        }

        private bool ClampMax(ref float v)
        {
            if (v >= _max)
            {
                v = _max;
                return true;
            }

            return false;
        }

        private bool ClampMax(ref float v, float max)
        {
            if (v >= max)
            {
                v = max;
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void ThrowIfNanOrNegative(float v)
        {
            if (float.IsNaN(v) || float.IsNegative(v))
                throw new System.ArgumentException($"{nameof(v)} is {v}");
        }
#endif
    }
}
