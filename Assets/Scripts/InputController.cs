using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UniRx;

public class InputController : MonoBehaviour
{
    private Keyboard _keyboard = default;

    private Subject<Unit> _onSpaceKeySubject = new();
    private KeyControl _spaceKey = default;

    private void Awake()
    {
        #region キーボードの初期化処理
        _keyboard = Keyboard.current;
        if (_keyboard is null) Debug.LogWarning("キーボードが接続されていません");

        _spaceKey = _keyboard.spaceKey;
        #endregion
    }

    private void Update()
    {
        // スペースキーを押されたとき
        if (_spaceKey.wasPressedThisFrame) _onSpaceKeySubject.OnNext(Unit.Default);
    }
}
