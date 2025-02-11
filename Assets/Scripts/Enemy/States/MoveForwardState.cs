using LearnGame.FSM;
using UnityEngine;

namespace LearnGame.Enemy.States
{
    public class MoveForwardState: BaseState
    {
        private readonly EnemyTarget _target;
        private readonly EnemyDirController _enemyDirController;

        private Vector3 _curPoint;

        public MoveForwardState(EnemyTarget target, EnemyDirController enemyDirController)
        {
            _target = target;
            _enemyDirController = enemyDirController;
        }

        public override void Execute()
        {
            Vector3 targetPosition = _curPoint;
            if (_target.Closest)
                targetPosition = _target.Closest.transform.position;
            if (_curPoint != targetPosition )
            {
                _curPoint = targetPosition;
                _enemyDirController.UpdateMoveDir(targetPosition);
            }
        }
    }
}
