using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public abstract class Weapon : MonoBehaviour
{
    protected Transform _transform = default;
    protected WeaponType _myWeaponType;
    [Tooltip("�`���[�W���ꂽ����")]
    protected float _chargedTime_s = 0f;

    [SerializeField, Tooltip("�ő�`���[�W����"), Min(0)]
    private float _maxChargeTime_s = default;

    private GameInputs _gameInputs = default;
    [Tooltip("�`���[�W���Ȃ�true")]
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
    /// �`���[�W���Ԃ���`���[�W�i�K���Z�o
    /// <br>��F�ő�`���[�W���ԁi_maxChargeTime_s�j��3s�̂Ƃ��A0�`1.5s�́uS�v�A1.5�`3s�́uM�v�A3s�ȏ�́uL�v</br>
    /// </summary>
    /// <returns></returns>
    private ChargeStage ChargeStageCalculation()
    {
        // �ő�`���[�W���Ԉȏ�̂Ƃ��uL�v
        if (_chargedTime_s >= _maxChargeTime_s)
        {
            return ChargeStage.L;
        }
        // �ő�`���[�W���Ԃ̔����ɖ����Ȃ��Ƃ��uS�v
        else if (_chargedTime_s < _maxChargeTime_s / 2)
        {
            return ChargeStage.S;
        }
        // �����̒��Ԃ̂Ƃ��uM�v
        else
        {
            return ChargeStage.M;
        }
    }

    /// <summary>
    /// �ˌ����̏���
    /// </summary>
    /// <param name="chargeType">�`���[�W�i�K</param>
    protected abstract void Fire(ChargeStage chargeType);
}
