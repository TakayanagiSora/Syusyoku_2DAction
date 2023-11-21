using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using YouYouLibrary.SaveSystem;

/// <summary>
/// プール利用者*のクラスに、利用可能なオブジェクト名をポップアップ表示するEditorクラス
/// <br>- 注釈*：プール利用者とは、Instantiateの代わりにPoolControllerよりオブジェクトを取りだすクラスのことをいう</br>
/// <br>- 当プロジェクトのオブジェクトプールは、利用できるプールオブジェクトを一意に識別するstring型のキーを使用し、Dictionaryにアクセスすることで取得できる。このクラスの意義はそれらの操作を安全に行うことができる点である</br>
/// </summary>
[CustomPropertyDrawer(typeof(UsePoolObject))]
public class PoolUserDrawer : PropertyDrawer
{
    private SelectablePoolObject _selectablePoolObject = new SelectablePoolObject();
    [Tooltip("保存したデータのファイル名")]
    private const string ARRAY_FILE_NAME = "PoolObjectNames";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 保存されているデータを取り出す（デシリアライズ）
        // ------------------------------------------------------------------------------------------------------------------
        // 詳しい原因調査のため勉強中だが、おそらくUnityの仕様上PropertyDrawerを継承したクラスでのSerializable属性が非対応のため、
        // 別ファイルに書き込む形でシリアライズ化している
        _selectablePoolObject = SaveSystem.LoadSave<SelectablePoolObject>(ARRAY_FILE_NAME);
        // ------------------------------------------------------------------------------------------------------------------

        if (_selectablePoolObject._names.Count == 0)
        {
            return;
        }

        // 選択可能なプールオブジェクトリストをポップアップ表示し、拡張先のプロパティに代入
        // ------------------------------------------------------------------------------------------------------------------
        // 選択可能なプールオブジェクトリストは、PoolControllerクラスのInspectorが変更されたタイミングで更新される
        var usePoolObjectName = property.FindPropertyRelative("_name");
        var usePoolObjectIndex = property.FindPropertyRelative("_arrayIndex");
        usePoolObjectIndex.intValue = EditorGUI.Popup(position, "UsePoolObjectName", usePoolObjectIndex.intValue, _selectablePoolObject._names.ToArray());

        usePoolObjectName.stringValue = _selectablePoolObject._names[usePoolObjectIndex.intValue];
        // ------------------------------------------------------------------------------------------------------------------
    }

    /// <summary>
    /// 選択可能なプールオブジェクトリストを更新する
    /// <br>- PoolControllerクラスのInspectorが変更されたタイミングで更新する</br>
    /// </summary>
    /// <param name="list">選択可能なプールオブジェクトが格納されたリスト</param>
    public void UpdateSelectableList(List<PoolController.PoolData> list)
    {
        // 現在のリストを初期化
        _selectablePoolObject._names.Clear();

        // 渡されたリスト内のGameObjectの、オブジェクト名を自クラスのリストに格納する
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

        // 渡したクラスをJson形式で保存する（シリアライズ化）
        SaveSystem.Save(_selectablePoolObject, ARRAY_FILE_NAME);
    }
}

/// <summary>
/// 利用するオブジェクトリストを保存するクラス
/// </summary>
public class SelectablePoolObject
{
    public List<string> _names = new List<string>();
}
