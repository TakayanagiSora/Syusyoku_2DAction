using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UniRx;

/// <summary>
/// �e�̊��V�X�e���i���ہj
/// </summary>
public abstract class Bullet : PoolObject
{
    [SerializeField, Tooltip("��{���x"), Min(0)]
    protected float _speed = default;
    [SerializeField, Tooltip("��������"), Min(0)]
    private float _lifeTime_s = default;
    [SerializeField, Tooltip("�U����"), Min(0)]
    private int _attackPower = default;

    protected Transform _transform = default;
    [Tooltip("�����蔻������Ώۂ̃��C���[")]
    // ����EnemyLayer
    protected int _targetLayer = 1 << 7;

    [Tooltip("���g�̃R���C�_�[")]
    private CapsuleCollider2D _myCollider = default;
    [Tooltip("�q�b�g�����R���C�_�[���i�[����z��")]
    // Overlap-NonAlloc�֐��Ŏg�p�BGC�p�x�������邽�߁A�z������炩���ߍ쐬
    // �Î��I�Ɂu�q�b�g�������ׂẴR���C�_�[���i�[�v�Ƃ��������߁A�\���Ȑ����m��
    private Collider2D[] _hitColliders = new Collider2D[16];

    private CancellationTokenSource _cts = default;

    [Tooltip("�e���Ώۂɓ��������Ƃ��ɔ��΂���C�x���g")]
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
        // ������
        _transform.position = spawnPos;
        _transform.rotation = spawnDir;
        this.gameObject.SetActive(true);

        // CancellationToken�𐶐����Aawait�������Ăяo��
        _cts = new CancellationTokenSource();
        try
        {
            await LifeTimeAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            // �L�����Z�����ꂽ�ꍇ�A�����ɏ��ł�����
            _poolController.Return(this);
        }
    }

    public override void Disable()
    {
        // �g�p�ς݂�CancellationToken��GC�Ώۂɂ���
        _cts = null;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �������Ԃ�񓯊��Ōv�����A���ł�����
    /// </summary>
    private async UniTask LifeTimeAsync(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lifeTime_s, cancellationToken: token);
        _poolController.Return(this);
    }

    /// <summary>
    /// �����蔻����擾���A�_���[�W��^���鏈��
    /// </summary>
    private void CheckCollision()
    {
        // �����蔻����擾
        int count = Physics2D.OverlapCapsuleNonAlloc(_transform.position, _myCollider.size, _myCollider.direction, 0f, _hitColliders, _targetLayer);

        // �擾�����R���C�_�[�̃I�u�W�F�N�g�Ƀ_���[�W��^����
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
    /// �e�̋���
    /// </summary>
    protected abstract void Move();
}
