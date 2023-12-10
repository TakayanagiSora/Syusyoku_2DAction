using UnityEngine;

public class NormalBullet : Bullet
{
    private Vector2 _direction = Vector2.right;

    protected override void Awake()
    {
        base.Awake();
        // EnemyLayer‚ğ–¾¦“I‚Éİ’è
        _targetLayer = 1 << 7;
    }

    protected override void Move()
    {
        _transform.Translate(_direction * _speed * Time.deltaTime);
    }
}
