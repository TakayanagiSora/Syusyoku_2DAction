using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int _hp = default;

    public void TakeDamage(int damage)
    {
        print("ダメージを食らいました");
        _hp -= damage;
    }
}
