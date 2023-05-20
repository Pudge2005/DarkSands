namespace DevourDev.Unity.Ai.Core
{
    public interface IStateMachine<TContext> : IStateMachineBase
    {
        TContext Context { get; }


        void SetContext(TContext context);
    }
}
