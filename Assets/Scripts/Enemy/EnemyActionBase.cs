using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class EnemyActionBase : MonoBehaviour
{
    protected Transform _transform = default;

    [SerializeField, Tooltip("移動速度"), Min(0)]
    protected float _moveSpeed = default;

    protected virtual void Awake()
    {
        _transform = this.transform;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <returns></returns>
    public abstract void Move();

    /// <summary>
    /// このアクションが開始するときに呼ばれる初期化処理
    /// <br>- CancellationTokenの生成など</br>
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// このアクションが終了するときに呼ばれる処理
    /// <br>- Disposeなど</br>
    /// </summary>
    public abstract void Disable();
}
