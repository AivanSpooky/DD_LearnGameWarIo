using System;
using UnityEngine;

namespace LearnGame.SpawnZones
{
    public class PlayerSpawnZone : BaseSpawnZone
    {
        [SerializeField]
        private GameObject _playerPrefab;

        public static event Action<PlayerCharacter> OnPlayerSpawned;

        protected override GameObject GetPrefabToSpawn()
        {
            if (!_playerPrefab) return null;
            return _playerPrefab;
        }

        protected override void SpawnItem()
        {
            base.SpawnItem();

            if (_spawnedItems.Count > 0)
            {
                GameObject newest = _spawnedItems[^1];
                var playerComp = newest.GetComponent<PlayerCharacter>();
                if (playerComp != null)
                    OnPlayerSpawned?.Invoke(playerComp);
            }
        }
    }
}