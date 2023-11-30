using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public abstract class Bullet : PoolObject
{
    [SerializeField, Tooltip("基本速度"), Min(0)]
    protected float _speed = default;
    [SerializeField, Tooltip("生存時間"), Min(0)]
    private float _lifeTime_s = default;
    [SerializeField, Tooltip("攻撃力"), Min(0)]
    private int _attackPower = default;

    protected Transform _transform = default;

    private CancellationTokenSource _cts = default;
    private CapsuleCollider2D _collider = default;
    [Tooltip("ヒットしたコライダーを格納する配列")]
    // Overlap-NonAlloc関数で使用。GC頻度を下げるため、配列をあらかじめ作成
    // 暗示的に「ヒットしたすべてのコライダーを格納」としたいため、十分な数を確保
    private Collider2D[] _hitColliders = new Collider2D[16];
    private const int ENEMY_LAYER = 1 << 7;


    private void Awake()
    {
        _transform = this.transform;
        _collider = this.GetComponent<CapsuleCollider2D>();
        _poolController = FindObjectOfType<PoolController>();
    }

    private void Update()
    {
        CheckCollision();
        Move();
    }

    public override async void Enable(Vector2 spawnPos, Quaternion spawnDir)
    {
        _transform.position = spawnPos;
        _transform.rotation = spawnDir;
        this.gameObject.SetActive(true);

        _cts = new CancellationTokenSource();
        try
        {
            await LifeTimeAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("弾がキャンセルして消滅した場合の例外処理");
            _poolController.Return(this);
        }
    }

    public override void Disable()
    {
        _cts = null;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 生存時間を非同期で計測し、消滅させる
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask LifeTimeAsync(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lifeTime_s, cancellationToken: token);
        _poolController.Return(this);
    }

    private void CheckCollision()
    {
        // 当たり判定を取得
        int count = Physics2D.OverlapCapsuleNonAlloc(_transform.position, _collider.size, _collider.direction, 0f, _hitColliders, ENEMY_LAYER);

        for (int i = 0; i < count; i++)
        {
            if (_hitColliders[i].TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(_attackPower);
                _cts.Cancel();
            }
        }
    }

    /// <summary>
    /// 弾の挙動
    /// </summary>
    protected abstract void Move();
}
