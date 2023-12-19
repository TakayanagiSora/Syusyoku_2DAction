using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// �㉺�ɓ����Ȃ���e���΂��A�N�V����
/// </summary>
public class EnemyAction_ShootsBullet : EnemyActionBase
{
    [SerializeField, Tooltip("�U���Ԋu"), Min(0)]
    private float _attackInterval = default;
    [SerializeField, Tooltip("����ړ���"), Min(0)]
    private float _moveLimit = default;
    [SerializeField]
    private UsePool _bullet = default;

    [Tooltip("�݌v�ړ���")]
    private float _movedDistance = 0f;
    [Tooltip("������ւ̈ړ����\")]
    private bool _canMoveUp = true;
    private PoolController _poolController = default;
    private CancellationTokenSource _atk_cts;

    private readonly Vector2 _upDir = Vector2.up;
    private readonly Vector2 _downDir = Vector2.down;
    private readonly Vector2 _attackPosition = Vector2.left * 2f;

    protected override void Awake()
    {
        base.Awake();
        _poolController = FindObjectOfType<PoolController>();
    }

    private async UniTask Attack(CancellationToken token)
    {
        while (true)
        {
            _poolController.Get(_bullet.Key, (Vector2)_transform.position + _attackPosition, Quaternion.identity);
            await UniTask.WaitForSeconds(_attackInterval, cancellationToken: token);
        }
    }

    public override void Move()
    {
        ChangeDirection();

        Vector2 moveDelta;

        // �ړ���������
        if (_canMoveUp)
        {
            moveDelta = _upDir * _moveSpeed;
        }
        else
        {
            moveDelta = _downDir * _moveSpeed;
        }

        // �i�񂾋����̎Z�o
        _movedDistance += _moveSpeed * Time.deltaTime;
        _transform.Translate(moveDelta * Time.deltaTime);
    }

    public override async void Enable()
    {
        _atk_cts = new CancellationTokenSource();
        await Attack(_atk_cts.Token);
    }

    public override void Disable()
    {
        
    }

    /// <summary>
    /// �����]�����s������
    /// </summary>
    private void ChangeDirection()
    {
        if (_movedDistance >= _moveLimit)
        {
            _movedDistance = 0f;
            _canMoveUp = !_canMoveUp;
        }
    }
}
