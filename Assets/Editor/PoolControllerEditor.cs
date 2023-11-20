using UnityEditor;

[CustomEditor(typeof(PoolController))]
public class PoolControllerEditor : Editor
{
    PoolUserDrawer _drawer = new PoolUserDrawer();

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            if (target is PoolController poolController)
            {
                _drawer.UpdateSelectableList(poolController.PoolDataList);
            }
        }
    }
}