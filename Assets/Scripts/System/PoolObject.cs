using UnityEngine;

/// <summary>
/// �v�[�����ΏۃI�u�W�F�N�g�̊��N���X
/// </summary>
public abstract class PoolObject : MonoBehaviour
{
    protected PoolController _poolController = default;

    /// <summary>
    /// Get���̏���������
    /// <br>- SetActive���L�q���邱��</br>
    /// </summary>
    /// <param name="spawnPos">�o���ʒu</param>
    /// <param name="spawnDir">�o���p�x</param>
    public abstract void Enable(Vector2 spawnPos, Quaternion spawnDir);
    /// <summary>
    /// Return���̏���������
    /// <br>- SetActive���L�q���邱��</br>
    /// </summary>
    public abstract void Disable();
}
