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

    protected override void Fire()
    {
        NormalBullet.Get(_transform.position, Quaternion.identity);
    }
}
