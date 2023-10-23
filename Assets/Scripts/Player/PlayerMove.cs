using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove
{
    #region Move変数
    [Tooltip("プレイヤーの基本速度")]
    private const float STANDARD_SPEED = 2.0f;
    [Tooltip("入力ホールド時の速度増加値")]
    private const float SPEED_INCREASE_VALUE = 0.1f;
    [Tooltip("速度増加の限界係数")]
    private const float INCREASE_LIMIT = 3.0f;

    [Tooltip("増加分の速度")]
    private float _increasedSpeed = default;
    [Tooltip("移動方向（左右）")]
    private Vector2 _moveDir = default;
    [Tooltip("移動入力がある間trueを返す")]
    private bool _isMove = default;
    #endregion


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        _isMove = true;
    }

    public void OnStop(InputAction.CallbackContext context)
    {
        _increasedSpeed = 0f;
        _moveDir = Vector2.zero;
        _isMove = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    /// <summary>
    /// 移動処理
    /// <br>- 入力ホールドに応じて速度を増加させる</br>
    /// </summary>
    /// <param name="transform"></param>
    public void Run(Transform transform)
    {
        if (!_isMove) { return; }

        float speed = CalculateSpeed(ref _increasedSpeed);
        transform.Translate(_moveDir * speed);
    }

    public void Jump(Transform transform)
    {

    }

    /// <summary>
    /// 移動速度の算出
    /// </summary>
    /// <param name="previousSpeed">1フレーム前の増加分速度（基本速度を除いた値）</param>
    /// <returns></returns>
    private float CalculateSpeed(ref float previousSpeed)
    {
        float nowSpeed = STANDARD_SPEED + previousSpeed;
        float limitSpeed = STANDARD_SPEED * INCREASE_LIMIT;

        // 現在の増加速度が限界値を下回っている間、速度を増加させる
        if (nowSpeed < limitSpeed) { previousSpeed += SPEED_INCREASE_VALUE; }

        // 算出した最終速度（基本速度 + 増加速度）
        return (STANDARD_SPEED + previousSpeed) * Time.deltaTime;
    }
}
