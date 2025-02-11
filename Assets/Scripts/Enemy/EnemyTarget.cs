using UnityEngine;

namespace LearnGame.Enemy
{
    public class EnemyTarget
    {
        public GameObject Closest {  get; private set; }

        private readonly float _viewRadius;
        private readonly Transform _agentTransform;
        private readonly PlayerCharacter _player;
        private readonly EnemyCharacter _agentCharacter;

        //public float Health
        //{
        //    get
        //    {
        //        var baseCharacter = Closest ? Closest.GetComponent<BaseCharacter>() : null;
        //        return baseCharacter ? baseCharacter.CurrentHealth : 0f;
        //    }
        //}

        private readonly Collider[] _colliders = new Collider[10];

        public EnemyTarget(Transform agent, PlayerCharacter player, float viewRadius,
            EnemyCharacter agentCharacter)
        {
            _agentTransform = agent;
            _agentCharacter = agentCharacter;
            _player = player;
            _viewRadius = viewRadius;
        }

        public void FindClosest()
        {
            float minDistance = float.MaxValue;
            Closest = null;

            int finalMask = 0;
            if (!_agentCharacter.SpeedBoostActive)
                finalMask |= LayerUtils.PickUpSpeedBonusMask;
            if (_agentCharacter.HasDefaultWeapon)
                finalMask |= LayerUtils.PickUpMask;
            finalMask |= LayerUtils.PlayerMask;

            var count = FindAllTargets(finalMask);

            for (int i = 0; i < count; i++)
            {
                var go = _colliders[i].gameObject;
                if (go == _agentTransform.gameObject) continue;

                var distance = DistanceFromAgentTo(go);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    Closest = go;
                }
            }

            if (_player && DistanceFromAgentTo(_player.gameObject) < minDistance)
                Closest = _player.gameObject;
        }

        public float DistanceToClosestFromAgent()
        {
            if (Closest)
                return DistanceFromAgentTo(Closest);

            return 0;
        }

        private int FindAllTargets(int layerMask)
        {
            var size = Physics.OverlapSphereNonAlloc(
                _agentTransform.position,
                _viewRadius,
                _colliders,
                layerMask);
            return size;
        }

        private float DistanceFromAgentTo(GameObject go) => (_agentTransform.position - go.transform.position).magnitude;
    }
}
