using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_Summon : EnemyActionBase
{
    [SerializeField, Tooltip("¢Š«ˆÊ’u")]
    private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField, Tooltip("¢Š«ŠÔŠu"), Min(0)]
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
        await Summon();
    }

    public override void Move()
    {

    }

    /// <summary>
    /// G‹›iƒhƒ[ƒ“j‚ğ¢Š«‚·‚é
    /// </summary>
    /// <returns></returns>
    private async UniTask Summon()
    {
        // “o˜^‚³‚ê‚Ä‚¢‚éSpawnPoint‚Ì”‚¾‚¯G‹›‚ğ¢Š«
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            _poolController.Get(_mob.Key, _spawnPoints[i].position, Quaternion.identity);
            await UniTask.WaitForSeconds(_summonInterval);
        }
    }
}
