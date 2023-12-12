using UnityEngine;

public class NormalWeapon : Weapon
{
    [SerializeField]
    private UsePool _normalBullet_S = default;
    [SerializeField]
    private UsePool _normalBullet_M = default;
    [SerializeField]
    private UsePool _normalBullet_L = default;

    protected override void Fire(ChargeLevel chargeStage)
    {
        switch (chargeStage)
        {
            case global::ChargeLevel.S:
                _poolController.Get(_normalBullet_S.Key, _transform.position, Quaternion.identity);
                break;

            case global::ChargeLevel.M:
                _poolController.Get(_normalBullet_M.Key, _transform.position, Quaternion.identity);
                break;

            case global::ChargeLevel.L:
                _poolController.Get(_normalBullet_L.Key, _transform.position, Quaternion.identity);
                break;
        }
    }
}
