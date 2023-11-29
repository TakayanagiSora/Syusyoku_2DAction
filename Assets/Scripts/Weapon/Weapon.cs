using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UniRx;

public abstract class Weapon : MonoBehaviour
{
    protected Transform _transform = default;
    protected PoolController _poolController = default;
    protected WeaponType _myWeaponType;
    [Tooltip("チャージされた時間")]
    protected float _chargedTime_s = 0f;
    protected ChargeLevel _chargeLevel = ChargeLevel.S;

    [SerializeField, Tooltip("最大チャージ時間"), Min(0)]
    private float _maxChargeTime_s = default;

    private GameInputs _gameInputs = default;

    [Tooltip("チャージ中ならtrue")]
    private bool _isCharging = false;
    private OnlyOnce _onlyOnce_ChargeM = new OnlyOnce();
    private OnlyOnce _onlyOnce_ChargeL = new OnlyOnce();

    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;
        _poolController = FindObjectOfType<PoolController>();

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
        _chargeLevel = ChargeLevel.S;
        _isCharging = true;
        await ChargeAsync();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _isCharging = false;
        Fire(_chargeLevel);

        _chargedTime_s = 0f;
        _onlyOnce_ChargeM.Reset();
        _onlyOnce_ChargeL.Reset();
    }

    private async UniTask ChargeAsync()
    {
        while (_isCharging)
        {
            _chargedTime_s += Time.deltaTime;
            ChargeLevelCalculation();

            await UniTask.Yield();
        }
    }

    /// <summary>
    /// チャージ時間からチャージ段階を算出
    /// <br>例：最大チャージ時間（_maxChargeTime_s）が3sのとき、0～1.5sは「S」、1.5～3sは「M」、3s以上は「L」</br>
    /// </summary>
    /// <returns></returns>
    private void ChargeLevelCalculation()
    {
        // (最大チャージ時間の半分 <= チャージ時間 < 最大チャージ時間)のとき「M」
        // (1.5s <= チャージ時間 < 3s)
        if (_chargedTime_s >= _maxChargeTime_s / 2 && _chargedTime_s < _maxChargeTime_s)
        {
            _onlyOnce_ChargeM.Execution(() => _chargeLevel = ChargeLevel.M);
        }
        // 最大チャージ時間以上のとき「L」
        else if (_chargedTime_s >= _maxChargeTime_s)
        {
            _onlyOnce_ChargeL.Execution(() => _chargeLevel = ChargeLevel.L);
        }
    }

    /// <summary>
    /// 射撃時の処理
    /// </summary>
    /// <param name="chargeType">チャージ段階</param>
    protected abstract void Fire(ChargeLevel chargeType);
}
