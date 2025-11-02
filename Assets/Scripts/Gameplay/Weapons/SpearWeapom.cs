using UnityEngine;
using Gameplay.Projectiles;

namespace Gameplay.Weapons
{
    public class SpearWeapon : MonoBehaviour
    {
        [SerializeField] private SpearProjectile projectilePrefab;
        [SerializeField] private Transform holdPoint;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float damage = 8f;
        [SerializeField] private float projectileSpeed = 20f;
        [SerializeField] private Camera playerCamera;

        [Header("Звуки")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip throwSound; // Звук броска
        [SerializeField] private float throwVolume = 1f;

        private float _nextFireTime;
        private SpearProjectile _currentSpear;
        private bool _hasSpearInHand = true;
        private float _throwHeight;

        private void Start()
        {
            _throwHeight = holdPoint.position.y;

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            }

            CreateSpearInHand();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= _nextFireTime && _hasSpearInHand)
            {
                Debug.Log("Spear Thrown");
                ThrowSpear();
                _nextFireTime = Time.time + cooldown;
            }
        }

        private void ThrowSpear()
        {
            _hasSpearInHand = false;

            Vector3 targetPosition = GetMousePosition();
            targetPosition.y = _throwHeight;

            Vector3 direction = (targetPosition - holdPoint.position).normalized;

            float distance = Vector3.Distance(holdPoint.position, targetPosition);
            float lifetime = distance / projectileSpeed + 1f;


            PlayThrowSound();

            _currentSpear.transform.SetParent(null);

            Collider col = _currentSpear.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = true;
            }

            _currentSpear.Init(damage, projectileSpeed, lifetime, direction, _throwHeight);
            _currentSpear.OnSpearStuck += HandleSpearStuck;
            _currentSpear.OnSpearHitEnemy += HandleSpearHitEnemy;

            Invoke(nameof(RespawnSpear), cooldown);
        }

        private void PlayThrowSound()
        {
            if (throwSound != null && audioSource != null) 
            {
                audioSource.PlayOneShot(throwSound, throwVolume);
            }
            else if (throwSound != null)
            {
                // Запасной вариант если audioSource не работает
                AudioSource.PlayClipAtPoint(throwSound, transform.position, throwVolume);
            }
        }

        private void HandleSpearStuck()
        {
            Debug.Log("Spear stuck in object");
        }

        private void HandleSpearHitEnemy()
        {
            Debug.Log("Spear hit enemy");
        }

        private void RespawnSpear()
        {
            if (_currentSpear != null)
            {
                Destroy(_currentSpear.gameObject);
            }

            CreateSpearInHand();
            _hasSpearInHand = true;
            Debug.Log("Spear respawned");
        }

        private void CreateSpearInHand()
        {
            _currentSpear = Instantiate(projectilePrefab, holdPoint.position, holdPoint.rotation);
            _currentSpear.transform.SetParent(holdPoint);

            Rigidbody rb = _currentSpear.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            Collider col = _currentSpear.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            _currentSpear.transform.localPosition = Vector3.zero;
            _currentSpear.transform.localRotation = Quaternion.identity;
        }

        private Vector3 GetMousePosition()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, _throwHeight, 0));

            if (horizontalPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return ray.origin + ray.direction * 50f;
        }
    }
}