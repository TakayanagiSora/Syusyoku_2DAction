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
    private PoolController _poolController = default;

    private CancellationTokenSource _cts = default;


    private void Awake()
    {
        _transform = this.transform;
        _poolController = FindObjectOfType<PoolController>();
    }

    private void Update()
    {
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

    /// <summary>
    /// 弾の挙動
    /// </summary>
    protected abstract void Move();
}
