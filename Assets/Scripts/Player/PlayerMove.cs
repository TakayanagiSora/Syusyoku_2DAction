using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Transform _transform = default;
    private GameInputs _gameInputs = default;

    #region Move�ϐ�
    [Tooltip("�v���C���[�̊�{���x")]
    private const float STANDARD_SPEED = 2.0f;
    [Tooltip("���̓z�[���h���̑��x�����l")]
    private const float SPEED_INCREASE_VALUE = 0.1f;
    [Tooltip("���E���x�̌W���i��{���x�Ɋ|�����l�����E�l�j")]
    private const float SPEED_LIMIT_COEFFICIENT = 3.0f;

    [Tooltip("�������̑��x")]
    private float _increasedSpeed = default;
    [Tooltip("�ړ������i���E�j")]
    private Vector2 _moveDir = default;
    [Tooltip("�ړ����͂������true��Ԃ�")]
    private bool _isMove = default;
    #endregion

    #region Jumo�ϐ�
    [Tooltip("�v���C���[�̊�{�W�����v��")]
    private const float STANDARD_JUMP_POWER = 5.0f;

    // �W�����v�̑O���㔼�ŃW�����v�͂̑����ʂ𕪂���--------------
    // => �����������A�󒆂Ō�������悤�ȃW�����v
    [Tooltip("�O���̃W�����v�͑����l")]
    private const float JUMP_POWER_INCREASE_VALUE_AT_FIRST = 0.2f;
    [Tooltip("�㔼�̃W�����v�͑����l")]
    private const float JUMP_POWER_INCREASE_VALUE_AT_LAST = 0.5f;
    [Tooltip("���E�W�����v�͂̌W���i��{�W�����v�͂Ɋ|�����l�����E�l�j")]
    private const float JUMP_LIMIT_COEFFICIENT = 3.0f;
    // ------------------------------------------------------------

    [Tooltip("�������̃W�����v��")]
    private float _increaseJumpPower = default;
    [Tooltip("�W�����v���s���̊�true��Ԃ�")]
    private bool _isJump = default;
    #endregion


    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;

        _gameInputs = new();
        _gameInputs?.Enable();

        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnStop;
        _gameInputs.Player.Jump.started += OnJump;
        _gameInputs.Player.Jump.canceled += OnCancelJump;
    }

    private void Update()
    {
        if (_isMove) { Run(); }
        if (_isJump) { Jump(); }
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        _isMove = true;
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        _increasedSpeed = 0f;
        _moveDir = Vector2.zero;
        _isMove = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _isJump = true;
    }

    private void OnCancelJump(InputAction.CallbackContext context)
    {
        _isJump = false;
    }

    /// <summary>
    /// �ړ�����
    /// <br>- ���̓z�[���h�ɉ����đ��x�𑝉�������</br>
    /// </summary>
    public void Run()
    {
        float speed = CalculateSpeed(ref _increasedSpeed);
        _transform.Translate(_moveDir * speed);
    }

    /// <summary>
    /// �W�����v����
    /// <br>- �{�^���̓��͎��Ԃɉ����āu��W�����v�v�Ɓu���W�����v�v�ɕ��򂷂�</br>
    /// </summary>
    public void Jump()
    {
        float power = CalculateJumpPower(ref _increaseJumpPower);
        _transform.Translate(Vector2.up * power);
    }

    /// <summary>
    /// �ړ����x�̎Z�o
    /// </summary>
    /// <param name="previousSpeed">1�t���[���O�̑��������x�i��{���x���������l�j</param>
    /// <returns></returns>
    private float CalculateSpeed(ref float previousSpeed)
    {
        float currentSpeed = STANDARD_SPEED + previousSpeed;
        float limitSpeed = STANDARD_SPEED * SPEED_LIMIT_COEFFICIENT;

        // ���݂̑��x�����E�l��������Ă���ԁA���x�𑝉�������
        if (currentSpeed < limitSpeed) { previousSpeed += SPEED_INCREASE_VALUE; }

        // �Z�o�����ŏI���x�i��{���x + �������x�j
        return (STANDARD_SPEED + previousSpeed) * Time.deltaTime;
    }

    /// <summary>
    /// �W�����v�ʂ̎Z�o
    /// </summary>
    /// <param name="previousPower">1�t���[���O�̃W�����v��</param>
    /// <returns></returns>
    private float CalculateJumpPower(ref float previousPower)
    {
        float currentPower = STANDARD_JUMP_POWER + previousPower;
        float limitPower = STANDARD_JUMP_POWER * JUMP_LIMIT_COEFFICIENT;

        // ���݂̃W�����v�͂����E�l��������Ă���ԁA�W�����v�͂𑝉�������
        if (currentPower < limitPower) { previousPower += JUMP_POWER_INCREASE_VALUE_AT_FIRST; }

        // �Z�o�����ŏI�W�����v�́i��{�W�����v�� + �����W�����v�́j
        return (STANDARD_JUMP_POWER + previousPower) * Time.deltaTime;
    }
}
