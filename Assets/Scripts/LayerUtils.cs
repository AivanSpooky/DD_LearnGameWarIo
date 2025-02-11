using UnityEngine;

namespace LearnGame
{
    public static class LayerUtils
    {
        public const string BulletLayName = "Bullet";
        public const string PlayerLayName = "Player";
        public const string EnemyLayName = "Enemy";
        public const string PickUpLayName = "PickUp";
        public const string PickUpSpeedBonusLayName = "PickUpSpeedBonus";

        public static int BulletLay; // default player enemy -> 0 0 0 НЕ ПОЗВОЛЯЕТ СОЗДАТЬ ТУТ ЭТО ПОЛЕ
        public static readonly int PickUpLay = LayerMask.NameToLayer(PickUpLayName);
        public static readonly int PickUpSBLay = LayerMask.NameToLayer(PickUpSpeedBonusLayName);
        public static readonly int EnemyMask = LayerMask.GetMask(EnemyLayName);
        public static readonly int PlayerMask = LayerMask.GetMask(PlayerLayName);
        public static readonly int PickUpMask = LayerMask.GetMask(PickUpLayName);
        public static readonly int PickUpSpeedBonusMask = LayerMask.GetMask(PickUpSpeedBonusLayName);
        public static bool IsBullet(GameObject other) => other.layer == BulletLay;
        public static bool IsPickUp(GameObject other) => other.layer == PickUpLay || other.layer == PickUpSBLay;
    }
}
