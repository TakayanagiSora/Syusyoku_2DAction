using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public abstract class Weapon : MonoBehaviour
{
    protected Transform _transform = default;
    protected WeaponType _myWeaponType;
    [Tooltip("チャージされた時間")]
    protected float _chargedTime_s = 0f;

    [SerializeField, Tooltip("最大チャージ時間"), Min(0)]
    private float _maxChargeTime_s = default;

    private GameInputs _gameInputs = default;
    [Tooltip("チャージ中ならtrue")]
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


    private async void OnCharge(InputAction.CallbackContext context)
    {
        _isCharging = true;
        await ChargeAsync();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _isCharging = false;
        Fire(ChargeStageCalculation());

        _chargedTime_s = 0f;
    }

    private async UniTask ChargeAsync()
    {
        while (_isCharging)
        {
            _chargedTime_s += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// チャージ時間からチャージ段階を算出
    /// <br>例：最大チャージ時間（_maxChargeTime_s）が3sのとき、0～1.5sは「S」、1.5～3sは「M」、3s以上は「L」</br>
    /// </summary>
    /// <returns></returns>
    private ChargeStage ChargeStageCalculation()
    {
        // 最大チャージ時間以上のとき「L」
        if (_chargedTime_s >= _maxChargeTime_s)
        {
            return ChargeStage.L;
        }
        // 最大チャージ時間の半分に満たないとき「S」
        else if (_chargedTime_s < _maxChargeTime_s / 2)
        {
            return ChargeStage.S;
        }
        // それらの中間のとき「M」
        else
        {
            return ChargeStage.M;
        }
    }

    /// <summary>
    /// 射撃時の処理
    /// </summary>
    /// <param name="chargeType">チャージ段階</param>
    protected abstract void Fire(ChargeStage chargeType);
}
