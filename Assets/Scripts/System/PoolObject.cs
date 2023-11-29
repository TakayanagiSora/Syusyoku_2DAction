using UnityEngine;

/// <summary>
/// プール化対象オブジェクトの基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PoolObject : MonoBehaviour
{
    /// <summary>
    /// Get時の初期化処理
    /// <br>- SetActiveも記述すること</br>
    /// </summary>
    /// <param name="spawnPos">出現位置</param>
    /// <param name="spawnDir">出現角度</param>
    public abstract void Enable(Vector2 spawnPos, Quaternion spawnDir);
    /// <summary>
    /// Return時の初期化処理
    /// <br>- SetActiveも記述すること</br>
    /// </summary>
    public abstract void Disable();
}
