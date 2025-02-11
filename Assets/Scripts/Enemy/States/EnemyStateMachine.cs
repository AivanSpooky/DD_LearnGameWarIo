using System.Collections.Generic;
using LearnGame.FSM;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace LearnGame.Enemy.States
{
    public class EnemyStateMachine: BaseStateMachine
    {
        private const float NavMeshTurnOffDistance = 5;

        private BaseCharacter _selfCharacter;
        private EnemyTarget _target;

        [SerializeField]
        private float _escapeHPThreshold = 0.7f; //0.3
        [SerializeField]
        private float _escapeProbability = 1.1f; //0.7
        [SerializeField]
        private float _escapeSpeedBonus = 0.5f;

        public EnemyStateMachine(EnemyDirController enemyDirController,
            NavMesher navMesher, EnemyTarget target, BaseCharacter selfCharacter)
        {
            _selfCharacter = selfCharacter;
            _target = target;

            var idleState = new IdleState();
            var findWayState = new FindWayState(target, navMesher, enemyDirController);
            var moveForwardState = new MoveForwardState(target, enemyDirController);
            var escapeState = new EscapeState(target, enemyDirController, _escapeSpeedBonus);

            SetInitialState(idleState);

            AddState(state: idleState, transitions: new List<Transition>
                {
                    new Transition(
                        findWayState,
                        () => target.DistanceToClosestFromAgent() > NavMeshTurnOffDistance),
                    new Transition(
                        moveForwardState,
                        () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance),
                    new Transition(
                        escapeState,
                        () => IsLowHP() && ShouldEscape()),
                }
            );

            AddState(state: findWayState, transitions: new List<Transition>
                {
                    new Transition(
                        idleState,
                        () => !target.Closest),
                    new Transition(
                        moveForwardState,
                        () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance),
                    //new Transition(
                    //    escapeState,
                    //    () => IsLowHP() && ShouldEscape()),
                }
            );

            AddState(state: moveForwardState, transitions: new List<Transition>
                {
                    new Transition(
                        idleState,
                        () => !target.Closest),
                    new Transition(
                        findWayState,
                        () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance),
                    new Transition(
                        escapeState,
                        () => IsLowHP() && ShouldEscape()),
                }
            );

            AddState(escapeState, new List<Transition>
                {
                    new Transition(
                        idleState,
                        () => !target.Closest),
                }
            );
        }

        private bool IsLowHP()
        {
            return (!_target.Closest) ? false : _selfCharacter.CurrentHealth < _selfCharacter.MaxHealth * _escapeHPThreshold;
        }

        private bool ShouldEscape()
        {
            return Random.value < _escapeProbability;
        }
    }
}
