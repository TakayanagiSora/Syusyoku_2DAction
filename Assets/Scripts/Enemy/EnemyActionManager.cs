using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemyActionBase> _enemyActions = new List<EnemyActionBase>();

    private int _selectedActionIndex = 0;

    private void Start()
    {
        _enemyActions[0].Enable();
    }

    private void Update()
    {
        _enemyActions[_selectedActionIndex].Move();
    }

    private void UpdateAction()
    {
        _enemyActions[_selectedActionIndex].Disable();

        _selectedActionIndex++;

        _enemyActions[_selectedActionIndex].Enable();
    }
}
