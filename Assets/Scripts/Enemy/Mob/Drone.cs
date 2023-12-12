using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : PoolObject, IDamagable
{
    private Transform _transform = default;

    private void Awake()
    {
        _transform = this.transform;
        _poolController = FindObjectOfType<PoolController>();
    }

    //private void Update()
    //{
        
    //}

    public override void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public override void Enable(Vector2 spawnPos, Quaternion spawnDir)
    {
        _transform.position = spawnPos;
        _transform.rotation = spawnDir;
        this.gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        _poolController.Return(this);
    }
}
