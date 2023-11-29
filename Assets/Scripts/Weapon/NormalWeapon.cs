using UnityEngine;

public class NormalWeapon : Weapon
{
    [SerializeField]
    private PoolUser _normalBullet = default;

    protected override void Fire(ChargeLevel chargeStage)
    {
        switch (chargeStage)
        {
            case ChargeLevel.S:
                print("S");
                _poolController.Get(_normalBullet.Key, _transform.position, Quaternion.identity);
                break;

            case ChargeLevel.M:
                print("M");
                break;

            case ChargeLevel.L:
                print("L");
                break;
        }
    }
}
