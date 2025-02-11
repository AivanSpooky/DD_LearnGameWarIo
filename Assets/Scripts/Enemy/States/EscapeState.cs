using LearnGame.FSM;
using UnityEngine;

namespace LearnGame.Enemy.States
{
    public class EscapeState: BaseState
    {
        private readonly EnemyTarget _target;
        private readonly EnemyDirController _enemyDirController;
        private readonly float _escapeSpeedBonus;

        public EscapeState(EnemyTarget target,
                           EnemyDirController enemyDirController,
                           float escapeSpeedBonus)
        {
            _target = target;
            _enemyDirController = enemyDirController;
            _escapeSpeedBonus = escapeSpeedBonus;
        }

        public override void Execute()
        {
            if (!_target.Closest) return;

            var dirToClosest = _target.Closest.transform.position - _enemyDirController.transform.position;
            Vector3 oppositeDir = -dirToClosest.normalized;

            _enemyDirController.UpdateMoveDirWithBonus(oppositeDir, _escapeSpeedBonus);
        }
    }
}
