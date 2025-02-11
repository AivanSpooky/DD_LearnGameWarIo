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
        public bool HasDefaultWeapon { get; private set; } = true;

        [SerializeField]
        private float _maxHealth = 2f;
        public float MaxHealth => _maxHealth;
        private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        [SerializeField]
        private float _speedMultiplier = 2f;
        public bool SpeedBoostActive { get; private set; } = false;

        private IMovementDirSource _MovementDirSource;
        private CharacterMovementController _CMC;
        private ShootingController _SC;

        public void StartSpeedBoost(float multiplier, float duration)
        {
            StartCoroutine(SpeedBoostRoutine(multiplier, duration));
        }

        private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
        {
            SpeedBoostActive = true;
            _CMC.SetSpeedMultiplier(multiplier);
            yield return new WaitForSeconds(duration);
            SpeedBoostActive = false;
            _CMC.ResetSpeedMultiplier();
        }

        protected void Awake()
        {
            _CMC = GetComponent<CharacterMovementController>();
            _MovementDirSource = GetComponent<IMovementDirSource>();
            _SC = GetComponent<ShootingController>();

            _currentHealth = _maxHealth;
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

            if (!SpeedBoostActive)
                if (Input.GetKey(KeyCode.Space))
                    _CMC.SetSpeedMultiplier(_speedMultiplier);
                else
                    _CMC.ResetSpeedMultiplier();

            if (_currentHealth <= 0f)
                Destroy(gameObject);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (LayerUtils.IsBullet(other.gameObject))
            {
                var bullet = other.gameObject.GetComponent<Bullet>();
                _currentHealth -= bullet.Damage;
                Destroy(other.gameObject);
            }
            else if (LayerUtils.IsPickUp(other.gameObject))
            {
                var pickUp = other.gameObject.GetComponent<PickUpWeapon>();
                if (pickUp)
                {
                    _SC.SetWeapon(pickUp.WeaponPrefab, _hand);
                    HasDefaultWeapon = false;
                }

                var pickUpSB = other.gameObject.GetComponent<PickUpSpeedBonus>();
                if (pickUpSB)
                    StartSpeedBoost(pickUpSB._speedMultiplier, pickUpSB._duration);

                Destroy(other.gameObject);
            }
        }
    }
}
