using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UniRx;

/// <summary>
/// 弾の基底システム（抽象）
/// </summary>
public abstract class Bullet : PoolObject
{
    [SerializeField, Tooltip("基本速度"), Min(0)]
    protected float _speed = default;
    [SerializeField, Tooltip("生存時間"), Min(0)]
    private float _lifeTime_s = default;
    [SerializeField, Tooltip("攻撃力"), Min(0)]
    private int _attackPower = default;

    protected Transform _transform = default;
    [Tooltip("当たり判定を取る対象のレイヤー")]
    // 初期EnemyLayer
    protected int _targetLayer = 1 << 7;

    [Tooltip("自身のコライダー")]
    private CapsuleCollider2D _myCollider = default;
    [Tooltip("ヒットしたコライダーを格納する配列")]
    // Overlap-NonAlloc関数で使用。GC頻度を下げるため、配列をあらかじめ作成
    // 暗示的に「ヒットしたすべてのコライダーを格納」としたいため、十分な数を確保
    private Collider2D[] _hitColliders = new Collider2D[16];

    private CancellationTokenSource _cts = default;

    [Tooltip("弾が対象に当たったときに発火するイベント")]
    private Subject<Vector2> _hitSubject = new Subject<Vector2>();
    public IObservable<Vector2> OnHitBullet => _hitSubject;


    protected virtual void Awake()
    {
        _transform = this.transform;
        _myCollider = this.GetComponent<CapsuleCollider2D>();
        _poolController = FindObjectOfType<PoolController>();
    }

    private void Update()
    {
        CheckCollision();
        Move();
    }

    public override async void Enable(Vector2 spawnPos, Quaternion spawnDir)
    {
        // 初期化
        _transform.position = spawnPos;
        _transform.rotation = spawnDir;
        this.gameObject.SetActive(true);

        // CancellationTokenを生成し、await処理を呼び出す
        _cts = new CancellationTokenSource();
        try
        {
            await LifeTimeAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            // キャンセルされた場合、即座に消滅させる
            _poolController.Return(this);
        }
    }

    public override void Disable()
    {
        // 使用済みのCancellationTokenをGC対象にする
        _cts = null;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 生存時間を非同期で計測し、消滅させる
    /// </summary>
    private async UniTask LifeTimeAsync(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lifeTime_s, cancellationToken: token);
        _poolController.Return(this);
    }

    /// <summary>
    /// 当たり判定を取得し、ダメージを与える処理
    /// </summary>
    private void CheckCollision()
    {
        // 当たり判定を取得
        int count = Physics2D.OverlapCapsuleNonAlloc(_transform.position, _myCollider.size, _myCollider.direction, 0f, _hitColliders, _targetLayer);

        // 取得したコライダーのオブジェクトにダメージを与える
        for (int i = 0; i < count; i++)
        {
            if (_hitColliders[i].TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(_attackPower);
                _cts.Cancel();
                _hitSubject.OnNext(_transform.position);
            }
        }
    }

    /// <summary>
    /// 弾の挙動
    /// </summary>
    protected abstract void Move();
}
