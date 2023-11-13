using UnityEngine;

public class NormalWeapon : Weapon
{
    [SerializeField]
    private Bullet _razerPrefab = default;

    private void Start()
    {
        NormalBullet.SetOrigin = _razerPrefab;
        NormalBullet.InitialCreate(10);
    }

    protected override void Fire(ChargeStage chargeStage)
    {
        switch (chargeStage)
        {
            case ChargeStage.S:
                print("S");
                break;
            case ChargeStage.M:
                print("M");
                break;
            case ChargeStage.L:
                print("L");
                break;
            default:
                break;
        }

        NormalBullet.Get(_transform.position, Quaternion.identity);
    }
}
