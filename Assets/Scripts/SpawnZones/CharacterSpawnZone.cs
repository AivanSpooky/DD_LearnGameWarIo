using UnityEngine;

namespace LearnGame.SpawnZones
{
    public class CharacterSpawnZone : BaseSpawnZone
    {
        [SerializeField]
        private GameObject[] _charactersToSpawn;

        protected override GameObject GetPrefabToSpawn()
        {
            if (_charactersToSpawn == null || _charactersToSpawn.Length == 0)
                return null;

            int index = Random.Range(0, _charactersToSpawn.Length);
            return _charactersToSpawn[index];
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}