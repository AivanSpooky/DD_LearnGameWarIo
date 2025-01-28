using System.Collections.Generic;
using UnityEngine;

namespace LearnGame
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 5f;
        [SerializeField]
        private GameObject _pickUpPrefab;
        [SerializeField]
        private float _yOffset = 1f;
        [SerializeField]
        private int _maxItemsInZone = 3;
        [SerializeField]
        private float _spawnTimeMin = 3f;
        [SerializeField]
        private float _spawnTimeMax = 7f;

        private int _currentItemCount;
        private float _nextSpawnTimer;
        private readonly List<GameObject> _spawnedItems = new List<GameObject>();
        protected void Start()
        {
            _nextSpawnTimer = Random.Range(_spawnTimeMin, _spawnTimeMax);
        }
        protected void Update()
        {
            if (_currentItemCount < _maxItemsInZone)
            {
                _nextSpawnTimer -= Time.deltaTime;
                if (_nextSpawnTimer <= 0f)
                {
                    SpawnItem();
                    _nextSpawnTimer = Random.Range(_spawnTimeMin, _spawnTimeMax);
                }
            }
        }
        private void SpawnItem()
        {
            Vector2 randomPos2D = Random.insideUnitCircle * _radius;
            Vector3 spawnPos = new Vector3(
                transform.position.x + randomPos2D.x,
                transform.position.y + _yOffset,
                transform.position.z + randomPos2D.y
            );

            GameObject newItem = Instantiate(_pickUpPrefab, spawnPos, Quaternion.identity);
            newItem.GetComponent<SpawnZoneItem>()?.Init(this);
            _currentItemCount++;
            _spawnedItems.Add(newItem);
        }

        public void OnItemDestroyed(GameObject item)
        {
            if (_spawnedItems.Remove(item))
                _currentItemCount--;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}