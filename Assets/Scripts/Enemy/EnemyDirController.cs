using LearnGame.Movement;
using UnityEngine;

namespace LearnGame.Enemy
{
    public class EnemyDirController : MonoBehaviour, IMovementDirSource
    {
        public Vector3 MoveDir { get; private set; }
        private readonly float baseSpeed = 1f;

        public void UpdateMoveDir(Vector3 targetPosition)
        {
            var realDir = targetPosition - transform.position;
            MoveDir = new Vector3(realDir.x, 0, realDir.z).normalized;
        }

        public void UpdateMoveDirWithBonus(Vector3 direction, float bonus)
        {
            MoveDir = direction.normalized * (baseSpeed + bonus);
        }
    }
}