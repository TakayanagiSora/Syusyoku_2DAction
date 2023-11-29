using UnityEngine;

public class NormalBullet : Bullet
{
    protected override void Move()
    {
        _transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }
}
