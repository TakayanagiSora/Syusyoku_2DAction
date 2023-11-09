using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public abstract class Bullet : PoolObject<Bullet>
{
    [SerializeField, Tooltip("��{���x")]
    protected float _speed = default;
    [SerializeField, Tooltip("��������")]
    private float _lifeTime_s = default;

    protected Transform _transform = default;

    private CancellationTokenSource _cts = default;


    private void Awake()
    {
        _transform = this.transform;
    }

    private void Update()
    {
        Move();
    }

    protected async override void Enable(Vector2 spawnPos, Quaternion spawnDir)
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

    protected override void Disable()
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
        Return(this);
    }

    /// <summary>
    /// �e�̋���
    /// </summary>
    protected abstract void Move();
}
