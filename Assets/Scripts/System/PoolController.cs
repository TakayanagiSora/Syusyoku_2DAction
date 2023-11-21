using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolController : MonoBehaviour
{
    [Serializable]
    public class PoolData
    {
        public GameObject _prefab = default;
        [Tooltip("������")]
        public int _createAmount = default;
    }

    [SerializeField, Tooltip("�v�[��������I�u�W�F�N�g�̏�񂪊i�[���ꂽ���X�g")]
    private List<PoolData> _poolDataList = new List<PoolData>();
    [Tooltip("�v�[�����O�Ɏg�p����X�^�b�N���i�[�����A�z�z��")]
    private Dictionary<string, Stack<GameObject>> _objectPools = new Dictionary<string, Stack<GameObject>>();

    public List<PoolData> PoolDataList => _poolDataList;


    private void Awake()
    {
        InitialCreate();
    }

    private void InitialCreate()
    {
        for (int i = 0; i < _poolDataList.Count; i++)
        {
            // �L�[���I�u�W�F�N�g���A�l���X�^�b�N�Ƃ����A�z�z��̗v�f��ǉ�
            string key = _poolDataList[i]._prefab.name;
            _objectPools.Add(key, new Stack<GameObject>());

            // �ݒ肳�ꂽ���Y�v�f�̐��������J��Ԃ��A�C���X�^���X�𐶐�����
            for (int k = 0; k < _poolDataList[i]._createAmount; k++)
            {
                GameObject obj = Instantiate(_poolDataList[i]._prefab);
                // ���������C���X�^���X��A�z�z��̗v�f�̃X�^�b�N�Ƀv�b�V������
                _objectPools[key].Push(obj);
            }
        }
    }

    //public GameObject Get(string key)
    //{
    //    GameObject obj;

    //    if (_objectPools[key].Count == 0)
    //    {
    //        obj = Instantiate(_)
    //    }
    //}
}
