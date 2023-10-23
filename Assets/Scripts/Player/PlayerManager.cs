using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private Transform _transform = default;

    private PlayerMove _playerMove = default;
    private PlayerStatus _playerStatus = default;
    private WeaponBase _equippedWeapon = default;

    private GameInputs _gameInputs = default;

    private void OnEnable()
    {
        _gameInputs?.Enable();
    }

    private void Awake()
    {
        _transform = this.transform;

        _playerMove = new();

        _gameInputs = new();
        _gameInputs.Enable();

        _gameInputs.Player.Move.performed += _playerMove.OnMove;
        _gameInputs.Player.Move.canceled += _playerMove.OnStop;
        //_gameInputs.Player.
    }

    private void OnDisable()
    {
        _gameInputs?.Disable();
    }

    private void Update()
    {
        _playerMove.Run(_transform);
    }
}
