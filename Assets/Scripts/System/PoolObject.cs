using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v�[�����ΏۃI�u�W�F�N�g�̊��N���X
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PoolObject<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("�v�[�����I�u�W�F�N�g")]
    private static T _originObject = default;
    private static Stack<T> _objectPool = new Stack<T>();
    [Tooltip("���������ς݂ł����true")]
    private static bool _isCreated = false;

    /// <summary>
    /// �v�[�����I�u�W�F�N�g�̃v���n�u��ݒ�
    /// </summary>
    public static T SetOrigin
    {
        set
        {
            if (_originObject is not null)
            {
                Debug.LogWarning("���Ƀv�[���ɓo�^����Ă��܂�");
                return;
            }

            _originObject = value;
        }
    }


    private void OnDestroy()
    {
        Clear();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="maxAmount">�v�[���e��</param>
    public static void InitialCreate(int maxAmount)
    {
        if (_isCreated)
        {
            Debug.LogWarning("���ɐ�������Ă��܂�");
            return;
        }

        _isCreated = true;

        for (int i = 0; i < maxAmount; i++)
        {
            T obj = Instantiate(_originObject);
            Return(obj);
        }
    }

    /// <summary>
    /// �v�[��������o��
    /// </summary>
    /// <param name="spawnPos">�o���ʒu</param>
    /// <param name="spawnDir">�o���p�x</param>
    /// <returns></returns>
    public static T Get(Vector2 spawnPos, Quaternion spawnDir)
    {
        T obj;

        // �X�^�b�N����łȂ���΃|�b�v�A��ł���Βǉ�����
        if (_objectPool.Count > 0)
        {
            obj = _objectPool.Pop();
        }
        else
        {
            Debug.LogWarning("�v�[���̗e�ʂ�����Ă��܂���");
            obj = Instantiate(_originObject);
        }

        // �������������Ăяo��
        (obj as PoolObject<T>).Enable(spawnPos, spawnDir);
        return obj;
    }

    /// <summary>
    /// �v�[���ɕԂ�
    /// </summary>
    /// <param name="obj">�ԋp�ΏۃI�u�W�F�N�g</param>
    public static void Return(T obj)
    {
        (obj as PoolObject<T>).Disable();
        _objectPool.Push(obj);
    }

    /// <summary>
    /// �I�u�W�F�N�g�j�����ɌĂяo����鏉��������
    /// <br>��F�V�[���J�ڎ�</br>
    /// </summary>
    private void Clear()
    {
        _originObject = null;
        _objectPool.Clear();
    }

    /// <summary>
    /// Get���̏���������
    /// <br>- SetActive���L�q���邱��</br>
    /// </summary>
    /// <param name="spawnPos">�o���ʒu</param>
    /// <param name="spawnDir">�o���p�x</param>
    protected abstract void Enable(Vector2 spawnPos, Quaternion spawnDir);
    /// <summary>
    /// Return���̏���������
    /// <br>- SetActive���L�q���邱��</br>
    /// </summary>
    protected abstract void Disable();
}
