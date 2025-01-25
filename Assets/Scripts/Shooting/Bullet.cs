using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace LearnGame.Shooting
{
    public class Bullet : MonoBehaviour
    {
        public float Damage { get; private set; }
        private Vector3 _dir;
        private float _flySpeed;
        private float _maxFlyDist;
        private float _curFlyDist;
        public void Initialize(Vector3 direction, float maxFLyDist, float flySpeed, float damage)
        {
            _dir = direction;
            _maxFlyDist = maxFLyDist;
            _flySpeed = flySpeed;
            Damage = damage;
        }
        protected void Update()
        {
            var delta = _flySpeed * Time.deltaTime;
            _curFlyDist += delta;
            transform.Translate(_dir * delta);

            if (_curFlyDist >= _maxFlyDist)
                Destroy(gameObject);
        }
    }
}
