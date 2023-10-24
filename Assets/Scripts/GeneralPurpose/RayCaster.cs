using UnityEngine;

/// <summary>
/// RayCast�𗘗p���钊�ۃN���X
/// <br>- �R���X�g���N�^�ł̕ϐ����������K�{</br>
/// </summary>
public class RayCaster : IRaycast, IGroundable
{
    private const int GROUND_LAYER = 1 << 6;

    [Tooltip("Ray����")]
    private Vector2 _direction = default;
    [Tooltip("Ray��")]
    private float _distance = default;


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
}
