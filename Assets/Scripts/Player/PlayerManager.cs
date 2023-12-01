using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(PlayerMove), typeof(PlayerAnimationChanger))]
public class PlayerManager : MonoBehaviour
{
    private Transform _transform = default;

    private PlayerStatus _playerStatus = default;
    private Weapon _equippedWeapon = default;

    private ReactiveProperty<PlayerState> _playerState = new ReactiveProperty<PlayerState>();
    public IReadOnlyReactiveProperty<PlayerState> PlayerState => _playerState;
}
