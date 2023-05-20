using DevourDev.Utility;
using UnityEngine;

namespace DevourDev.Unity.Ai.Core
{
    public abstract class StateSo : SoDatabaseElement
    {
        public StateBehaviourBase RentBehaviour()
        {
            var beh = HandleRentBehaviour();
            return beh;
        }

        public void ReturnBehaviour(StateBehaviourBase rentedBehaviour)
        {
            HandleReturnBehaviour(rentedBehaviour);
        }


        protected abstract StateBehaviourBase HandleRentBehaviour();
        protected abstract void HandleReturnBehaviour(StateBehaviourBase rentedBehaviour);
    }

    public abstract class StateSo<TState, TBehaviour, TContext, TStateMachine> : StateSo
        where TState : StateSo<TState, TBehaviour, TContext, TStateMachine>
        where TBehaviour : StateBehaviour<TState, TBehaviour, TContext, TStateMachine>
        where TStateMachine : StateMachineBase<TContext>
    {
        private UnityGameObjectsAutoPool<TBehaviour> _behsPool;
    }
}
