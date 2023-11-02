using UnityEngine;
using UniRx;

/// <summary>
/// RayCast�Őڒn������s���N���X
/// <br>- �R���X�g���N�^�ł̕ϐ����������K�{</br>
/// </summary>
public class GroundChecker
{
    private const int GROUND_LAYER = 1 << 6;

    [Tooltip("Ray����")]
    private readonly Vector2 _direction = default;
    [Tooltip("Ray��")]
    private readonly float _distance = default;

    private ReactiveProperty<bool> _isGrounded = new ReactiveProperty<bool>();

    /// <summary>
    /// �n�ʂɒ����Ă���Ƃ�true��Ԃ�Reactiveproperty
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
