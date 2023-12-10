using UnityEngine;

public class EnemyBullet : Bullet
{
    private Vector2 _direction = default;

    protected override void Awake()
    {
        base.Awake();

        // PlayerLayer‚ğİ’è
        _targetLayer = 1 << 8;
    }

    public override void Enable(Vector2 spawnPos, Quaternion spawnDir)
    {
        base.Enable(spawnPos, spawnDir);

        _direction = LookPlayer();
    }

    protected override void Move()
    {
        _transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private Vector2 LookPlayer()
    {
        return  (FindObjectOfType<PlayerMove>().transform.position - _transform.position).normalized;
    }
}
