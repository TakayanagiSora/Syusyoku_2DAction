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
        [Tooltip("生成数")]
        public int _createAmount = default;
    }

    [SerializeField, Tooltip("プール化するオブジェクトの情報が格納されたリスト")]
    private List<PoolData> _poolDataList = new List<PoolData>();

    [Tooltip("プーリングに使用するスタックを格納した連想配列")]
    private Dictionary<string, Stack<PoolObject>> _objectPools = new Dictionary<string, Stack<PoolObject>>();

    [Tooltip("スタックとリストのインデックスを連結させるための連想配列")]
    // インゲーム中の例外処理で_poolDataListを参照する必要があるため作成
    private Dictionary<Stack<PoolObject>, int> _poolIndices = new Dictionary<Stack<PoolObject>, int>();

    public List<PoolData> PoolDataList => _poolDataList;


    private void Awake()
    {
        InitialCreate();
    }

    /// <summary>
    /// オブジェクトプールの初期生成
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
                Debug.LogError($"PoolDataListのIndex:[{i}]にGameObjectが登録されていません。");
                continue;
            }

            Stack<PoolObject> stack = new Stack<PoolObject>();
            // キーをオブジェクト名、値をスタックとした連想配列の要素を追加
            _objectPools.Add(key, stack);
            _poolIndices.Add(stack, i);

            // 設定された当該要素の生成数分繰り返し、インスタンスを生成する
            for (int k = 0; k < _poolDataList[i]._createAmount; k++)
            {
                PoolObject obj = Instantiate(_poolDataList[i]._prefab);
                // 生成したインスタンスを連想配列の要素のスタックにプッシュする
                _objectPools[key].Push(obj);
                obj.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// プールからGameObjectを取りだす
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public PoolObject Get(string key, Vector2 spawnPlace, Quaternion spawnDir)
    {
        PoolObject obj;

        // スタックの中身が空のとき、新たに生成しスタックに加える
        if (_objectPools[key].Count == 0)
        {
            // 受け取ったキーからリストのインデックスを取得
            int poolDataIndex = _poolIndices[_objectPools[key]];
            obj = Instantiate(_poolDataList[poolDataIndex]._prefab);
        }
        // 受け取ったキーからスタックを特定し、取り出す
        else
        {
            obj = _objectPools[key].Pop();
        }

        obj.Enable(spawnPlace, spawnDir);
        return obj;
    }

    /// <summary>
    /// プールにGameObjectを戻す
    /// </summary>
    /// <param name="obj"></param>
    public void Return(PoolObject obj)
    {
        obj.Disable();

        // 受け取ったオブジェクトからキーを算出（Clone表記の削除）
        string key = Wrapper.OriginalizeTheName(obj.name);
        _objectPools[key].Push(obj);
    }
}
