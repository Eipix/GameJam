using UnityEngine;

namespace Gameplay.Projectiles
{
    public class HolyWaterProjectile : Projectile
    {
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private AnimationCurve arcCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float arcHeight = 2f;
        [SerializeField] private Puddle puddlePrefab;

        [Header("Настройка звука")]
        [SerializeField] private AudioClip breakSound;
        [SerializeField][Range(0f, 1f)] private float breakVolume = 1f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float travelTime;
        private float currentTime;


        protected override void InitializeMovement(Vector3 direction)
        {
            startPosition = transform.position;
            targetPosition = startPosition + direction.normalized * (speed * lifetime);
            travelTime = lifetime;
            currentTime = 0f;

            //Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            if (currentTime < travelTime)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / travelTime);

                Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);
                float height = arcCurve.Evaluate(t) * arcHeight;

                transform.position = horizontalPosition + Vector3.up * height;
            }
            else
            {
                DealAreaDamage();
                Destroy(gameObject);
            }
        }

        protected override void HandleCollider(Collider target)
        {
        }

        private void CreatPuddle()
        {
            // RaycastHit hit;
            // Vector3 position = transform.position;
            //
            // if (Physics.Raycast(position, Vector3.down, out hit))
            // {
            //     hit.point = new Vector3(hit.point.x, hit.point.y + 0.05f, hit.point.z);
            //     
            Vector3 position = transform.position;
        
            var puddle = Instantiate(puddlePrefab, position, Quaternion.identity);
            puddle.Init(2);
            //}
        }

        private void DealAreaDamage()
        {
            PlayBreakSound();
            CreatPuddle();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            Debug.Log(hitColliders.Length);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<Health>(out var health) && !hitCollider.gameObject.CompareTag("Player"))
                {
                    health.TakeDamage(damage, gameObject);
                    Debug.Log(health.CurrentHealth);
                }
            }
        }

        private void PlayBreakSound()
        {
            if (breakSound != null)
            {
                // Воспроизводим звук в позиции разбития
                AudioSource.PlayClipAtPoint(breakSound, transform.position, breakVolume);
            }
        }
    }
}