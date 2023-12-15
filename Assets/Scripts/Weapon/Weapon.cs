using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UniRx;

public abstract class Weapon : MonoBehaviour
{
    protected Transform _transform = default;
    protected PoolController _poolController = default;
    protected WeaponType _myWeaponType;
    [Tooltip("�`���[�W���ꂽ����")]
    protected float _chargedTime_s = 0f;

    protected ReactiveProperty<ChargeLevel> _chargeLevel = new ReactiveProperty<ChargeLevel>();
    public IReadOnlyReactiveProperty<ChargeLevel> ChargeLevel => _chargeLevel;

    [SerializeField, Tooltip("�ő�`���[�W����"), Min(0)]
    private float _maxChargeTime_s = default;

    private GameInputs _gameInputs = default;

    [Tooltip("�`���[�W���Ȃ�true")]
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

        // ������
        _chargeLevel.Value = global::ChargeLevel.None;
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }


    private async void OnCharge(InputAction.CallbackContext context)
    {
        _chargeLevel.Value = global::ChargeLevel.S;
        _isCharging = true;
        await ChargeAsync();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _isCharging = false;
        Fire(_chargeLevel.Value);

        // ������
        _chargedTime_s = 0f;
        _chargeLevel.Value = global::ChargeLevel.None;
        _onlyOnce_ChargeM.Reset();
        _onlyOnce_ChargeL.Reset();
    }

    /// <summary>
    /// �񓯊��Ń`���[�W�𔻒�
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
    /// �`���[�W���Ԃ���`���[�W�i�K���Z�o
    /// <br>��F�ő�`���[�W���ԁi_maxChargeTime_s�j��3s�̂Ƃ��A0�`1.5s�́uS�v�A1.5�`3s�́uM�v�A3s�ȏ�́uL�v</br>
    /// </summary>
    /// <returns></returns>
    private void ChargeLevelCalculation()
    {
        // (�ő�`���[�W���Ԃ̔��� <= �`���[�W���� < �ő�`���[�W����)�̂Ƃ��uM�v
        // (1.5s <= �`���[�W���� < 3s)
        if (_chargedTime_s >= _maxChargeTime_s / 2 && _chargedTime_s < _maxChargeTime_s)
        {
            _onlyOnce_ChargeM.Execution(() => _chargeLevel.Value = global::ChargeLevel.M);
        }
        // �ő�`���[�W���Ԉȏ�̂Ƃ��uL�v
        else if (_chargedTime_s >= _maxChargeTime_s)
        {
            _onlyOnce_ChargeL.Execution(() => _chargeLevel.Value = global::ChargeLevel.L);
        }
    }

    /// <summary>
    /// �ˌ����̏���
    /// </summary>
    /// <param name="chargeType">�`���[�W�i�K</param>
    protected abstract void Fire(ChargeLevel chargeType);
}
