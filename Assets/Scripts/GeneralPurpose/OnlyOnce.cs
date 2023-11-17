using System;

/// <summary>
/// 処理を一度だけ実行するためのクラス
/// </summary>
public class OnlyOnce
{
    private bool _isExecuted = false;


    /// <summary>
    /// 処理の実行（最初の1回のみ）
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
    /// 処理の実行（最初の1回のみ）
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
    /// 処理の実行（最初の1回のみ）
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
    /// 非実行状態にリセット
    /// </summary>
    public void Reset()
    {
        _isExecuted = false;
    }
}
