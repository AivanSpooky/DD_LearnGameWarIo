using System.Collections.Generic;
using UnityEngine;

namespace LearnGame.SpawnZones
{
    public class PickUpSpawnZone : BaseSpawnZone
    {
        [SerializeField]
        private GameObject _pickUpPrefab;

        protected override GameObject GetPrefabToSpawn()
        {
            return _pickUpPrefab;
        }
    }
}