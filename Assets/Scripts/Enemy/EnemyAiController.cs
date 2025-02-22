using System.Collections;
using LearnGame.Enemy.States;
using UnityEngine;

namespace LearnGame.Enemy
{
    public class EnemyAiController : MonoBehaviour
    {
        [SerializeField]
        private float _viewRadius = 20f;
        private EnemyTarget _target;
        private EnemyStateMachine _stateMachine;

        public void ResetTargetIfDead(BaseCharacter character)
        {
            if (_target.Closest == character.gameObject)
            {
                _target.ResetTarget();
                _target.FindClosest();

                if (_target.Closest == null)
                {
                    Debug.LogWarning($"Враг {gameObject.name} потерял цель!");
                }
            }
        }

        protected void Awake()
        {
            var player = FindObjectOfType<PlayerCharacter>();
            var enemyDirController = GetComponent<EnemyDirController>();
            var selfCharacter = GetComponent<EnemyCharacter>();

            var navMesher = new NavMesher(transform);
            _target = new EnemyTarget(transform, player, _viewRadius, selfCharacter);

            _stateMachine = new EnemyStateMachine(enemyDirController, navMesher, _target, selfCharacter);
        }

        public void Update()
        {
            _target.FindClosest();
            _stateMachine.Update();
        }
    }
}