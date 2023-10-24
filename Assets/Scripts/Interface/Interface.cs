using UnityEngine;

public interface IRaycast
{
    /// <summary>
    /// �Ώۂ�GameObject�ɐڂ��Ă���Ƃ��Atrue��Ԃ�
    /// </summary>
    /// <param name="origin">Ray�̌��_</param>
    /// <returns></returns>
    bool Check(Vector2 origin);

    /// <summary>
    /// �Ώۂ��ݒ肵��Layer��GameObject�ɐڂ��Ă���Ƃ��Atrue��Ԃ�
    /// </summary>
    /// <param name="origin">Ray�̌��_</param>
    /// <param name="layerMask">�t�B���^�����O����Layer�̃}�X�N�l</param>
    /// <returns></returns>
    bool Check(Vector2 origin, int layerMask);
}

public interface IGroundable
{
    /// <summary>
    /// �Ώۂ�GroundLayer��GameObject�ɐڂ��Ă���Ƃ��Atrue��Ԃ�
    /// </summary>
    /// <param name="origin">Ray�̌��_</param>
    /// <returns></returns>
    bool CheckGround(Vector2 origin);
}
