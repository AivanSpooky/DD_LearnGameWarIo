using UnityEngine;

namespace LearnGame.PickUp
{
    public class PickUpSpeedBonus : SpawnZoneItem
    {
        [SerializeField]
        public float _speedMultiplier = 2f;
        [SerializeField]
        public float _duration = 3f;
    }
}