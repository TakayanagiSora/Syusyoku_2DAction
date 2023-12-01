using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerAnimationChanger : MonoBehaviour
{
    private void Awake()
    {
        Animator animator = this.GetComponent<Animator>();

        PlayerManager playerManager = this.GetComponent<PlayerManager>();
        playerManager.PlayerState.Where(state => state == PlayerState.Idle).Subscribe(state => animator.SetTrigger(""));
    }
}
