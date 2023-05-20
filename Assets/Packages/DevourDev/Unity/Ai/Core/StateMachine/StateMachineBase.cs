using UnityEngine;

namespace DevourDev.Unity.Ai.Core
{
    public abstract class StateMachineBase<TContext> : StateMachineBase, IStateMachine<TContext>
    {
        private TContext _context;


        public sealed override object ContextBase => _context;
        public TContext Context => _context;


        public void SetContext(TContext context)
        {
            _context = context;
        }


        protected sealed override void HandleSetBaseContext(object context)
        {
            SetContext((TContext)context);
        }
    }

    public abstract class StateMachineBase : MonoBehaviour, IStateMachineBase
    {
        private StateBehaviourBase _activeStateBehaviour;


        public StateBehaviourBase ActiveStateBehaviour => _activeStateBehaviour;

        public abstract object ContextBase { get; }


        public void SetBaseContext(object context)
        {
            HandleSetBaseContext(context);
        }


        public void ChangeState(StateSo state)
        {
            ExitFromActiveState();
            EnterState(state);
        }


        private void EnterState(StateSo state)
        {
            if (state == null)
                return;

            var beh = state.RentBehaviour();
            _activeStateBehaviour = beh;
            beh.EnterState(state);
        }

        private void ExitFromActiveState()
        {
            var beh = _activeStateBehaviour;

            if (beh == null)
                return;

            _activeStateBehaviour = null;

            beh.ExitState();
            beh.ReturnToPool();
        }

        protected abstract void HandleSetBaseContext(object context);
    }
}
