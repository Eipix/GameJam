using DG.Tweening;
using Gameplay;
using System;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class ExperienceParticle : MonoBehaviour
{
    [SerializeField, Range(1f, 100f)] private float _fallDistance = 1.0f;
    [SerializeField, Range(0.1f, 1f)] private float _fallDuration = .1f;

    [SerializeField, Range(0.1f, 3f)] private float _moveToPlayerDuration;

    public event Action Collecting;
    public event Action Collected;

    private UIProgressBar _progressBar;
    private SphereCollider _collider;
    private Collider _target;

    private Tween _fallToFloor;

    public Sequence MoveToPlayer { get; private set; }

    public int ScorePerParticle { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    public void Init(Transform target, UIProgressBar progressBar, int scorePerParticle)
    {
        _progressBar = progressBar;
        ScorePerParticle = scorePerParticle;

        if (target.TryGetComponent(out Collider collider) is false)
            throw new InvalidOperationException($"Target для спавна опыта должен содержать коллайдер!");

        _target = collider;
        var bounds = _target.bounds;

        Vector3 bottomPoint = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);

        float angleRad = UnityEngine.Random.Range(1, 361) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * _fallDistance;
        float z = Mathf.Sin(angleRad) * _fallDistance;

        Vector3 finalPoint = bottomPoint + new Vector3(x, 0f, z);

        _fallToFloor = transform.DOMove(finalPoint, _fallDuration);
    }

    public void ForceCollect(Player player)
    {
        if (_fallToFloor != null && _fallToFloor.IsActive())
            _fallToFloor.Complete();

        if (MoveToPlayer != null && MoveToPlayer.IsActive())
            MoveToPlayer.Complete(true);

        MoveToPlayer = DOTween.Sequence();
        MoveToPlayer.Append(transform.DOMove(player.transform.position, _moveToPlayerDuration));
        MoveToPlayer.Append(_progressBar.DOFill(ScorePerParticle));

        MoveToPlayer.OnStart(() => Collecting?.Invoke());
        MoveToPlayer.OnComplete(() =>
        {
            Collected?.Invoke();
            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) is false)
            return;

        ForceCollect(player);
    }

    private void OnDestroy()
    {
        MoveToPlayer?.Kill();
    }
}
