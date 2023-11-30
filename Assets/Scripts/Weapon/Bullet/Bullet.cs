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

    private CancellationTokenSource _cts = default;
    private CapsuleCollider2D _collider = default;
    [Tooltip("�q�b�g�����R���C�_�[���i�[����z��")]
    // Overlap-NonAlloc�֐��Ŏg�p�BGC�p�x�������邽�߁A�z������炩���ߍ쐬
    // �Î��I�Ɂu�q�b�g�������ׂẴR���C�_�[���i�[�v�Ƃ��������߁A�\���Ȑ����m��
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
            Debug.Log("�e���L�����Z�����ď��ł����ꍇ�̗�O����");
            _poolController.Return(this);
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

    private void CheckCollision()
    {
        // �����蔻����擾
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
    /// �e�̋���
    /// </summary>
    protected abstract void Move();
}
