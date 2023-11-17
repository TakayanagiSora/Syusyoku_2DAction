using System;

/// <summary>
/// ��������x�������s���邽�߂̃N���X
/// </summary>
public class OnlyOnce
{
    private bool _isExecuted = false;


    /// <summary>
    /// �����̎��s�i�ŏ���1��̂݁j
    /// </summary>
    /// <param name="action"></param>
    public void Execution(Action action)
    {
        if (!_isExecuted)
        {
            _isExecuted = true;
            action();
        }
    }

    /// <summary>
    /// �����̎��s�i�ŏ���1��̂݁j
    /// </summary>
    /// <param name="action1"></param>
    /// <param name="action2"></param>
    public void Execution(Action action1, Action action2)
    {
        if (!_isExecuted)
        {
            _isExecuted = true;
            action1();
            action2();
        }
    }

    /// <summary>
    /// �����̎��s�i�ŏ���1��̂݁j
    /// </summary>
    /// <param name="action1"></param>
    /// <param name="action2"></param>
    /// <param name="action3"></param>
    public void Execution(Action action1, Action action2, Action action3)
    {
        if (!_isExecuted)
        {
            _isExecuted = true;
            action1();
            action2();
            action3();
        }
    }

    /// <summary>
    /// ����s��ԂɃ��Z�b�g
    /// </summary>
    public void Reset()
    {
        _isExecuted = false;
    }
}
