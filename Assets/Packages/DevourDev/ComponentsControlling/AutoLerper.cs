using System.Collections;
using UnityEngine;

namespace DevourDev.Unity.Utils
{
    public delegate T Lerp<T>(T from, T to, float t);
    public delegate void SetValue<T>(T value);

    public sealed class AutoLerper2<T>
    {
        private readonly Lerp<T> _lerp;
        private readonly SetValue<T> _setValue;

        private bool _lerping;
        private float _timeTotal;
        private float _timePassed;
        private T _from;
        private T _to;


        public AutoLerper2(Lerp<T> lerp, SetValue<T> setValue)
        {
            _lerp = lerp;
            _setValue = setValue;
        }


        public void StartLerping(T from, T to, float time)
        {
            if (float.IsSubnormal(time) || time <= Vector3.kEpsilon)
            {
                _lerping = false;
                _setValue(to);
            }

            _lerping = true;
            _timeTotal = time;
            _timePassed = 0f;
            _from = from;
            _to = to;
        }


        public void Evaluate(float deltaTime)
        {
#if UNITY_EDITOR
            if (deltaTime < 0)
                throw new System.Exception("delta is negative: " + deltaTime);
#endif

            if (!_lerping)
                return;

            _timePassed += deltaTime;

            if (_timePassed >= _timeTotal)
                goto END;

            float t = _timePassed / _timeTotal;
            var val = _lerp(_from, _to, t);
            _setValue(val);
            return;

        END:
            _setValue(_to);
            _lerping = false;
        }
    }


    public sealed class AutoLerper<T>
    {
        private readonly MonoBehaviour _coroutinsParent;
        private readonly Lerp<T> _lerp;
        private readonly SetValue<T> _setValue;

        private Coroutine _activeCoroutine;


        public AutoLerper(MonoBehaviour coroutinsParent, Lerp<T> lerp, SetValue<T> setValue)
        {
            _coroutinsParent = coroutinsParent;
            _lerp = lerp;
            _setValue = setValue;
        }


        public void StartLerping(T from, T to, float time)
        {
            if (_activeCoroutine != null)
                _coroutinsParent.StopCoroutine(_activeCoroutine);

            if (time > 0)
            {
                _activeCoroutine = _coroutinsParent.StartCoroutine(GetLerpingEnumerator(from, to, time));
                return;
            }

            _setValue(to);
        }


        private IEnumerator GetLerpingEnumerator(T from, T to, float time)
        {
            if (float.IsSubnormal(time))
                goto END;

            for (float timeLeft = time; ;)
            {
                if ((timeLeft -= Time.deltaTime) <= Vector3.kEpsilon)
                    goto END;

                float t = 1f - timeLeft / time;
                var val = _lerp(from, to, t);
                _setValue(val);
                yield return null;
            }

        END:
            _setValue(to);
            _activeCoroutine = null;
        }
    }
}
