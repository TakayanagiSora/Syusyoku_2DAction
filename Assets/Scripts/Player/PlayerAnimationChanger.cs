using UnityEngine;

public class PlayerAnimationChanger : MonoBehaviour
{
    private GameInputs _gameInputs = default;

    private readonly int _runHash = Animator.StringToHash("Run");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _chargeHash = Animator.StringToHash("Charge");
    private readonly int _shotHash = Animator.StringToHash("Shot");

    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        Animator animator = this.GetComponent<Animator>();

        _gameInputs = new GameInputs();
        _gameInputs.Enable();
        _gameInputs.Player.Move.started += context => animator.SetBool(_runHash, true);
        _gameInputs.Player.Move.canceled += context => animator.SetBool(_runHash, false);
        _gameInputs.Player.Jump.started += conatext => animator.SetTrigger(_jumpHash);
        _gameInputs.Player.Fire.started += conatext => animator.SetTrigger(_chargeHash);
        _gameInputs.Player.Fire.canceled += conatext => animator.SetTrigger(_shotHash);
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }
}
