using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using YouYouLibrary.SaveSystem;

/// <summary>
/// �v�[�����p��*�̃N���X�ɁA���p�\�ȃI�u�W�F�N�g�����|�b�v�A�b�v�\������Editor�N���X
/// <br>- ����*�F�v�[�����p�҂Ƃ́AInstantiate�̑����PoolController���I�u�W�F�N�g����肾���N���X�̂��Ƃ�����</br>
/// <br>- ���v���W�F�N�g�̃I�u�W�F�N�g�v�[���́A���p�ł���v�[���I�u�W�F�N�g����ӂɎ��ʂ���string�^�̃L�[���g�p���ADictionary�ɃA�N�Z�X���邱�ƂŎ擾�ł���B���̃N���X�̈Ӌ`�͂����̑�������S�ɍs�����Ƃ��ł���_�ł���</br>
/// </summary>
[CustomPropertyDrawer(typeof(UsePoolObject))]
public class PoolUserDrawer : PropertyDrawer
{
    private SelectablePoolObject _selectablePoolObject = new SelectablePoolObject();
    [Tooltip("�ۑ������f�[�^�̃t�@�C����")]
    private const string ARRAY_FILE_NAME = "PoolObjectNames";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �ۑ�����Ă���f�[�^�����o���i�f�V���A���C�Y�j
        // ------------------------------------------------------------------------------------------------------------------
        // �ڂ������������̂��ߕ׋��������A�����炭Unity�̎d�l��PropertyDrawer���p�������N���X�ł�Serializable��������Ή��̂��߁A
        // �ʃt�@�C���ɏ������ތ`�ŃV���A���C�Y�����Ă���
        _selectablePoolObject = SaveSystem.LoadSave<SelectablePoolObject>(ARRAY_FILE_NAME);
        // ------------------------------------------------------------------------------------------------------------------

        if (_selectablePoolObject._names.Count == 0)
        {
            return;
        }

        // �I���\�ȃv�[���I�u�W�F�N�g���X�g���|�b�v�A�b�v�\�����A�g����̃v���p�e�B�ɑ��
        // ------------------------------------------------------------------------------------------------------------------
        // �I���\�ȃv�[���I�u�W�F�N�g���X�g�́APoolController�N���X��Inspector���ύX���ꂽ�^�C�~���O�ōX�V�����
        var usePoolObjectName = property.FindPropertyRelative("_name");
        var usePoolObjectIndex = property.FindPropertyRelative("_arrayIndex");
        usePoolObjectIndex.intValue = EditorGUI.Popup(position, "UsePoolObjectName", usePoolObjectIndex.intValue, _selectablePoolObject._names.ToArray());

        usePoolObjectName.stringValue = _selectablePoolObject._names[usePoolObjectIndex.intValue];
        // ------------------------------------------------------------------------------------------------------------------
    }

    /// <summary>
    /// �I���\�ȃv�[���I�u�W�F�N�g���X�g���X�V����
    /// <br>- PoolController�N���X��Inspector���ύX���ꂽ�^�C�~���O�ōX�V����</br>
    /// </summary>
    /// <param name="list">�I���\�ȃv�[���I�u�W�F�N�g���i�[���ꂽ���X�g</param>
    public void UpdateSelectableList(List<PoolController.PoolData> list)
    {
        // ���݂̃��X�g��������
        _selectablePoolObject._names.Clear();

        // �n���ꂽ���X�g����GameObject�́A�I�u�W�F�N�g�������N���X�̃��X�g�Ɋi�[����
        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                _selectablePoolObject._names.Add(list[i]._prefab.name);
            }
            catch (NullReferenceException)
            {
                _selectablePoolObject._names.Add("[NULL]");
            }
        }

        // �n�����N���X��Json�`���ŕۑ�����i�V���A���C�Y���j
        SaveSystem.Save(_selectablePoolObject, ARRAY_FILE_NAME);
    }
}

/// <summary>
/// ���p����I�u�W�F�N�g���X�g��ۑ�����N���X
/// </summary>
public class SelectablePoolObject
{
    public List<string> _names = new List<string>();
}
