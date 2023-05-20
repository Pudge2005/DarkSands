using System;
using DevourDev.Utility;
using UnityEngine;

namespace DevourDev.Unity.Ai.Core
{
    public abstract class StateBehaviour<TState, TBehaviour, TContext, TStateMachine> : StateBehaviourBase
        where TState : StateSo<TState, TBehaviour, TContext, TStateMachine>
        where TBehaviour : StateBehaviour<TState, TBehaviour, TContext, TStateMachine>
        where TStateMachine : StateMachineBase<TContext>
    {
        private TState _activeState;
        private TStateMachine _stateMachine;


        public sealed override StateSo ActiveStateBase => _activeState;
        public sealed override StateMachineBase StateMachineBase => _stateMachine;

        public TState ActiveState => _activeState;
        public TStateMachine StateMachine => _stateMachine;


        protected sealed override void HandleEnterState(StateSo state)
        {
            var castedState = (TState)state;
            _activeState = castedState;
            OnStateEnter(castedState);
        }

        protected sealed override void HandleExitState()
        {
            OnStateExit();
            _activeState = null;
        }


        protected abstract void OnStateEnter(TState state);
        protected abstract void OnStateExit();
    }


    public abstract class StateBehaviourBase : MonoBehaviour, IAutoPoolableItem
    {
        public abstract StateSo ActiveStateBase { get; }
        public abstract StateMachineBase StateMachineBase { get; }


        public void EnterState(StateSo state)
        {
#if UNITY_EDITOR
            if (state == null)
            {
                var exception = new System.ArgumentNullException(nameof(state));
                LogException(exception);
                return;
            }
#endif

#if UNITY_EDITOR
            if (ActiveStateBase != null)
            {
                LogError($"Attempt to enter state, but {nameof(ActiveStateBase)} " +
                    $"is not null ({ActiveStateBase})");

                return;
            }
#endif

            HandleEnterState(state);
        }

        public void ExitState()
        {
#if UNITY_EDITOR
            if (ActiveStateBase == null)
            {
                LogError("Attempt to exit state " +
                    $"but {nameof(ActiveStateBase)} is null.");

                return;
            }
#endif

            HandleExitState();
        }


        protected void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        protected void LogError(string errorMsg)
        {
            UnityEngine.Debug.LogError(errorMsg);
        }


        protected abstract void HandleEnterState(StateSo state);
        protected abstract void HandleExitState();

        public virtual void OnBeforeRent()
        {
        }

        public virtual void OnReturn()
        {
        }

        public virtual void OnPoolableItemDestroy()
        {
        }

        public void ReturnToPool()
        {
            ActiveStateBase.ReturnBehaviour(this);
        }
    }
}
