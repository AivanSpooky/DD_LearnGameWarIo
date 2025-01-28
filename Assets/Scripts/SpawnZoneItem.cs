using UnityEngine;

namespace LearnGame
{
    public class SpawnZoneItem : MonoBehaviour
    {
        private SpawnZone _zone;
        public void Init(SpawnZone zone)
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