using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearnGame.SpawnZones
{
    public abstract class BaseSpawnZone : MonoBehaviour
    {
        [SerializeField]
        protected float _radius = 5f;
        [SerializeField]
        protected float _yOffset = 2f;
        [SerializeField]
        protected int _maxItemsInZone = 3;
        [SerializeField]
        protected float _spawnTimeMin = 3f;
        [SerializeField]
        protected float _spawnTimeMax = 7f;


        protected int _currentItemCount;
        protected float _nextSpawnTimer;
        protected List<GameObject> _spawnedItems = new List<GameObject>();

        protected abstract GameObject GetPrefabToSpawn();

        protected virtual void Start()
        {
            _nextSpawnTimer = Random.Range(_spawnTimeMin, _spawnTimeMax);
        }

        protected virtual void Update()
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

        protected virtual void SpawnItem()
        {
            var prefabToSpawn = GetPrefabToSpawn();
            if (!prefabToSpawn) return;

            Vector2 randomPos2D = Random.insideUnitCircle * _radius;
            Vector3 spawnPos = new Vector3(
                transform.position.x + randomPos2D.x,
                transform.position.y + _yOffset,
                transform.position.z + randomPos2D.y
            );

            GameObject newItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            newItem.GetComponent<SpawnZoneItem>()?.Init(this);

            _currentItemCount++;
            _spawnedItems.Add(newItem);
        }

        public virtual void OnItemDestroyed(GameObject item)
        {
            if (_spawnedItems.Remove(item))
                _currentItemCount--;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}