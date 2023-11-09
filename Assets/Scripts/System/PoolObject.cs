using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プール化対象オブジェクトの基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PoolObject<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("プール化オブジェクト")]
    private static T _originObject = default;
    private static Stack<T> _objectPool = new Stack<T>();
    [Tooltip("初期生成済みであればtrue")]
    private static bool _isCreated = false;

    /// <summary>
    /// プール化オブジェクトのプレハブを設定
    /// </summary>
    public static T SetOrigin
    {
        set
        {
            if (_originObject is not null)
            {
                Debug.LogWarning("既にプールに登録されています");
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
    /// 初期生成
    /// </summary>
    /// <param name="maxAmount">プール容量</param>
    public static void InitialCreate(int maxAmount)
    {
        if (_isCreated)
        {
            Debug.LogWarning("既に生成されています");
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
    /// プールから取り出す
    /// </summary>
    /// <param name="spawnPos">出現位置</param>
    /// <param name="spawnDir">出現角度</param>
    /// <returns></returns>
    public static T Get(Vector2 spawnPos, Quaternion spawnDir)
    {
        T obj;

        // スタックが空でなければポップ、空であれば追加生成
        if (_objectPool.Count > 0)
        {
            obj = _objectPool.Pop();
        }
        else
        {
            Debug.LogWarning("プールの容量が足りていません");
            obj = Instantiate(_originObject);
        }

        // 初期化処理を呼び出す
        (obj as PoolObject<T>).Enable(spawnPos, spawnDir);
        return obj;
    }

    /// <summary>
    /// プールに返す
    /// </summary>
    /// <param name="obj">返却対象オブジェクト</param>
    public static void Return(T obj)
    {
        (obj as PoolObject<T>).Disable();
        _objectPool.Push(obj);
    }

    /// <summary>
    /// オブジェクト破棄時に呼び出される初期化処理
    /// <br>例：シーン遷移時</br>
    /// </summary>
    private void Clear()
    {
        _originObject = null;
        _objectPool.Clear();
    }

    /// <summary>
    /// Get時の初期化処理
    /// <br>- SetActiveも記述すること</br>
    /// </summary>
    /// <param name="spawnPos">出現位置</param>
    /// <param name="spawnDir">出現角度</param>
    protected abstract void Enable(Vector2 spawnPos, Quaternion spawnDir);
    /// <summary>
    /// Return時の初期化処理
    /// <br>- SetActiveも記述すること</br>
    /// </summary>
    protected abstract void Disable();
}
