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
            case ChargeLevel.S:
                print("S");
                _poolController.Get(_normalBullet_S.Key, _transform.position, Quaternion.identity);
                break;

            case ChargeLevel.M:
                print("M");
                _poolController.Get(_normalBullet_M.Key, _transform.position, Quaternion.identity);
                break;

            case ChargeLevel.L:
                print("L");
                _poolController.Get(_normalBullet_L.Key, _transform.position, Quaternion.identity);
                break;
        }
    }
}
