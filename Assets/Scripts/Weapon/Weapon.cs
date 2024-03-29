using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UniRx;

/// <summary>
/// 武器の基底システム
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    protected Transform _transform = default;
    protected PoolController _poolController = default;
    protected WeaponType _myWeaponType;
    [Tooltip("チャージされた時間")]
    protected float _chargedTime_s = 0f;

    [SerializeField, Tooltip("最大チャージ時間"), Min(0)]
    private float _maxChargeTimeSec = default;

    private GameInputs _gameInputs = default;

    [Tooltip("チャージ中ならtrue")]
    private bool _isCharging = false;
    private OnlyOnce _onlyOnceChargeMedium = new OnlyOnce();
    private OnlyOnce _onlyOnceChargeLarge = new OnlyOnce();

    private ReactiveProperty<ChargeLevel> _chargeLevel = new ReactiveProperty<ChargeLevel>();
    public IReadOnlyReactiveProperty<ChargeLevel> ChargeLevel => _chargeLevel;

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

        // 初期化
        _chargeLevel.Value = global::ChargeLevel.None;
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }


    private async void OnCharge(InputAction.CallbackContext context)
    {
        _chargeLevel.Value = global::ChargeLevel.Small;
        _isCharging = true;
        await ChargeAsync();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _isCharging = false;
        Fire(_chargeLevel.Value);

        // 初期化
        _chargedTime_s = 0f;
        _chargeLevel.Value = global::ChargeLevel.None;
        _onlyOnceChargeMedium.Reset();
        _onlyOnceChargeLarge.Reset();
    }

    /// <summary>
    /// 非同期でチャージを判定
    /// </summary>
    /// <returns></returns>
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
    /// <br>例：最大チャージ時間（_maxChargeTime_s）が3sのとき、0〜1.5sは「S」、1.5〜3sは「M」、3s以上は「L」</br>
    /// </summary>
    /// <returns></returns>
    private void ChargeLevelCalculation()
    {
        // (最大チャージ時間の半分 <= チャージ時間 < 最大チャージ時間)のとき「M」
        // (1.5s <= チャージ時間 < 3s)
        if (_chargedTime_s >= (_maxChargeTimeSec / 2) && 
            _chargedTime_s < _maxChargeTimeSec)
        {
            _onlyOnceChargeMedium.Execution(
                () => _chargeLevel.Value = global::ChargeLevel.Medium);
        }
        // 最大チャージ時間以上のとき「L」
        else if (_chargedTime_s >= _maxChargeTimeSec)
        {
            _onlyOnceChargeLarge.Execution(
                () => _chargeLevel.Value = global::ChargeLevel.Large);
        }
    }

    /// <summary>
    /// 射撃時の処理
    /// </summary>
    /// <param name="chargeType">チャージ段階</param>
    protected abstract void Fire(ChargeLevel chargeType);
}
