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

    private void Awake()
    {
        gameObject.SetActive(false);

        _frames = GetComponentsInChildren<Frame>(true);

        if (_frames is null || _frames.Length is 0)
            throw new InvalidOperationException("Не удалось найти кадры в дочерних объектах катсцены. Катсцена должна содержать хотя бы 1 кадр ");

        foreach (var frame in _frames)
        {
            frame.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Increment();
    }

    // Запуск катсцены
    public void Launch()
    {
        gameObject.SetActive(true);
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
