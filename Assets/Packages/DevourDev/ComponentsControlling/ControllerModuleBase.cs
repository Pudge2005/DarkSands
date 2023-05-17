using System.Collections;
using UnityEngine;

namespace DevourDev.Unity.Utils
{
    public abstract class ControllerModuleBase<T>
    {
        private readonly MonoBehaviour _coroutinsParent;
        private Coroutine _activeCoroutine;


        protected ControllerModuleBase(MonoBehaviour coroutinsParent)
        {
            _coroutinsParent = coroutinsParent;
        }


        public void DoLerp(T from, T to, float time)
        {
            if (_activeCoroutine != null)
                _coroutinsParent.StopCoroutine(_activeCoroutine);

            _activeCoroutine = _coroutinsParent.StartCoroutine(GetLerpingEnumerator(from, to, time));
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
                var val = Lerp(from, to, t);
                SetValue(val);
                yield return null;
            }

        END:
            SetValue(to);
            _activeCoroutine = null;
        }


        protected abstract T Lerp(T from, T to, float t);

        protected abstract void SetValue(T value);
    }
}
