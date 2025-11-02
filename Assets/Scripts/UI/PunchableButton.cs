using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PunchableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Range(0, 1)]
    [SerializeField] private float _punchStrength;
    [Range(0, 1)]
    [SerializeField] private float _duration;

    private RectTransform _rectTransform;

    private Tween _tween;

    private Vector2 _defaultScale;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _defaultScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData) => ScaleDown();

    public void OnPointerUp(PointerEventData eventData) => ScaleUp();

    private void ScaleDown()
    {
        CompleteIfActive(true);
        _tween = _rectTransform.DOScale(_defaultScale * (1 - _punchStrength), _duration).SetUpdate(true);
    }

    private void ScaleUp()
    {
        CompleteIfActive(true);
        _tween = _rectTransform.DOScale(_defaultScale, _duration).SetUpdate(true);
    }

    private void CompleteIfActive(bool withCallbacks)
    {
        if (_tween != null && _tween.IsActive())
            _tween.Complete(withCallbacks);
    }
}
