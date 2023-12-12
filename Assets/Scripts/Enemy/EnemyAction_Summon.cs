using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAction_Summon : EnemyActionBase
{
    [SerializeField, Tooltip("è¢ä´à íu")]
    private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField, Tooltip("è¢ä´ä‘äu"), Min(0)]
    private float _summonInterval = default;
    [Space, SerializeField]
    private UsePool _mob = default;
    private PoolController _poolController = default;


    protected override void Awake()
    {
        base.Awake();
        _poolController = FindObjectOfType<PoolController>();
    }

    public override void Disable()
    {
        throw new System.NotImplementedException();
    }

    public override async void Enable()
    {
        await Attack();
    }

    public override void Move()
    {

    }

    private async UniTask Attack()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            _poolController.Get(_mob.Key, _spawnPoints[i].position, Quaternion.identity);
            await UniTask.WaitForSeconds(_summonInterval);
        }
    }
}
