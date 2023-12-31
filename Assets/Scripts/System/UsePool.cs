using UnityEngine;
using System;

public class UsePool : MonoBehaviour
{
    [SerializeField]
    private UsePoolObject _key = new UsePoolObject();

    public string Key => _key.Name;
}

[Serializable]
public class UsePoolObject
{
    // EditorクラスからSetされる
    // -------------------------------------------------------------
    [SerializeField]
    private string _name = default;
    [SerializeField]
    private int _arrayIndex = default;
    // -------------------------------------------------------------

    public string Name => _name;
}