using DG.Tweening;
using Gameplay;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class ExperienceParticle : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int _scorePerParticle;
    [SerializeField] private float _moveToPlayerDuration;

    private UIProgressBar _progressBar;
    private SphereCollider _collider;

    public Tween MoveToPlayer { get; private set; }

    public int ScorePerParticle => _scorePerParticle;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    public void Init(UIProgressBar progressBar) => _progressBar = progressBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) is false)
            return;

        MoveToPlayer = transform.DOMove(player.transform.position, _moveToPlayerDuration)
            .OnComplete(() => _progressBar.DOFill(_scorePerParticle));
    }
}
