using UnityEngine;
using UnityEngine.AI;

namespace DevourDev.Unity.CharacterControlling.Movement
{
    public sealed class NavMeshMovementController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private IMovementStats _movementStats;

        private bool _isMoving;
        private bool _isFollowingPath;

        private Vector3 _moveInDirection;
        private float _moveSpeedCached;


        [field: SerializeField] public float MoveSpeed { get; set; }

        public Vector3 MoveDirection
        {
            get
            {
                if (!_isMoving)
                    return Vector3.zero;

                if (_isFollowingPath)
                    return transform.forward * _moveSpeedCached;

                return _moveInDirection;
            }
            set
            {
                SetMoveInDirection(value);
            }
        }

        public IMovementStats MovementStats
        {
            get => _movementStats;
            set => SetMovementStats(value);
        }


        private void Update()
        {
            Move(Time.deltaTime);
        }

        public void StopMoving()
        {
            if (!_isMoving)
                return;

            if (_isFollowingPath)
                CancelPathFollowing();

            _isMoving = false;
        }

        public void MoveToPoint(Vector3 worldPoint)
        {
            _isMoving = true;
            _isFollowingPath = true;

            _navMeshAgent.isStopped = false;
            bool pathBuilt = _navMeshAgent.SetDestination(worldPoint);

            if (!pathBuilt)
                return;
        }


        private void Move(float deltaTime)
        {
            if (!_isMoving)
                return;

            MoveInDirection(deltaTime);
        }

        private void MoveInDirection(float deltaTime)
        {
            if (_isFollowingPath)
                return;

            _navMeshAgent.Move(_moveInDirection * (_moveSpeedCached * deltaTime));
        }

        private void SetMovementStats(IMovementStats value)
        {
            float prevMoveSpeed = default;
            if (_movementStats != null)
            {
                _movementStats.MoveSpeedChanged -= MovementStats_MoveSpeedChanged;
                prevMoveSpeed = _movementStats.MoveSpeed;
            }

            _movementStats = value;

            if (value == null)
                return;

            value.MoveSpeedChanged += MovementStats_MoveSpeedChanged;
            MovementStats_MoveSpeedChanged(value, prevMoveSpeed, value.MoveSpeed);
        }

        private void MovementStats_MoveSpeedChanged(IMovementStats stats, float prevSpeed, float newSpeed)
        {
            _moveSpeedCached = newSpeed;
            _navMeshAgent.speed = newSpeed;
        }

        private void SetMoveInDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                StopMoving();
                return;
            }

            _moveInDirection = direction.normalized;
            _isMoving = true;

            CancelPathFollowing();
        }

        private void CancelPathFollowing()
        {
            if (!_isMoving || !_isFollowingPath)
                return;

            _isFollowingPath = false;
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = true;
        }
    }
}
