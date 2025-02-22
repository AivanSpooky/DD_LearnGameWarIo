using System.Collections;
using UnityEngine;

namespace LearnGame.Shooting
{
    public class ShootingController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _mask;
        public bool HasTarget => _target != null;
        public Vector3 TargetPosition => _target.transform.position;
        private Weapon _weapon;
        private Collider[] _colliders = new Collider[2];
        private float _nextShotTimerSec;
        private GameObject _target;
        protected void Update()
        {
            _target = GetTarget();
            _nextShotTimerSec -= Time.deltaTime;
            if ( _nextShotTimerSec < 0 )
            {
                if (HasTarget)
                    _weapon.Shoot(TargetPosition);
                _nextShotTimerSec = _weapon.ShootFrequencySec;
            }
        }

        public void SetWeapon(Weapon weaponPrefab, Transform hand)
        {
            if (_weapon)
                Destroy(_weapon.gameObject);
            _weapon = Instantiate(weaponPrefab, hand);
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;
        }
        private GameObject GetTarget()
        {
            GameObject target = null;
            var pos = _weapon.transform.position;
            var radius = _weapon.ShootRadius;
            var size = Physics.OverlapSphereNonAlloc(pos, radius, _colliders, _mask);
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    BaseCharacter character = _colliders[i].gameObject.GetComponent<BaseCharacter>();
                    if (_colliders[i].gameObject != gameObject && character && !character.Dying)
                    {
                        target = _colliders[i].gameObject;
                        break;
                    }
                }
            }
            return target;
        }
    }
}