using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolController : MonoBehaviour
{
    [Serializable]
    private class PoolData
    {
        public PoolUser _prefab = default;
        public int _createAmount = default;
    }

    [SerializeField]
    private List<PoolData> _poolData = new List<PoolData>();

    private Dictionary<string, Stack<PoolUser>> _objectPools = new Dictionary<string, Stack<PoolUser>>();


    private void Awake()
    {
        InitialCreate();
    }

    private void InitialCreate()
    {
        for (int i = 0; i < _poolData.Count; i++)
        {
            _objectPools.Add(_poolData[i]._prefab.gameObject.name, new Stack<PoolUser>());

            for (int k = 0; k < _poolData[i]._createAmount; k++)
            {
                PoolUser obj = Instantiate(_poolData[i]._prefab);
                //_objectPools[i].Push(obj);
            }
        }
    }

    //public PoolUser Get(string key)
    //{
    //    if (_objectPools.)
    //}
}
