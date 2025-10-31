using DG.Tweening;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    [SerializeField, Range(0.01f, 10f)]
    private float _jumpPower = 1f;
    [SerializeField] private float _duration = 3f;

    private Tween _jumping;
    private Vector3 _defaultPosition;

    private int _numJumps = 1;

    private void OnValidate()
    {
        if (_jumping.IsActive() is false)
            return;

        Restart();
    }

    private void Start()
    {
        _defaultPosition = transform.position;
        Restart();
    }

    private void Restart()
    {
        Kill();
        transform.position = _defaultPosition;
        _jumping = transform.DOJump(transform.position, _jumpPower, _numJumps, _duration).SetLoops(-1, LoopType.Restart);
    }

    private void Kill() =>_jumping?.Kill();
}
