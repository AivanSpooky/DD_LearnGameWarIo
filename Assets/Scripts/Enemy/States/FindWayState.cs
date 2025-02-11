using LearnGame.FSM;
using UnityEngine;

namespace LearnGame.Enemy.States
{
    public class FindWayState: BaseState
    {
        private const float MaxDistanceBetweenRealAndCalculated = 3f;
        private readonly EnemyTarget _target;
        private readonly NavMesher _navMesher;
        private readonly EnemyDirController _enemyDirController;

        private Vector3 _curPoint;

        public FindWayState(EnemyTarget target, NavMesher navMesher, EnemyDirController enemyDirController)
        {
            _target = target;
            _navMesher = navMesher;
            _enemyDirController = enemyDirController;
        }

        public override void Execute()
        {
            Vector3 targetPosition = _target.Closest.transform.position;

            if (!_navMesher.IsPathCalculated || _navMesher.DistanceToTargetPointFrom(targetPosition) > 
                MaxDistanceBetweenRealAndCalculated)
                _navMesher.CalculatePath(targetPosition);

            var curPoint = _navMesher.GetCurPoint();
            if (_curPoint != curPoint)
            {
                _curPoint = curPoint;
                _enemyDirController.UpdateMoveDir(_curPoint);
            }
        }
    }
}
