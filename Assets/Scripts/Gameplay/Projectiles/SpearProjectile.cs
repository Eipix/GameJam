using UnityEngine;
using System;

namespace Gameplay.Projectiles
{
    public class SpearProjectile : Projectile
    {
        public event Action OnSpearStuck;
        public event Action OnSpearHitEnemy;

        private bool _hasHit = false;
        private Transform _stuckTransform;
        private float _fixedHeight;

        public void Init(float damage, float speed, float lifetime, Vector3 direction, float height)
        {
            this.damage = damage;
            this.speed = speed;
            this.lifetime = lifetime;
            this._fixedHeight = height;

            InitializeMovement(direction.normalized);
        }

        protected override void Awake()
        {
            rb = GetComponent<Rigidbody>();

            // Запрещаем вращение
            if (rb != null)
            {
                rb.freezeRotation = true;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        protected override void InitializeMovement(Vector3 direction)
        {
            direction.y = 0;
            direction = direction.normalized;

            rb.isKinematic = false;
            rb.useGravity = false;

            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.velocity = direction * speed;

            Vector3 startPosition = transform.position;
            startPosition.y = _fixedHeight;
            transform.position = startPosition;

            if (direction != Vector3.zero)
            {
                transform.forward = direction;
            }

            Destroy(gameObject, lifetime);
        }

        protected override void HandleCollider(Collider target)
        {
            //if (_hasHit) return;
            if (target.CompareTag("Player")) return;

            _hasHit = true;

            // Останавливаем копье
            // rb.velocity = Vector3.zero;
            // rb.angularVelocity = Vector3.zero;
            // rb.isKinematic = true;

            bool hasHealth = target.TryGetComponent<Health>(out var health);
            
            if (hasHealth && !target.CompareTag("Player"))
            {
                health.TakeDamage(damage, gameObject);
                OnSpearHitEnemy?.Invoke();
                //Debug.LogError(health.CurrentHealth);
                Destroy(gameObject);
            }
            else
            {
                //HandleStuckInObject(target);
                OnSpearStuck?.Invoke();
            }
        }

        private void HandleStuckInObject(Collider target)
        {
            if (target.attachedRigidbody != null)
            {
                transform.SetParent(target.transform);
                _stuckTransform = target.transform;
            }

            Collider spearCollider = GetComponent<Collider>();
            if (spearCollider != null)
            {
                spearCollider.enabled = false;
            }
        }

        private void Update()
        {
            if (!_hasHit && rb != null && !rb.isKinematic)
            {
                Vector3 position = transform.position;
                position.y = _fixedHeight;
                transform.position = position;
            }
        }

        private void OnDestroy()
        {
            OnSpearStuck = null;
            OnSpearHitEnemy = null;
        }
    }
}