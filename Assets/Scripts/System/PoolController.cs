using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolController : MonoBehaviour
{
    [Serializable]
    public class PoolData
    {
        public PoolObject _prefab = default;
        [Tooltip("������")]
        public int _createAmount = default;
    }

    [SerializeField, Tooltip("�v�[��������I�u�W�F�N�g�̏�񂪊i�[���ꂽ���X�g")]
    private List<PoolData> _poolDataList = new List<PoolData>();

    [Tooltip("�v�[�����O�Ɏg�p����X�^�b�N���i�[�����A�z�z��")]
    private Dictionary<string, Stack<PoolObject>> _objectPools = new Dictionary<string, Stack<PoolObject>>();

    [Tooltip("�X�^�b�N�ƃ��X�g�̃C���f�b�N�X��A�������邽�߂̘A�z�z��")]
    // �C���Q�[�����̗�O������_poolDataList���Q�Ƃ���K�v�����邽�ߍ쐬
    private Dictionary<Stack<PoolObject>, int> _poolIndices = new Dictionary<Stack<PoolObject>, int>();

    public List<PoolData> PoolDataList => _poolDataList;


    private void Awake()
    {
        InitialCreate();
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[���̏�������
    /// </summary>
    private void InitialCreate()
    {
        for (int i = 0; i < _poolDataList.Count; i++)
        {
            string key;

            try
            {
                key = _poolDataList[i]._prefab.name;
            }
            catch (NullReferenceException)
            {
                Debug.LogError($"PoolDataList��Index:[{i}]��GameObject���o�^����Ă��܂���B");
                continue;
            }

            Stack<PoolObject> stack = new Stack<PoolObject>();
            // �L�[���I�u�W�F�N�g���A�l���X�^�b�N�Ƃ����A�z�z��̗v�f��ǉ�
            _objectPools.Add(key, stack);
            _poolIndices.Add(stack, i);

            // �ݒ肳�ꂽ���Y�v�f�̐��������J��Ԃ��A�C���X�^���X�𐶐�����
            for (int k = 0; k < _poolDataList[i]._createAmount; k++)
            {
                PoolObject obj = Instantiate(_poolDataList[i]._prefab);
                // ���������C���X�^���X��A�z�z��̗v�f�̃X�^�b�N�Ƀv�b�V������
                _objectPools[key].Push(obj);
                obj.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �v�[������GameObject����肾��
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public PoolObject Get(string key, Vector2 spawnPlace, Quaternion spawnDir)
    {
        PoolObject obj;

        // �X�^�b�N�̒��g����̂Ƃ��A�V���ɐ������X�^�b�N�ɉ�����
        if (_objectPools[key].Count == 0)
        {
            // �󂯎�����L�[���烊�X�g�̃C���f�b�N�X���擾
            int poolDataIndex = _poolIndices[_objectPools[key]];
            obj = Instantiate(_poolDataList[poolDataIndex]._prefab);
        }
        // �󂯎�����L�[����X�^�b�N����肵�A���o��
        else
        {
            obj = _objectPools[key].Pop();
        }

        obj.Enable(spawnPlace, spawnDir);
        return obj;
    }

    /// <summary>
    /// �v�[����GameObject��߂�
    /// </summary>
    /// <param name="obj"></param>
    public void Return(PoolObject obj)
    {
        obj.Disable();

        // �󂯎�����I�u�W�F�N�g����L�[���Z�o�iClone�\�L�̍폜�j
        string key = Wrapper.OriginalizeTheName(obj.name);
        _objectPools[key].Push(obj);
    }
}
