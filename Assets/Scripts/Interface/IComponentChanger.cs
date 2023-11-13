using UnityEditor.Animations;
using UnityEngine;

public interface IChangeSprite
{
    SpriteRenderer SetSpriteRenderer { set; }
    void Sprite(Sprite sprite);
}

public interface IChangeAnimator
{
    Animator SetAnimator { set; }
    void AnimatorController(AnimatorController controller);
}