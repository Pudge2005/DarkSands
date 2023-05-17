using DevourDev.Utility;
using UnityEngine;

namespace DevourDev.Ai
{
    public abstract class StateSoBase : ScriptableObject
    {
        public abstract StateBehaviourBase RentBaseStateBehaviour();
        public abstract void ReturnBaseStateBehaviour(StateBehaviourBase stateBehaviourBase);
    }

    /// <summary>
    /// Базовый класс состояния. Используется как ассоциация
    /// и как пул объектов-поведений данного состояния.
    /// </summary>
    /// <typeparam name="TStateBehaviour"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class StateSoBase<TStateBehaviour, TContext> : StateSoBase
        where TStateBehaviour : StateBehaviourBase<TContext>, new()
        where TContext : AiContextBase
    {
        private static readonly ThreadSafeAutoPool<TStateBehaviour> _pool = new();

        public sealed override StateBehaviourBase RentBaseStateBehaviour()
        {
            return GetStateBehaviour();
        }

        public sealed override void ReturnBaseStateBehaviour(StateBehaviourBase stateBehaviourBase)
        {
            ReturnStateBehaviour((TStateBehaviour)stateBehaviourBase);
        }

        public TStateBehaviour GetStateBehaviour()
        {
            return _pool.Rent();
        }

        public void ReturnStateBehaviour(TStateBehaviour stateBehaviour)
        {
            _pool.Return(stateBehaviour);
        }
    }
}
