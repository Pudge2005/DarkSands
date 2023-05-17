using System;
using DevourDev.Utility;

namespace DevourDev.Ai
{
    public abstract class StateBehaviourBase : IAutoPoolableItem, IDisposable
    {
        public StateSoBase StateSo { get; internal set; }


        internal abstract void EnterBaseState(AiContextBase context);
        internal abstract void UpdateBaseState(AiContextBase context);
        internal abstract void ExitBaseState(AiContextBase context);


        public virtual void OnBeforeRent()
        {
        }

        public virtual void OnReturn()
        {
            StateSo = null;
        }

        public virtual void OnPoolableItemDestroy()
        {
        }

        public void Dispose()
        {
            StateSo.ReturnBaseStateBehaviour(this);
        }
    }

    public abstract class StateBehaviourBase<TContext> : StateBehaviourBase
        where TContext : AiContextBase
    {
        internal sealed override void EnterBaseState(AiContextBase context)
        {
            EnterState((TContext)context);
        }

        internal sealed override void UpdateBaseState(AiContextBase context)
        {
            UpdateState((TContext)context);
        }

        internal sealed override void ExitBaseState(AiContextBase context)
        {
            ExitState((TContext)context);
        }


        internal void EnterState(TContext context)
        {
            OnStateEnter(context);
        }

        internal void UpdateState(TContext context)
        {
            OnStateUpdate(context);
        }

        internal void ExitState(TContext context)
        {
            OnStateExit(context);
        }


        protected abstract void OnStateEnter(TContext context);
        protected abstract void OnStateUpdate(TContext context);
        protected abstract void OnStateExit(TContext context);
    }
}
