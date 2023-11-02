using System;

/// <summary>
/// 処理を一度だけ実行するためのクラス
/// </summary>
public class OnlyOnce
{
    private bool _isExecuted = false;


    public void Execution(Action action)
    {
        if (!_isExecuted)
        {
            _isExecuted = true;
            action();
        }
    }

    public void Execution(Action action1, Action action2)
    {
        if (!_isExecuted)
        {
            _isExecuted = true;
            action1();
            action2();
        }
    }

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

    public void Reset()
    {
        _isExecuted = false;
    }
}
