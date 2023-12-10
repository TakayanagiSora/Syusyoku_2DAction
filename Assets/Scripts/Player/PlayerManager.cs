using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerManager : MonoBehaviour
{
    private Transform _transform = default;

    private PlayerStatus _playerStatus = default;
    private Weapon _equippedWeapon = default;
}
