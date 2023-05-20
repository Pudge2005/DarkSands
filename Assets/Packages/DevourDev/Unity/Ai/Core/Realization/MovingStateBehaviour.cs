using UnityEngine;

namespace DevourDev.Unity.Ai.Core
{
    public sealed class MyContext
    {

    }

    public sealed class MovingStateSo : StateSo<MovingStateSo, MovingStateBehaviour>
    {
        protected override StateBehaviourBase HandleRentBehaviour()
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleReturnBehaviour(StateBehaviourBase rentedBehaviour)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class MovingStateBehaviour : StateBehaviourBase<MovingStateSo, MovingStateBehaviour, object>
    {

        private void Move(Transform movingTransform,
            Vector3 movingVector, float movingSpeed)
        {
            movingTransform.Translate(movingVector.normalized * movingSpeed, Space.Self);
        }
    }
}
