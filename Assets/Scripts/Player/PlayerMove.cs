using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class PlayerMove : MonoBehaviour
{
    private Transform _transform = default;
    private GameInputs _gameInputs = default;
    private GroundChecker _groundChecker;
    private OnlyOnce _onlyOnce = new OnlyOnce();

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

    #region Jump変数
    [Header("Jump変数")]
    [SerializeField, Tooltip("プレイヤーの基本ジャンプ力")]
    private float _standardJumpPower = default;
    [SerializeField, Tooltip("重力加速度")]
    private float _gravityAcceleration_BySecond = default;
    [SerializeField, Tooltip("重力上昇係数")]
    private float _gravityCoefficient = default;
    [Tooltip("現在の重力")]
    private float _currentGravity = default;

    [Tooltip("上下方向の速度")]
    private float _ySpeed = default;
    [Tooltip("ジャンプ入力があったらtrueを返す")]
    private bool _isJumpInput = default;
    #endregion


    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;

        #region InputSystemのイベント購読
        _gameInputs = new GameInputs();
        _gameInputs.Enable();

        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnStop;
        _gameInputs.Player.Jump.started += OnJump;
        #endregion

        #region GroundCheckerのイベント購読
        _groundChecker = new GroundChecker(direction: Vector2.down, distance: 0.1f);
        // IsGroundedがtrueに変わったとき = 着地したとき
        _groundChecker.IsGrounded.Where(value => value).Subscribe(value => FinishedJump());
        #endregion
    }

    private void Start()
    {
        _currentGravity = _gravityAcceleration_BySecond;
    }

    private void Update()
    {
        // 中心座標が(0, 0)、LocalScaleが3のときは(0, -1.5)を原点とする
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
    /// 移動方向のベクトルを算出
    /// <br>- 入力ホールドに応じて速度を増加させる</br>
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
    /// 上下方向のベクトルを算出
    /// </summary>
    public Vector2 Jump()
    {
        // 地面にいる
        if (_groundChecker.IsGrounded.Value)
        {
            // ジャンプ入力がある
            if (_isJumpInput)
            {
                _ySpeed = _standardJumpPower;
                _currentGravity = _gravityAcceleration_BySecond;
            }
        }
        // 空中にいる
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
        // 着地判定を取ったら止まる
        _ySpeed = 0f;
        _onlyOnce.Reset();
        _isJumpInput = false;
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
        if (currentSpeed < limitSpeed)
        {
            previousSpeed += _speedIncreaseValue;
        }

        // 算出した最終速度（基本速度 + 増加速度）
        return _standardSpeed + previousSpeed;
    }
}
