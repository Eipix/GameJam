using UnityEngine;
using DG.Tweening;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform _fill;
    [SerializeField] private Vector2 _range;

    private Tween _filling;

    public void Fill(Vector2 range, float currentValue)
    {
        float value = GetValue(range, currentValue);
        _fill.anchoredPosition = new Vector2(0, value);
    }

    public Tween DOFill(Vector2 range, float currentValue, float duration)
    {
        if (_filling != null && _filling.IsActive())
            _filling.Complete();

        float value = GetValue(range, currentValue);
        _filling = _fill.DOAnchorPosX(value, duration);
        return _filling;
    }

    public Tween FillMax(float duration) => _fill.DOAnchorPos(new Vector2(0, _range.y), duration);
    public Tween FillMin(float duration) => _fill.DOAnchorPos(new Vector2(0, _range.x), duration);

    public void FillMax() => _fill.anchoredPosition = new Vector2(0, _range.y);

    public void FillMin() => _fill.anchoredPosition = new Vector2(0, _range.x);

    private float GetValue(Vector2 range, float currentValue) => _range.x + (_range.y - _range.x) * ((currentValue - range.x) /
                                                                                                      (range.y - range.x));
}
