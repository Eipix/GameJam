using UnityEngine;
using DG.Tweening;

namespace Gameplay
{
    public class Umbrella : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float appearDuration = 1f;
        [SerializeField] private float floatHeight = 2f;
        [SerializeField] private float rotationDuration = 2f;

        [Header("Target Object")]
        [SerializeField] private Transform targetObject;

        private bool _isActive = false;
        private Tween _rotationTween;

        private void Start()
        {
            if (targetObject != null)
            {
                transform.position = GetTargetPosition();
            }

            transform.localScale = Vector3.zero;
            SetupColliders();
        }

        private void SetupColliders()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.isTrigger = true;
            }
        }

        private void Update()
        {
            if (!_isActive) return;
            if (targetObject == null) return;

            transform.position = GetTargetPosition();
        }

        private Vector3 GetTargetPosition()
        {
            return targetObject.position + Vector3.up * floatHeight;
        }

        public void ShowUmbrella()
        {
            if (_isActive) return;

            _isActive = true;
            gameObject.SetActive(true);

            transform.localScale = Vector3.zero;

            if (targetObject != null)
            {
                transform.position = GetTargetPosition();
            }

            transform.DOScale(1f, appearDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() => StartRotation());
        }

        public void HideUmbrella()
        {
            if (!_isActive) return;

            _isActive = false;
            StopRotation();

            transform.DOScale(0f, appearDuration * 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void StartRotation()
        {
            StopRotation();
            _rotationTween = transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);

        }

        public void StopRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive())
            {
                _rotationTween.Kill();
                _rotationTween = null;
            }
        }

        public void SetTarget(Transform newTarget)
        {
            targetObject = newTarget;

            if (_isActive && targetObject != null)
            {
                transform.position = GetTargetPosition();
            }
        }
    }
}