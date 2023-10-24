using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Transform _transform = default;
    private GameInputs _gameInputs = default;
    private IGroundable _groundChecker = default;

    #region Run�ϐ�
    [Header("Run�ϐ�")]
    [SerializeField, Tooltip("�v���C���[�̊�{���x")]
    private float _standardSpeed = default;
    [SerializeField, Tooltip("���̓z�[���h���̑��x�����l")]
    private float _speedIncreaseValue = default;
    [Tooltip("�������̑��x")]
    private float _increasedSpeed = default;
    [Tooltip("�ړ������i���E�j")]
    private Vector2 _moveDir = default;
    [Tooltip("�ړ����͂������true��Ԃ�")]
    private bool _isMove = default;

    [Tooltip("���E���x�̌W���i��{���x�Ɋ|�����l�����E�l�j")]
    private const float SPEED_LIMIT_COEFFICIENT = 3.0f;
    #endregion

    #region Jumo�ϐ�
    [Header("Jump�ϐ�")]
    [SerializeField, Tooltip("�v���C���[�̊�{�W�����v��")]
    private float _standardJumpPower = default;
    [SerializeField, Tooltip("�d�͉����x")]
    private float _gravityAcceleration = default;

    // �W�����v�̑O���㔼�ŃW�����v�͂̑����ʂ𕪂���--------------
    // => �����������A�󒆂Ō�������悤�ȃW�����v
    //[SerializeField, Tooltip("�O���̃W�����v�͑����l")]
    //private float _jumpPowerIncreaseValue_AtFirst = default;
    //[SerializeField, Tooltip("�㔼�̃W�����v�͑����l")]
    //private float _jumpPowerIncreaseValue_AtLast = default;
    // ------------------------------------------------------------

    [Tooltip("�������̃W�����v��")]
    private float _currentJumpPower = default;
    [Tooltip("�n�ʂɒ����Ă����true��Ԃ�")]
    private bool _isGrounded = default;
    [Tooltip("�W�����v���s���̊�true��Ԃ�")]
    private bool _isJump = default;

    [Tooltip("���E�W�����v�͂̌W���i��{�W�����v�͂Ɋ|�����l�����E�l�j")]
    private const float JUMP_LIMIT_COEFFICIENT = 3.0f;
    #endregion


    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;

        _gameInputs = new();
        _gameInputs.Enable();

        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnStop;
        _gameInputs.Player.Jump.started += OnJump;
        _gameInputs.Player.Jump.canceled += OnCancelJump;

        _groundChecker = new RayCaster(direction: Vector2.down, distance: 0.6f);
    }

    private void Update()
    {
        Vector2 moveDelta = Run() + Jump();
        _transform.Translate(moveDelta * Time.deltaTime);
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
        _currentJumpPower = _standardJumpPower;
        _isJump = true;
    }

    private void OnCancelJump(InputAction.CallbackContext context)
    {

    }

    /// <summary>
    /// �ړ������̃x�N�g�����Z�o
    /// <br>- ���̓z�[���h�ɉ����đ��x�𑝉�������</br>
    /// </summary>
    public Vector2 Run()
    {
        if (!_isMove) { return Vector2.zero; }

        float speed = CalculateSpeed(ref _increasedSpeed);
        return _moveDir * speed;
    }

    /// <summary>
    /// �W�����v����
    /// <br>- �{�^���̓��͎��Ԃɉ����āu��W�����v�v�Ɓu���W�����v�v�ɕ��򂷂�</br>
    /// </summary>
    public Vector2 Jump()
    {
        bool isGrounded = _groundChecker.CheckGround(origin: _transform.position);
        if (_isJump && isGrounded) { return Vector2.zero; }

        _currentJumpPower -= _gravityAcceleration;
        return Vector2.up * _currentJumpPower;
    }

    /// <summary>
    /// �ړ����x�̎Z�o
    /// </summary>
    /// <param name="previousSpeed">1�t���[���O�̑��������x�i��{���x���������l�j</param>
    /// <returns></returns>
    private float CalculateSpeed(ref float previousSpeed)
    {
        float currentSpeed = _standardSpeed + previousSpeed;
        float limitSpeed = _standardSpeed * SPEED_LIMIT_COEFFICIENT;

        // ���݂̑��x�����E�l��������Ă���ԁA���x�𑝉�������
        if (currentSpeed < limitSpeed) { previousSpeed += _speedIncreaseValue; }

        // �Z�o�����ŏI���x�i��{���x + �������x�j
        return _standardSpeed + previousSpeed;
    }

    /// <summary>
    /// �W�����v�ʂ̎Z�o
    /// </summary>
    /// <param name="previousPower">1�t���[���O�̃W�����v��</param>
    /// <returns></returns>
    private float CalculateJumpPower(ref float previousPower)
    {
        _currentJumpPower -= _gravityAcceleration;

        return _currentJumpPower;

        //float currentPower = _standardJumpPower + previousPower;
        //float limitPower = _standardJumpPower * JUMP_LIMIT_COEFFICIENT;

        //// ���݂̃W�����v�͂����E�l��������Ă���ԁA�W�����v�͂𑝉�������
        //if (currentPower < limitPower) { previousPower += _jumpPowerIncreaseValue_AtFirst; }

        //// �Z�o�����ŏI�W�����v�́i��{�W�����v�� + �����W�����v�́j
        //return _standardJumpPower + previousPower;
    }
}
