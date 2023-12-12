using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class EnemyActionBase : MonoBehaviour
{
    protected Transform _transform = default;

    [SerializeField, Tooltip("�ړ����x"), Min(0)]
    protected float _moveSpeed = default;

    protected virtual void Awake()
    {
        _transform = this.transform;
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    /// <returns></returns>
    public abstract void Move();

    /// <summary>
    /// ���̃A�N�V�������J�n����Ƃ��ɌĂ΂�鏉��������
    /// <br>- CancellationToken�̐����Ȃ�</br>
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// ���̃A�N�V�������I������Ƃ��ɌĂ΂�鏈��
    /// <br>- Dispose�Ȃ�</br>
    /// </summary>
    public abstract void Disable();
}
