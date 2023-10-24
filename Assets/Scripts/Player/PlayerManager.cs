using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private Transform _transform = default;

    private PlayerStatus _playerStatus = default;
    private WeaponBase _equippedWeapon = default;
}
