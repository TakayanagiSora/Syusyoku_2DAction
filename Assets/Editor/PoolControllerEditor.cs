using UnityEditor;

/// <summary>
/// プール管理者のInspectorを監視するEditorクラス
/// </summary>
[CustomEditor(typeof(PoolController))]
public class PoolControllerEditor : Editor
{
    private PoolUserDrawer _drawer = new PoolUserDrawer();

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // この内容にInspector上で変更があった場合、EndChangeCheck()がtrueを返す
        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            // targetを拡張対象クラスにキャスト
            if (target is PoolController poolController)
            {
                // 取得したリストを外部のPropertyDrawerに投げる
                _drawer.UpdateSelectableList(poolController.PoolDataList);
            }
        }
    }
}