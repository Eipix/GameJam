using UnityEngine;

namespace Gameplay.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected float damage = 10f;
        protected float speed = 15f;
        protected float lifetime = 5f;
        
        protected Rigidbody rb;
    
        public float Damage => damage;
        public float Speed => speed;
        public float Lifetime => lifetime;
        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Init(float damage, float speed, float lifetime, Vector3 direction)
        {
            this.damage = damage;
            this.speed = speed;
            this.lifetime = lifetime;
            
            InitializeMovement(direction.normalized);
        }
        
        protected virtual void InitializeMovement(Vector3 direction)
        {
            rb.velocity = direction * speed;
            
            Destroy(gameObject, lifetime);
        }
    
        private void OnTriggerEnter(Collider other)
        {
            HandleCollider(other);
        }
    
        protected virtual void HandleCollider(Collider target)
        {
            if (!target.TryGetComponent<Health>(out var health) || target.CompareTag("Player")) 
                return;
            
            health.TakeDamage(damage, gameObject);
                
            Destroy(gameObject);
        }
    }
}