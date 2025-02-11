using LearnGame.SpawnZones;
using UnityEngine;

namespace LearnGame
{
    public class SpawnZoneItem : MonoBehaviour
    {
        private BaseSpawnZone _zone;
        public void Init(BaseSpawnZone zone)
        {
            _zone = zone;
        }

        private void OnDestroy()
        {
            if (_zone)
                _zone.OnItemDestroyed(gameObject);
        }
    }
}