using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class PlayerMove : MonoBehaviour
{
    private Transform _transform = default;
    private GameInputs _gameInputs = default;
    private IGroundable _groundChecker = default;

    #region Run変数
    [Header("Run変数")]
    [SerializeField, Tooltip("プレイヤーの基本速度")]
    private float _standardSpeed = default;
    [SerializeField, Tooltip("入力ホールド時の速度増加値")]
    private float _speedIncreaseValue = default;
    [Tooltip("増加分の速度")]
    private float _increasedSpeed = default;
    [Tooltip("移動方向（左右）")]
    private Vector2 _moveDir = default;
    [Tooltip("移動入力がある間trueを返す")]
    private bool _isMove = default;

    [Tooltip("限界速度の係数（基本速度に掛けた値が限界値）")]
    private const float SPEED_LIMIT_COEFFICIENT = 3.0f;
    #endregion

    #region Jumo変数
    [Header("Jump変数")]
    [SerializeField, Tooltip("プレイヤーの基本ジャンプ力")]
    private float _standardJumpPower = default;
    [SerializeField, Tooltip("重力加速度")]
    private float _gravityAcceleration = default;

    // ジャンプの前半後半でジャンプ力の増加量を分ける--------------
    // => 初速が高く、空中で減速するようなジャンプ
    //[SerializeField, Tooltip("前半のジャンプ力増加値")]
    //private float _jumpPowerIncreaseValue_AtFirst = default;
    //[SerializeField, Tooltip("後半のジャンプ力増加値")]
    //private float _jumpPowerIncreaseValue_AtLast = default;
    // ------------------------------------------------------------

    [Tooltip("上下方向の速度")]
    private float _ySpeed = default;
    [Tooltip("地面に着いている間trueを返す")]
    private bool _isGrounded = default;
    [Tooltip("ジャンプ実行中の間trueを返す")]
    private bool _isJumping = default;
    [Tooltip("ジャンプ入力があったらtrueを返す")]
    private bool _isJumpInput = default;

    [Tooltip("限界ジャンプ力の係数（基本ジャンプ力に掛けた値が限界値）")]
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
    /// 移動方向のベクトルを算出
    /// <br>- 入力ホールドに応じて速度を増加させる</br>
    /// </summary>
    public Vector2 Run()
    {
        if (!_isMove) { return Vector2.zero; }

        float speed = CalculateSpeed(ref _increasedSpeed);
        return _moveDir * speed;
    }

    /// <summary>
    /// ジャンプ処理
    /// <br>- ボタンの入力時間に応じて「大ジャンプ」と「小ジャンプ」に分岐する</br>
    /// </summary>
    public Vector2 Jump()
    {
        bool isGrounded = _groundChecker.CheckGround(origin: _transform.position);

        // 地面にいる
        if (isGrounded)
        {
            // ジャンプしていない
            if (!_isJumping)
            {
                _ySpeed = 0f;

                // ジャンプ入力がある
                if (_isJumpInput)
                {
                    _ySpeed = _standardJumpPower;
                    _isJumping = true;
                }
            }
        }
        // 空中にいる
        else
        {
            _ySpeed -= _gravityAcceleration;
            _isJumping = false;
            _isJumpInput = false;
        }

        return Vector2.up * _ySpeed;
    }

    /// <summary>
    /// 移動速度の算出
    /// </summary>
    /// <param name="previousSpeed">1フレーム前の増加分速度（基本速度を除いた値）</param>
    /// <returns></returns>
    private float CalculateSpeed(ref float previousSpeed)
    {
        float currentSpeed = _standardSpeed + previousSpeed;
        float limitSpeed = _standardSpeed * SPEED_LIMIT_COEFFICIENT;

        // 現在の速度が限界値を下回っている間、速度を増加させる
        if (currentSpeed < limitSpeed) { previousSpeed += _speedIncreaseValue; }

        // 算出した最終速度（基本速度 + 増加速度）
        return _standardSpeed + previousSpeed;
    }
}
