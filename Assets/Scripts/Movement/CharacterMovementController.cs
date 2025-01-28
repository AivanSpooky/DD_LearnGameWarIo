using UnityEngine;

namespace LearnGame.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementController : MonoBehaviour
    {
        private static readonly float sqrEps = Mathf.Epsilon * Mathf.Epsilon;
        [SerializeField]
        private float _speed = 1f;
        private float _curSpeedMultiplier = 1f;

        [SerializeField]
        private float _maxRadDelta = 10f;
        private CharacterController _characterController;
        public Vector3 MoveDir { get; set; }
        public Vector3 LookDir { get; set; }
        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>(); 
        }
        protected void Update()
        {
            Translate();
            if (_maxRadDelta > 0 && LookDir != Vector3.zero)
                Rotate();
        }
        private void Rotate()
        {
            var curLookDir = transform.rotation * Vector3.forward;
            float sqrMagnitude = (curLookDir - LookDir).sqrMagnitude;
            if (sqrMagnitude > sqrEps)
            {
                var newRot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookDir, Vector3.up), _maxRadDelta * Time.deltaTime);
                transform.rotation = newRot;
            }
        }
        private void Translate()
        {
            var delta = MoveDir * _speed * _curSpeedMultiplier * Time.deltaTime;
            _characterController.Move(delta);
        }

        public void SetSpeedMultiplier(float multiplier) => _curSpeedMultiplier = multiplier;
        public void ResetSpeedMultiplier() => _curSpeedMultiplier = 1f;
    }
}