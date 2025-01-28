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
        private bool _speedBoostActive = false;

        private IMovementDirSource _MovementDirSource;
        private CharacterMovementController _CMC;
        private ShootingController _SC;
        public void StartSpeedBoost(float multiplier, float duration)
        {
            StartCoroutine(SpeedBoostRoutine(multiplier, duration));
        }
        private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
        {
            _speedBoostActive = true;
            _CMC.SetSpeedMultiplier(multiplier);
            yield return new WaitForSeconds(duration);
            _speedBoostActive = false;
            _CMC.ResetSpeedMultiplier();
        }
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

            if (!_speedBoostActive)
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
                if (pickUp)
                    _SC.SetWeapon(pickUp.WeaponPrefab, _hand);

                var pickUpSB = other.gameObject.GetComponent<PickUpSpeedBonus>();
                if (pickUpSB)
                    StartSpeedBoost(pickUpSB._speedMultiplier, pickUpSB._duration);

                Destroy(other.gameObject);
            }
        }
    }
}
