using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// 上下に動きながら弾を飛ばすアクション
/// </summary>
public class EnemyAction_ShootsBullet : EnemyActionBase
{
    [SerializeField, Tooltip("攻撃間隔"), Min(0)]
    private float _attackInterval = default;
    [SerializeField, Tooltip("上限移動量"), Min(0)]
    private float _moveLimit = default;
    [SerializeField]
    private UsePool _bullet = default;

    [Tooltip("累計移動量")]
    private float _movedDistance = 0f;
    [Tooltip("上方向への移動が可能")]
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

        // 移動方向を代入
        if (_canMoveUp)
        {
            moveDelta = _upDir * _moveSpeed;
        }
        else
        {
            moveDelta = _downDir * _moveSpeed;
        }

        // 進んだ距離の算出
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
    /// 方向転換を行う処理
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
