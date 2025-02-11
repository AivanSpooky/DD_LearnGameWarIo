using System;
using UnityEngine;

namespace LearnGame.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _followCamOffset = Vector3.zero;
        [SerializeField]
        private Vector3 _rotationOffset = Vector3.zero;

        private PlayerCharacter _player;

        protected void OnEnable()
        {
            SpawnZones.PlayerSpawnZone.OnPlayerSpawned += HandlePlayerSpawned;
        }
        protected void OnDisable()
        {
            SpawnZones.PlayerSpawnZone.OnPlayerSpawned -= HandlePlayerSpawned;
        }

        private void HandlePlayerSpawned(PlayerCharacter player)
        {
            _player = player;
        }

        protected void Awake()
        {
            LayerUtils.BulletLay = LayerMask.NameToLayer(LayerUtils.BulletLayName);
            //if (!_player)
            //    throw new NullReferenceException($"Cam can't follow null player {nameof(_player)}");
        }

        protected void LateUpdate()
        {
            if (_player)
            {
                Vector3 targetRot = _rotationOffset - _followCamOffset;
                transform.position = _player.transform.position + _followCamOffset;
                transform.rotation = Quaternion.LookRotation(targetRot, Vector3.up);
            }
        }
    }
}
