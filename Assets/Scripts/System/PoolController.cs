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
        [Tooltip("生成数")]
        public int _createAmount = default;
    }

    [SerializeField, Tooltip("プール化するオブジェクトの情報が格納されたリスト")]
    private List<PoolData> _poolDataList = new List<PoolData>();
    [Tooltip("プーリングに使用するスタックを格納した連想配列")]
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
            // キーをオブジェクト名、値をスタックとした連想配列の要素を追加
            string key = _poolDataList[i]._prefab.name;
            _objectPools.Add(key, new Stack<GameObject>());

            // 設定された当該要素の生成数分繰り返し、インスタンスを生成する
            for (int k = 0; k < _poolDataList[i]._createAmount; k++)
            {
                GameObject obj = Instantiate(_poolDataList[i]._prefab);
                // 生成したインスタンスを連想配列の要素のスタックにプッシュする
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
