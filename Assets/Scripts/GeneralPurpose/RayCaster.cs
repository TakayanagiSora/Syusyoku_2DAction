using UnityEngine;
using UniRx;

/// <summary>
/// RayCast�𗘗p���钊�ۃN���X
/// <br>- �R���X�g���N�^�ł̕ϐ����������K�{</br>
/// </summary>
public class RayCaster : IRaycast, IGroundable
{
    private const int GROUND_LAYER = 1 << 6;

    [Tooltip("Ray����")]
    private readonly Vector2 _direction = default;
    [Tooltip("Ray��")]
    private readonly float _distance = default;

    private ReactiveProperty<bool> _isGrounded = new ReactiveProperty<bool>();

    public IReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;


    public RayCaster(Vector2 direction, float distance)
    {
        _direction = direction;
        _distance = distance;
    }

    public bool Check(Vector2 origin)
    {
        bool isHit = Physics2D.Raycast(origin, _direction, _distance);
        Debug.DrawRay(origin, _direction * _distance, Color.red);

        return isHit;
    }

    public bool Check(Vector2 origin, int layerMask)
    {
        bool isHit = Physics2D.Raycast(origin, _direction, _distance, layerMask);
        Debug.DrawRay(origin, _direction * _distance, Color.red);

        return isHit;
    }

    public bool CheckGround(Vector2 origin)
    {
        bool isHit = Physics2D.Raycast(origin, _direction, _distance, GROUND_LAYER);
        Debug.DrawRay(origin, _direction * _distance, Color.red);

        return isHit;
    }

    public void ICheckGround(Vector2 origin)
    {
        _isGrounded.Value = Physics2D.Raycast(origin, _direction, _distance, GROUND_LAYER);
        Debug.DrawRay(origin, _direction * _distance, Color.red);
    }
}
