using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cutscene : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction Ended;

    private Frame[] _frames;

    private int _previousIndex;
    private int _currentIndex;

    private bool _isInit;

    private void Awake()
    {
        Init();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Increment();
    }

    public void Init()
    {
        if (_isInit)
            return;

        _frames = GetComponentsInChildren<Frame>(true);

        if (_frames is null || _frames.Length is 0)
            throw new InvalidOperationException("Не удалось найти кадры в дочерних объектах катсцены. Катсцена должна содержать хотя бы 1 кадр ");

        foreach (var frame in _frames)
        {
            frame.gameObject.SetActive(false);
        }

        _isInit = true;
    }

    // Запуск катсцены
    public void Launch()
    {
        Init();
        _frames[0].gameObject.SetActive(true);
    }

    private void Increment()
    {
        _currentIndex++;

        if (_currentIndex >= _frames.Length)
        {
            _frames[_previousIndex].gameObject.SetActive(false);

            _currentIndex = 0;
            _previousIndex = 0;

            gameObject.SetActive(false);

            Ended?.Invoke();
        }
        else
        {
            _frames[_previousIndex].gameObject.SetActive(false);
            _frames[_currentIndex].gameObject.SetActive(true);
            _previousIndex = _currentIndex;
        }
    }
}
