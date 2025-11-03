using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class UIProgressBar : MonoBehaviour
{
    public const int MaxProgress = 900;

    [SerializeField, Range(0.1f, 3f)] private float _fillSpeed = 0.1f;
    [SerializeField] private RectTransform _fill;
    [SerializeField] private Vector2 _range = new(-540f, 0f);

    public event UnityAction<int> ValueChanged;
    public event UnityAction MinValueAchieved;

    private Tween _filling;

    public int CurrentProgress { get; private set; } = MaxProgress;

    protected void Fill(Vector2 range, float currentValue)
    {
        float value = GetValue(range, currentValue);
        _fill.anchoredPosition = new Vector2(0, value);
    }

    public void Restart()
    {
        _filling.Complete();
        CurrentProgress = MaxProgress;
        var fillRect = (RectTransform)_fill.transform;
        fillRect.anchoredPosition = Vector2.zero;
    }

    public Tween DOFill(int decreasing)
    {
        if (_filling != null && _filling.IsActive())
            _filling.Complete(true);

        CurrentProgress -= Mathf.Max(0, decreasing);
        CurrentProgress = Mathf.Max(0, CurrentProgress);

        float value = GetValue(new Vector2(0, MaxProgress), CurrentProgress);

        _filling = _fill.DOAnchorPosX(value, _fillSpeed); 

        if (CurrentProgress <= 0)
            MinValueAchieved?.Invoke();

        ValueChanged?.Invoke(CurrentProgress);

        return _filling;
    }

    public Tween FillMax(float duration) => _fill.DOAnchorPos(new Vector2(0, _range.y), duration);
    public Tween FillMin(float duration) => _fill.DOAnchorPos(new Vector2(0, _range.x), duration);

    public void FillMax() => _fill.anchoredPosition = new Vector2(0, _range.y);

    public void FillMin() => _fill.anchoredPosition = new Vector2(0, _range.x);

    private float GetValue(Vector2 range, float currentValue) => _range.x + (_range.y - _range.x) * ((currentValue - range.x) /
                                                                                                      (range.y - range.x));
}
