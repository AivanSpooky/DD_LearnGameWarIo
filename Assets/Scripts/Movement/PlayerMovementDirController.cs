using UnityEngine;

namespace LearnGame.Movement
{
    public class PlayerMovementDirController : MonoBehaviour, IMovementDirSource
    {
        private UnityEngine.Camera _cam;
        public Vector3 MoveDir { get; private set; }
        protected void Awake()
        {
            _cam = UnityEngine.Camera.main;
        }
        protected void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var dir = new Vector3(horizontal, 0, vertical);
            dir = _cam.transform.rotation * dir;
            dir.y = 0;
            MoveDir = dir.normalized;
            //MoveDir = new Vector3(horizontal, 0, vertical);
        }
    }
}