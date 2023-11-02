using UnityEngine;
using UniRx;

/// <summary>
/// RayCastで接地判定を行うクラス
/// <br>- コンストラクタでの変数初期化が必須</br>
/// </summary>
public class GroundChecker
{
    private const int GROUND_LAYER = 1 << 6;

    [Tooltip("Ray方向")]
    private readonly Vector2 _direction = default;
    [Tooltip("Ray長")]
    private readonly float _distance = default;

    private ReactiveProperty<bool> _isGrounded = new ReactiveProperty<bool>();

    /// <summary>
    /// 地面に着いているときtrueを返すReactiveproperty
    /// </summary>
    public IReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;


    public GroundChecker(Vector2 direction, float distance)
    {
        _direction = direction;
        _distance = distance;
    }

    public void Check(Vector2 origin)
    {
        _isGrounded.Value = Physics2D.Raycast(origin, _direction, _distance, GROUND_LAYER);
        Debug.DrawRay(origin, _direction * _distance, Color.red);
    }
}
