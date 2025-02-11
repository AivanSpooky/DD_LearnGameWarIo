using System;
using UnityEngine;

namespace LearnGame.SpawnZones
{
    public class PlayerSpawnZone : BaseSpawnZone
    {
        [SerializeField]
        private GameObject _playerPrefab;

        private static bool s_playerSpawned = false;

        public static event Action<PlayerCharacter> OnPlayerSpawned;

        protected override GameObject GetPrefabToSpawn()
        {
            if (!_playerPrefab) return null;
            return _playerPrefab;
        }

        protected override void SpawnItem()
        {
            if (s_playerSpawned)
                return;

            base.SpawnItem();

            if (_spawnedItems.Count > 0)
            {
                GameObject newest = _spawnedItems[^1];
                var playerComp = newest.GetComponent<PlayerCharacter>();
                if (playerComp != null)
                {
                    OnPlayerSpawned?.Invoke(playerComp);
                    s_playerSpawned = true;
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}