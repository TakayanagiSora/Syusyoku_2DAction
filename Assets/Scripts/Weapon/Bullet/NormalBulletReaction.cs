using UnityEngine;
using UniRx;

public class NormalBulletReaction : MonoBehaviour
{
    [SerializeField]
    private UsePool _reaction_S = default;

    private Bullet _bullet = default;
    private PoolController _poolController = default;

    private void Awake()
    {
        _bullet = this.GetComponent<Bullet>();
        _bullet.OnHitBullet.Subscribe(position => CallReaction(position));

        _poolController = FindObjectOfType<PoolController>();
    }

    private void CallReaction(Vector2 hitPosition)
    {
        _poolController.Get(_reaction_S.Key, hitPosition, Quaternion.identity);
    }
}
