using DevourDev.Utility;

namespace DevourDev.Ai
{
    public abstract class Fsm
    {
        private readonly AiSensorDataCollection _sensorDataCollection = new();


        public AiSensorDataCollection SensorDataCollection => _sensorDataCollection;
        public abstract StateBehaviourBase CurrentBaseState { get; }


        public void ChangeBaseState(StateSoBase stateSo, AiContextBase context)
        {
            var behaviour = stateSo.RentBaseStateBehaviour();
            ChangeBaseState(behaviour, context);
        }

        public abstract void ChangeBaseState(StateBehaviourBase state, AiContextBase context);
    }

    public class Fsm<TContext> : Fsm
         where TContext : AiContextBase, new()
    {
        private static readonly ThreadSafeAutoPool<TContext> _contextPool = new();

        private StateBehaviourBase<TContext> _currentState;
        private TContext _context;


        public override StateBehaviourBase CurrentBaseState => _currentState;
        public TContext Context => _context;


        public static TContext RentContext() => _contextPool.Rent();
        public static void ReturnContext(TContext context) => _contextPool.Return(context);
, т
        public void Update(TContext context)
        {
            if (_context != null)
            {
                ReturnContext(_context);
            }

            context.StateMachine = this;
            _context = context;

            if (_currentState == null)
                return;

            _currentState.UpdateState(context);
        }

        public override void ChangeBaseState(StateBehaviourBase state, AiContextBase context)
        {
            ChangeState((StateBehaviourBase<TContext>)state, (TContext)context);
        }

        public void ChangeState(StateBehaviourBase<TContext> state, TContext context)
        {
            var prevState = _currentState;
            _currentState = state;

            if (prevState != null)
            {
                prevState.ExitState(context);
                prevState.Dispose();
            }

            if (state != null)
            {
                state.EnterState(context);
            }
        }
    }
}
