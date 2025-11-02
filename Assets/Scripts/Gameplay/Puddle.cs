using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class Puddle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private AnimationCurve animationCurveAlpha;

        public void Init(float fadeDuration)
        {
            FadeAndDestroy(fadeDuration);
        }

        private void FadeAndDestroy(float fadeDuration)
        {
            Color startColor = spriteRenderer.color;
            
            spriteRenderer.DOColor(new Color(startColor.r, startColor.g, startColor.b, 0f), fadeDuration)
                .SetEase(animationCurveAlpha)
                .OnComplete(() => 
                {
                    Destroy(gameObject);
                });
        }
        
        private void OnDestroy()
        {
            DOTween.Kill(spriteRenderer);
        }
    }
}