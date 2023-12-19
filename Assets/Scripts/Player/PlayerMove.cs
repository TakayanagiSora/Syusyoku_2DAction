using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    private Transform _transform = default;
    private GameInputs _gameInputs = default;
    private GroundChecker _groundChecker;
    private OnlyOnce _onlyOnce = new OnlyOnce();

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

    #region Jump�ϐ�
    [Header("Jump�ϐ�")]
    [SerializeField, Tooltip("�v���C���[�̊�{�W�����v��")]
    private float _standardJumpPower = default;
    [SerializeField, Tooltip("�d�͉����x")]
    private float _gravityAcceleration_BySecond = default;
    [SerializeField, Tooltip("�d�͏㏸�W��")]
    private float _gravityCoefficient = default;
    [Tooltip("���݂̏d��")]
    private float _currentGravity = default;

    [Tooltip("�㉺�����̑��x")]
    private float _ySpeed = default;
    [Tooltip("�W�����v���͂���������true��Ԃ�")]
    private bool _isJumpInput = default;
    #endregion


    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;

        #region InputSystem�̃C�x���g�w��
        _gameInputs = new GameInputs();
        _gameInputs.Enable();

        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnStop;
        _gameInputs.Player.Jump.started += OnJump;
        #endregion

        #region GroundChecker�̃C�x���g�w��
        _groundChecker = new GroundChecker(direction: Vector2.down, distance: 0.1f);
        // IsGrounded��true�ɕς�����Ƃ� = ���n�����Ƃ�
        _groundChecker.IsGrounded.Where(value => value).Subscribe(value => FinishedJump());
        #endregion
    }

    private void Start()
    {
        _currentGravity = _gravityAcceleration_BySecond;
    }

    private void Update()
    {
        // ���S���W��(0, 0)�ALocalScale��3�̂Ƃ���(0, -1.5)�����_�Ƃ���
        Vector2 origin = new Vector2(_transform.position.x, _transform.position.y - _transform.localScale.z / 2);
        _groundChecker.Check(origin);

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
        if (!_isMove)
        {
            return Vector2.zero;
        }

        float speed = CalculateSpeed(ref _increasedSpeed);
        return _moveDir * speed;
    }

    /// <summary>
    /// �㉺�����̃x�N�g�����Z�o
    /// </summary>
    public Vector2 Jump()
    {
        // �n�ʂɂ���
        if (_groundChecker.IsGrounded.Value)
        {
            // �W�����v���͂�����
            if (_isJumpInput)
            {
                _ySpeed = _standardJumpPower;
                _currentGravity = _gravityAcceleration_BySecond;
            }
        }
        // �󒆂ɂ���
        else
        {
            _ySpeed -= _currentGravity * Time.deltaTime;

            if (_ySpeed <= 0f)
            {
                _onlyOnce.Execution(() => _currentGravity *= _gravityCoefficient);
            }
        }

        return Vector2.up * _ySpeed;
    }

    private void FinishedJump()
    {
        // ���n������������~�܂�
        _ySpeed = 0f;
        _onlyOnce.Reset();
        _isJumpInput = false;
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
        if (currentSpeed < limitSpeed)
        {
            previousSpeed += _speedIncreaseValue;
        }

        // �Z�o�����ŏI���x�i��{���x + �������x�j
        return _standardSpeed + previousSpeed;
    }
}
