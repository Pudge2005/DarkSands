using System;

namespace DevourDev.StateEventing
{
    public delegate void StateChangedArgs<T>(T prevState, T curState);

    public sealed class StateEventor<T> : IStateEventor<T>
         where T : IEquatable<T>
    {
        private T _state;


        public T State => _state;


        public event StateChangedArgs<T> OnStateChanged;


        public void ChangeState(T newState)
        {
            if (_state.Equals(newState))
                return;

            var prev = _state;
            _state = newState;
            OnStateChanged?.Invoke(prev, newState);
        }
    }
}
