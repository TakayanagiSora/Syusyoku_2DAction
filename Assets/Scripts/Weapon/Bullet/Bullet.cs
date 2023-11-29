using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public abstract class Bullet : PoolObject
{
    [SerializeField, Tooltip("��{���x"), Min(0)]
    protected float _speed = default;
    [SerializeField, Tooltip("��������"), Min(0)]
    private float _lifeTime_s = default;
    [SerializeField, Tooltip("�U����"), Min(0)]
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
            Debug.Log("�e���L�����Z�����ď��ł����ꍇ�̗�O����");
        }
    }

    public override void Disable()
    {
        _cts = null;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �������Ԃ�񓯊��Ōv�����A���ł�����
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask LifeTimeAsync(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lifeTime_s, cancellationToken: token);
        _poolController.Return(this);
    }

    /// <summary>
    /// �e�̋���
    /// </summary>
    protected abstract void Move();
}
