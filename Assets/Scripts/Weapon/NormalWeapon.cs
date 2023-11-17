using UnityEngine;

public class NormalWeapon : Weapon
{
    [SerializeField]
    private NormalBullet_S _bulletPrefab_S = default;
    [SerializeField]
    private NormalBullet_M _bulletPrefab_M = default;
    [SerializeField]
    private NormalBullet_L _bulletPrefab_L = default;
    [SerializeField, Tooltip("íeÇÃèâä˙ê∂ê¨êî"), Min(0)]
    private int _bulletInitialAmount = default;

    private void Start()
    {
        NormalBullet_S.SetOrigin = _bulletPrefab_S;
        NormalBullet_S.InitialCreate(_bulletInitialAmount);

        NormalBullet_M.SetOrigin = _bulletPrefab_M;
        NormalBullet_M.InitialCreate(_bulletInitialAmount);

        NormalBullet_L.SetOrigin = _bulletPrefab_L;
        NormalBullet_L.InitialCreate(_bulletInitialAmount);
    }

    protected override void Fire(ChargeLevel chargeStage)
    {
        switch (chargeStage)
        {
            case ChargeLevel.S:
                print("S");
                NormalBullet_S.Get(_transform.position, Quaternion.identity);
                break;

            case ChargeLevel.M:
                print("M");
                NormalBullet_M.Get(_transform.position, Quaternion.identity);
                break;

            case ChargeLevel.L:
                print("L");
                NormalBullet_L.Get(_transform.position, Quaternion.identity);
                break;
        }
    }
}
