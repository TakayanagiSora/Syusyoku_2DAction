using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public abstract class Weapon : MonoBehaviour
{
    protected Transform _transform = default;
    protected WeaponType _myWeaponType;
    protected float _chargeTime_s = 0f;

    private GameInputs _gameInputs = default;
    private bool _isCharging = false;


    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;

        _gameInputs = new GameInputs();
        _gameInputs.Enable();

        _gameInputs.Player.Fire.started += OnCharge;
        _gameInputs.Player.Fire.canceled += OnFire;
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }

    /// <summary>
    /// éÀåÇéûÇÃèàóù
    /// </summary>
    protected abstract void Fire();

    private async void OnCharge(InputAction.CallbackContext context)
    {
        _isCharging = true;
        await ChargeAsync();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _isCharging = false;
        Fire();

        _chargeTime_s = 0f;
    }

    private async UniTask ChargeAsync()
    {
        while (_isCharging)
        {
            _chargeTime_s += Time.deltaTime;
            await UniTask.Yield();
        }
    }
}
