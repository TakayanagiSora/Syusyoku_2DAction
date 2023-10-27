using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

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

    [Tooltip("�㉺�����̑��x")]
    private float _ySpeed = default;
    [Tooltip("�n�ʂɒ����Ă����true��Ԃ�")]
    private bool _isGrounded = default;
    [Tooltip("�W�����v���s���̊�true��Ԃ�")]
    private bool _isJumping = default;
    [Tooltip("�W�����v���͂���������true��Ԃ�")]
    private bool _isJumpInput = default;

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
        _isJumpInput = true;
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

        // �n�ʂɂ���
        if (isGrounded)
        {
            // �W�����v���Ă��Ȃ�
            if (!_isJumping)
            {
                _ySpeed = 0f;

                // �W�����v���͂�����
                if (_isJumpInput)
                {
                    _ySpeed = _standardJumpPower;
                    _isJumping = true;
                }
            }
        }
        // �󒆂ɂ���
        else
        {
            _ySpeed -= _gravityAcceleration;
            _isJumping = false;
            _isJumpInput = false;
        }

        return Vector2.up * _ySpeed;
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
}
