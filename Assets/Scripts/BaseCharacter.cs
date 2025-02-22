using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using LearnGame.Enemy;
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
        private Animator _animator;
        public bool HasDefaultWeapon { get; private set; } = true;

        [SerializeField]
        private float _maxHealth = 2f;
        public float MaxHealth => _maxHealth;
        private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        [SerializeField]
        private float _speedMultiplier = 2f;
        public bool SpeedBoostActive { get; private set; } = false;
        public bool Dying { get; private set; } = false;

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
            if (Dying) return;
            if (_currentHealth <= 0f && !Dying)
                Die();

            var dir = _MovementDirSource.MoveDir;
            var lookDir = dir;
            if (_SC.HasTarget)
                lookDir = (_SC.TargetPosition - transform.position).normalized;
            _CMC.MoveDir = dir;
            _CMC.LookDir = lookDir;


            _animator.SetBool("IsMoving", dir != Vector3.zero);
            _animator.SetBool("IsShooting", _SC.HasTarget);
            _animator.SetBool("IsDying", _currentHealth <= 0f);

            if (!SpeedBoostActive)
                if (Input.GetKey(KeyCode.Space))
                    _CMC.SetSpeedMultiplier(_speedMultiplier);
                else
                    _CMC.ResetSpeedMultiplier();
        }

        private void Die()
        {
            Dying = true;
            _CMC.MoveDir = Vector3.zero;
            _animator.SetLayerWeight(1, 0);

            var movementScript = GetComponent<PlayerMovementDirController>();
            if (movementScript) movementScript.enabled = false;
            var enemMovementScript = GetComponent<EnemyDirController>();
            if (enemMovementScript) enemMovementScript.enabled = false;

            if (_CMC) _CMC.enabled = false;
            var charController = GetComponent<CharacterController>();
            if (charController) charController.enabled = false;
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer) meshRenderer.enabled = false;
            if (_SC) _SC.enabled = false;

            //if (this is PlayerCharacter)
            //{
            //    var enemies = FindObjectsOfType<EnemyAiController>();
            //    foreach (var enemy in enemies)
            //        {
            //            enemy.ResetTargetIfDead(this);
            //            enemy.Update();
            //        }
            //}

            StartCoroutine(WaitForDeathAnimation());
        }

        private IEnumerator WaitForDeathAnimation()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            yield return new WaitForSeconds(/*stateInfo.length*/3);

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
