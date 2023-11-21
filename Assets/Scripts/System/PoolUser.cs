using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class PoolUser : MonoBehaviour
{
    [SerializeField]
    private List<UsePoolObject> _usePoolObjectNames = new List<UsePoolObject>();
    protected List<UsePoolObject> UsePoolObjectNames => _usePoolObjectNames;
}

[Serializable]
public class UsePoolObject
{
    // EditorƒNƒ‰ƒX‚©‚çSet‚³‚ê‚é
    // -------------------------------------------------------------
    [SerializeField]
    private string _name = default;
    [SerializeField]
    private int _arrayIndex = default;
    // -------------------------------------------------------------

    public string Name => _name;
}