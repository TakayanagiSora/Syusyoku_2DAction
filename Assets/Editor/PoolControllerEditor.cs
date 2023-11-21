using UnityEditor;

/// <summary>
/// �v�[���Ǘ��҂�Inspector���Ď�����Editor�N���X
/// </summary>
[CustomEditor(typeof(PoolController))]
public class PoolControllerEditor : Editor
{
    private PoolUserDrawer _drawer = new PoolUserDrawer();

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // ���̓��e��Inspector��ŕύX���������ꍇ�AEndChangeCheck()��true��Ԃ�
        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            // target���g���ΏۃN���X�ɃL���X�g
            if (target is PoolController poolController)
            {
                // �擾�������X�g���O����PropertyDrawer�ɓ�����
                _drawer.UpdateSelectableList(poolController.PoolDataList);
            }
        }
    }
}