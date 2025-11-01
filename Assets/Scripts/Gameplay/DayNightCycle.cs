﻿using Common;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class DayNightCycle : Singleton<DayNightCycle>
    {
        [SerializeField] private Light directionalLight;
        [SerializeField] private float transitionDuration = 2f;

        [Header("Ночь")]
        [SerializeField] private Color nightColor = new Color(0.1f, 0.1f, 0.2f);
        [SerializeField] private float nightIntensity = 0.2f;
        [SerializeField] private Vector3 nightRotation = new Vector3(-10f, -30f, 0f);

        [Header("Вечер")]
        [SerializeField] private Color eveningColor = new Color(1f, 0.5f, 0.3f);
        [SerializeField] private float eveningIntensity = 0.5f;
        [SerializeField] private Vector3 eveningRotation = new Vector3(10f, -30f, 0f);

        [Header("День")]
        [SerializeField] private Color dayColor = new Color(1f, 0.96f, 0.84f);
        [SerializeField] private float dayIntensity = 1f;
        [SerializeField] private Vector3 dayRotation = new Vector3(50f, -30f, 0f);

        private void Start()
        {
            SetNightImmediate();
            EnemySpawner.Instance.WaveEnded += OnWaveEnded;
        }

        private void OnDestroy()
        {
            if (EnemySpawner.Instance)
            {
                EnemySpawner.Instance.WaveEnded -= OnWaveEnded;
            }
        }

        private void OnWaveEnded(int waveIndex)
        {
            if (waveIndex == 0) // После первой волны - вечер
            {
                SetEvening();
            }
            else if (waveIndex == 1) // После второй волны - день
            {
                SetDay();
            }
        }

        private void SetNightImmediate()
        {
            directionalLight.color = nightColor;
            directionalLight.intensity = nightIntensity;
            directionalLight.transform.rotation = Quaternion.Euler(nightRotation);
        }

        public void SetEvening()
        {
            StartTransition(eveningColor, eveningIntensity, eveningRotation);
        }

        public void SetDay()
        {
            StartTransition(dayColor, dayIntensity, dayRotation);
        }

        private void StartTransition(Color targetColor, float targetIntensity, Vector3 targetRotation)
        {
            DOTween.Kill(directionalLight);
            DOTween.Kill(directionalLight.transform);

            directionalLight.DOColor(targetColor, transitionDuration).SetEase(Ease.InOutQuad);
            DOTween.To(() => directionalLight.intensity, x => directionalLight.intensity = x, targetIntensity, transitionDuration)
                .SetEase(Ease.InOutQuad);
            directionalLight.transform.DORotate(targetRotation, transitionDuration).SetEase(Ease.InOutQuad);
        }
    }
}
