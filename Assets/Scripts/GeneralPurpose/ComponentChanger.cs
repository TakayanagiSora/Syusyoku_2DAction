using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ComponentChanger : IChangeSprite, IChangeAnimator
{
    private SpriteRenderer _spriteRenderer = default;
    public SpriteRenderer SetSpriteRenderer { set => _spriteRenderer = value; }

    private Animator _animator = default;
    public Animator SetAnimator { set => _animator = value; }

    public void AnimatorController(AnimatorController controller)
    {
        _animator.runtimeAnimatorController = controller;
    }

    public void Sprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
