using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int _hp = default;

    public void TakeDamage(int damage)
    {
        print("�_���[�W��H�炢�܂���");
        _hp -= damage;
    }
}
