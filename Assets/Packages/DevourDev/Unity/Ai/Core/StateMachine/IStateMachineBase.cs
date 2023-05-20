namespace DevourDev.Unity.Ai.Core
{
    public interface IStateMachineBase
    {
        StateBehaviourBase ActiveStateBehaviour { get; }
        object ContextBase { get; }


        void SetBaseContext(object context);

        void ChangeState(StateSo state);
    }
}
