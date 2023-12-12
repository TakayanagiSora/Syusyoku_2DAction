using UnityEngine;
using UniRx;

public class ChargeEffect_Normal : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon = default;
    [SerializeField]
    private float _animationSpeed_Level_S = default;
    [SerializeField]
    private float _animationSpeed_Level_M = default;
    [SerializeField]
    private float _animationSpeed_Level_L = default;
    private Transform _transform = default;
    private Animator _animator = default;
    private SpriteRenderer _spriteRenderer = default;
    private Vector3 _scale = default;

    private readonly int _speedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        _transform = this.transform;
        _animator = this.GetComponent<Animator>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        // 初期化
        _spriteRenderer.enabled = false;
        _scale = _transform.localScale;

        // イベント購読
        _weapon.ChargeLevel.Subscribe(level => ChangeEffect(level));
    }

    /// <summary>
    /// チャージ段階に応じてEffectの見た目を変える処理
    /// </summary>
    /// <param name="chargeLevel"></param>
    private void ChangeEffect(ChargeLevel chargeLevel)
    {
        switch (chargeLevel)
        {
            case ChargeLevel.S:
                _animator.SetFloat(_speedHash, _animationSpeed_Level_S);
                _transform.localScale = _scale;
                _spriteRenderer.enabled = true;
                break;

            case ChargeLevel.M:
                _animator.SetFloat(_speedHash, _animationSpeed_Level_M);
                _transform.localScale = _scale * 1.5f;
                break;

            case ChargeLevel.L:
                _animator.SetFloat(_speedHash, _animationSpeed_Level_L);
                _transform.localScale = _scale * 2f;
                break;

            case ChargeLevel.None:
                _spriteRenderer.enabled = false;
                break;
        }
    }
}
