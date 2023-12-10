using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float _hp = default;

    public void TakeDamage(int damage)
    {
        print("プレイヤーがくらいました");
        _hp -= damage;
    }
}
