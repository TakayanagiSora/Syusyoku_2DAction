using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Transform _transform = default;
    private GameInputs _gameInputs = default;

    #region Move変数
    [Tooltip("プレイヤーの基本速度")]
    private const float STANDARD_SPEED = 2.0f;
    [Tooltip("入力ホールド時の速度増加値")]
    private const float SPEED_INCREASE_VALUE = 0.1f;
    [Tooltip("限界速度の係数（基本速度に掛けた値が限界値）")]
    private const float SPEED_LIMIT_COEFFICIENT = 3.0f;

    [Tooltip("増加分の速度")]
    private float _increasedSpeed = default;
    [Tooltip("移動方向（左右）")]
    private Vector2 _moveDir = default;
    [Tooltip("移動入力がある間trueを返す")]
    private bool _isMove = default;
    #endregion

    #region Jumo変数
    [Tooltip("プレイヤーの基本ジャンプ力")]
    private const float STANDARD_JUMP_POWER = 5.0f;

    // ジャンプの前半後半でジャンプ力の増加量を分ける--------------
    // => 初速が高く、空中で減速するようなジャンプ
    [Tooltip("前半のジャンプ力増加値")]
    private const float JUMP_POWER_INCREASE_VALUE_AT_FIRST = 0.2f;
    [Tooltip("後半のジャンプ力増加値")]
    private const float JUMP_POWER_INCREASE_VALUE_AT_LAST = 0.5f;
    [Tooltip("限界ジャンプ力の係数（基本ジャンプ力に掛けた値が限界値）")]
    private const float JUMP_LIMIT_COEFFICIENT = 3.0f;
    // ------------------------------------------------------------

    [Tooltip("増加分のジャンプ力")]
    private float _increaseJumpPower = default;
    [Tooltip("ジャンプ実行中の間trueを返す")]
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
    /// 移動処理
    /// <br>- 入力ホールドに応じて速度を増加させる</br>
    /// </summary>
    public void Run()
    {
        float speed = CalculateSpeed(ref _increasedSpeed);
        _transform.Translate(_moveDir * speed);
    }

    /// <summary>
    /// ジャンプ処理
    /// <br>- ボタンの入力時間に応じて「大ジャンプ」と「小ジャンプ」に分岐する</br>
    /// </summary>
    public void Jump()
    {
        float power = CalculateJumpPower(ref _increaseJumpPower);
        _transform.Translate(Vector2.up * power);
    }

    /// <summary>
    /// 移動速度の算出
    /// </summary>
    /// <param name="previousSpeed">1フレーム前の増加分速度（基本速度を除いた値）</param>
    /// <returns></returns>
    private float CalculateSpeed(ref float previousSpeed)
    {
        float currentSpeed = STANDARD_SPEED + previousSpeed;
        float limitSpeed = STANDARD_SPEED * SPEED_LIMIT_COEFFICIENT;

        // 現在の速度が限界値を下回っている間、速度を増加させる
        if (currentSpeed < limitSpeed) { previousSpeed += SPEED_INCREASE_VALUE; }

        // 算出した最終速度（基本速度 + 増加速度）
        return (STANDARD_SPEED + previousSpeed) * Time.deltaTime;
    }

    /// <summary>
    /// ジャンプ量の算出
    /// </summary>
    /// <param name="previousPower">1フレーム前のジャンプ量</param>
    /// <returns></returns>
    private float CalculateJumpPower(ref float previousPower)
    {
        float currentPower = STANDARD_JUMP_POWER + previousPower;
        float limitPower = STANDARD_JUMP_POWER * JUMP_LIMIT_COEFFICIENT;

        // 現在のジャンプ力が限界値を下回っている間、ジャンプ力を増加させる
        if (currentPower < limitPower) { previousPower += JUMP_POWER_INCREASE_VALUE_AT_FIRST; }

        // 算出した最終ジャンプ力（基本ジャンプ力 + 増加ジャンプ力）
        return (STANDARD_JUMP_POWER + previousPower) * Time.deltaTime;
    }
}
