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
        #region �L�[�{�[�h�̏���������
        _keyboard = Keyboard.current;
        if (_keyboard is null) Debug.LogWarning("�L�[�{�[�h���ڑ�����Ă��܂���");

        _spaceKey = _keyboard.spaceKey;
        #endregion
    }

    private void Update()
    {
        // �X�y�[�X�L�[�������ꂽ�Ƃ�
        if (_spaceKey.wasPressedThisFrame) _onSpaceKeySubject.OnNext(Unit.Default);
    }
}
