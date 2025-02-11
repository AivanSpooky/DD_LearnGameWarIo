using LearnGame.Shooting;
using UnityEngine;

namespace LearnGame.PickUp
{
    public class PickUpWeapon : SpawnZoneItem
    {
        [field: SerializeField]
        public Weapon WeaponPrefab { get; private set; }

    }
}