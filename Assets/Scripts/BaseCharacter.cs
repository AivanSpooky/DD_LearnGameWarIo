using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LearnGame.Movement;
using LearnGame.PickUp;
using LearnGame.Shooting;
using UnityEngine;

namespace LearnGame
{
    [RequireComponent(typeof(CharacterMovementController), typeof(ShootingController))]
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField]
        private Weapon WeaponPrefab;
        [SerializeField]
        private Transform _hand;
        [SerializeField]
        private float _health = 2f;
        [SerializeField]
        private float _speedMultiplier = 2f;

        private IMovementDirSource _MovementDirSource;
        private CharacterMovementController _CMC;
        private ShootingController _SC;
        protected void Awake()
        {
            _CMC = GetComponent<CharacterMovementController>();
            _MovementDirSource = GetComponent<IMovementDirSource>();
            _SC = GetComponent<ShootingController>();
        }
        protected void Start()
        {
            _SC.SetWeapon(WeaponPrefab, _hand);
        }
        protected void Update()
        {
            var dir = _MovementDirSource.MoveDir;
            var lookDir = dir;
            if (_SC.HasTarget)
                lookDir = (_SC.TargetPosition - transform.position).normalized;
            _CMC.MoveDir = dir;
            _CMC.LookDir = lookDir;

            if (Input.GetKey(KeyCode.Space))
                _CMC.SetSpeedMultiplier(_speedMultiplier);
            else
                _CMC.ResetSpeedMultiplier();

            if (_health <= 0f)
                Destroy(gameObject);
        }
        protected void OnTriggerEnter(Collider other)
        {
            if (LayerUtils.IsBullet(other.gameObject))
            {
                var bullet = other.gameObject.GetComponent<Bullet>();
                _health -= bullet.Damage;
                Destroy(other.gameObject);
            }
            else if (LayerUtils.IsPickUp(other.gameObject))
            {
                var pickUp = other.gameObject.GetComponent<PickUpWeapon>();
                _SC.SetWeapon(pickUp.WeaponPrefab, _hand);
                Destroy(other.gameObject);
            }
        }
    }
}
