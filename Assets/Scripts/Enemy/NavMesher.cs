using UnityEngine;
using UnityEngine.AI;

namespace LearnGame.Enemy
{
    public class NavMesher
    {
        private const float DistanceEps = 1.5f;
        public bool IsPathCalculated {  get; private set; }

        private readonly NavMeshQueryFilter _filter;
        private readonly Transform _agentTransform;

        private NavMeshPath _navMeshPath;
        private NavMeshHit _targetHit;
        private int _curPathPointIndex;

        public NavMesher(Transform agentTransform)
        {
            _filter = new NavMeshQueryFilter { areaMask = NavMesh.AllAreas };
            _navMeshPath = new NavMeshPath();
            IsPathCalculated = false;
            _agentTransform = agentTransform;
        }
        public void CalculatePath(Vector3 targetPosition)
        {
            NavMesh.SamplePosition(_agentTransform.position, out var agentHit, 10f, _filter);
            NavMesh.SamplePosition(targetPosition, out var targetHit, 10f, _filter);

            IsPathCalculated = NavMesh.CalculatePath(agentHit.position, targetHit.position, _filter, _navMeshPath);
            _curPathPointIndex = 0;

        }

        public Vector3 GetCurPoint()
        {
            var curPoint = _navMeshPath.corners[_curPathPointIndex];
            var distance = (_agentTransform.position - curPoint).magnitude;

            if (distance < DistanceEps)
                _curPathPointIndex++;

            if (_curPathPointIndex >= _navMeshPath.corners.Length)
                IsPathCalculated = false;
            else
                curPoint = _navMeshPath.corners[_curPathPointIndex];

            return curPoint;
        }

        public float DistanceToTargetPointFrom(Vector3 position) => (_targetHit.position - position).magnitude;
    }
}
