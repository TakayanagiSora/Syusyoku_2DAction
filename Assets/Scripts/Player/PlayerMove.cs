using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove
{
    #region Move�ϐ�
    [Tooltip("�v���C���[�̊�{���x")]
    private const float STANDARD_SPEED = 2.0f;
    [Tooltip("���̓z�[���h���̑��x�����l")]
    private const float SPEED_INCREASE_VALUE = 0.1f;
    [Tooltip("���x�����̌��E�W��")]
    private const float INCREASE_LIMIT = 3.0f;

    [Tooltip("�������̑��x")]
    private float _increasedSpeed = default;
    [Tooltip("�ړ������i���E�j")]
    private Vector2 _moveDir = default;
    [Tooltip("�ړ����͂������true��Ԃ�")]
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
    /// �ړ�����
    /// <br>- ���̓z�[���h�ɉ����đ��x�𑝉�������</br>
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
    /// �ړ����x�̎Z�o
    /// </summary>
    /// <param name="previousSpeed">1�t���[���O�̑��������x�i��{���x���������l�j</param>
    /// <returns></returns>
    private float CalculateSpeed(ref float previousSpeed)
    {
        float nowSpeed = STANDARD_SPEED + previousSpeed;
        float limitSpeed = STANDARD_SPEED * INCREASE_LIMIT;

        // ���݂̑������x�����E�l��������Ă���ԁA���x�𑝉�������
        if (nowSpeed < limitSpeed) { previousSpeed += SPEED_INCREASE_VALUE; }

        // �Z�o�����ŏI���x�i��{���x + �������x�j
        return (STANDARD_SPEED + previousSpeed) * Time.deltaTime;
    }
}
