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
        public int _createAmount = default;
    }

    [SerializeField]
    private List<PoolData> _poolDataList = new List<PoolData>();

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
            _objectPools.Add(_poolDataList[i]._prefab.gameObject.name, new Stack<GameObject>());

            for (int k = 0; k < _poolDataList[i]._createAmount; k++)
            {
                GameObject obj = Instantiate(_poolDataList[i]._prefab);
                //_objectPools[i].Push(obj);
            }
        }
    }

    //public GameObject Get(string key)
    //{
    //    if (_objectPools.)
    //}
}
