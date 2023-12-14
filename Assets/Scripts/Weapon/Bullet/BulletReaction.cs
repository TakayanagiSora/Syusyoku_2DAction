using UnityEngine;
using Cysharp.Threading.Tasks;

public class BulletReaction : PoolObject
{
    [SerializeField]
    private float _lifeTime = default;

    private Transform _transform = default;

    private void Awake()
    {
        _transform = this.transform;
        _poolController = FindObjectOfType<PoolController>();
    }

    public override void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public override async void Enable(Vector2 spawnPos, Quaternion spawnDir)
    {
        _transform.position = spawnPos;
        _transform.rotation = spawnDir;
        this.gameObject.SetActive(true);

        await LifeTimeAsync();
    }

    private async UniTask LifeTimeAsync()
    {
        await UniTask.WaitForSeconds(_lifeTime);
        _poolController.Return(this);
    }
}
