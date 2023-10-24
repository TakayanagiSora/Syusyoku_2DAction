using UnityEngine;

public interface IRaycast
{
    /// <summary>
    /// 対象がGameObjectに接しているとき、trueを返す
    /// </summary>
    /// <param name="origin">Rayの原点</param>
    /// <returns></returns>
    bool Check(Vector2 origin);

    /// <summary>
    /// 対象が設定したLayerのGameObjectに接しているとき、trueを返す
    /// </summary>
    /// <param name="origin">Rayの原点</param>
    /// <param name="layerMask">フィルタリングするLayerのマスク値</param>
    /// <returns></returns>
    bool Check(Vector2 origin, int layerMask);
}

public interface IGroundable
{
    /// <summary>
    /// 対象がGroundLayerのGameObjectに接しているとき、trueを返す
    /// </summary>
    /// <param name="origin">Rayの原点</param>
    /// <returns></returns>
    bool CheckGround(Vector2 origin);
}
