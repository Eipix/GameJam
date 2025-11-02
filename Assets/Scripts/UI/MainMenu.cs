using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _exit;
    [SerializeField] private Button _play;

    [SerializeField, Range(0.1f, 10f)] private float _duration = 0.3f;
    [SerializeField, Range(0.1f, 10f)] private float _idleMoveDuration = 1f;
    [SerializeField, Range(1f, 100f)] private float _idleMoveOffset = 10f;


    private Sequence _show;
    private Sequence _hide;
    private Sequence _idleMove;

    private RectTransform _canvasRect;

    private ButtonInfo _exitInfo;
    private ButtonInfo _playInfo;

    private void Awake()
    {
        gameObject.SetActive(false);

        _exitInfo = new ButtonInfo(_exit);
        _playInfo = new ButtonInfo(_play);

        _canvasRect = (RectTransform)transform.root.transform;
    }

    private void OnEnable() => Show();
    //private void OnDisable() => Hide();

    public Sequence Show()
    {
        if(_show is not null && _show.IsActive())
            _show.Complete();

        if (_hide is not null && _hide.IsActive())
            _hide.Complete();

        _idleMove.Complete();

        _show = DOTween.Sequence();

        _show.AppendCallback(() =>
        {
            _exitInfo.Hide();
            _playInfo.Hide();

            gameObject.SetActive(true);
        });
        _show.Append(_exitInfo.RectTransform.DOAnchorPosY(_exitInfo.TargetPosition.y, _duration));
        _show.Join(_playInfo.RectTransform.DOAnchorPosY(_playInfo.TargetPosition.y, _duration));

        _show.OnComplete(() =>
        {
            _idleMove = DOTween.Sequence()
             .Append(_playInfo.RectTransform.DOAnchorPosY(_playInfo.RectTransform.anchoredPosition.y + _idleMoveOffset, _idleMoveDuration).SetEase(Ease.InOutSine))
             .Join(_exitInfo.RectTransform.DOAnchorPosY(_exitInfo.RectTransform.anchoredPosition.y + _idleMoveOffset, _idleMoveDuration).SetEase(Ease.InOutSine))

             .Append(_playInfo.RectTransform.DOAnchorPosY(_playInfo.RectTransform.anchoredPosition.y - _idleMoveOffset, _idleMoveDuration).SetEase(Ease.InOutSine))
             .Join(_exitInfo.RectTransform.DOAnchorPosY(_exitInfo.RectTransform.anchoredPosition.y - _idleMoveOffset, _idleMoveDuration).SetEase(Ease.InOutSine))

             .SetLoops(-1, LoopType.Yoyo);
        });

        return _show;
    }

    public Sequence Hide()
    {
        if (_hide is not null && _hide.IsActive())
            _hide.Complete();

        if (_show is not null && _show.IsActive())
            _show.Complete();

        _idleMove.Complete();

        _hide = DOTween.Sequence();

        _hide.Append(_exitInfo.RectTransform.DOAnchorPosY(_exitInfo.HidePosition.y, _duration).SetEase(Ease.OutQuad));
        _hide.Join(_playInfo.RectTransform.DOAnchorPosY(_playInfo.HidePosition.y, _duration).SetEase(Ease.OutQuad));
        _show.AppendCallback(() => gameObject.SetActive(false));

        return _hide;
    }
}

public struct ButtonInfo
{
    public Button Button { get; private set; }
    public RectTransform RectTransform { get; private set; }

    public Vector2 TargetPosition { get; private set; }
    public Vector2 HidePosition { get; private set; }

    public ButtonInfo(Button button)
    {
        Button = button;
        RectTransform = (RectTransform)button.transform;
        TargetPosition = RectTransform.anchoredPosition;

        var canvasAnchoredPosition = ((RectTransform)RectTransform.root).anchoredPosition;

        HidePosition = TargetPosition + new Vector2(0f, canvasAnchoredPosition.y);
    }

    public void Hide() => RectTransform.anchoredPosition = HidePosition;
    public void Show() => RectTransform.anchoredPosition = TargetPosition;
}