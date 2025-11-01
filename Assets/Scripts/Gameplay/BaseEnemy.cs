using UnityEngine;
using UnityEngine.AI;

namespace Gameplay
{
    [RequireComponent(
        typeof(NavMeshAgent),
        typeof(Health),
        typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float stoppingDistance = 2f;

        [Header("Attack Settings")]
        [SerializeField] private float attackDamage = 1f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;

        [Header("Target")]
        [SerializeField] private Transform target;

        private NavMeshAgent _navMeshAgent;
        private Health _health;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _lastPosition;
        private float _lastAttackTime;

        void Start()
        {
            Init();
        }

        void Init()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            _navMeshAgent.speed = moveSpeed;
            _navMeshAgent.stoppingDistance = stoppingDistance;

            _health.OnDie += HandleDeath;
            _lastPosition = transform.position;
        }

        void Update()
        {
            if (!target || !_navMeshAgent.isActiveAndEnabled) 
                return;
            
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                TryAttack();
            }
            else
            {
                _navMeshAgent.SetDestination(target.position);
            }

            FlipSprite();
        }

        void TryAttack()
        {
            if (Time.time >= _lastAttackTime + attackCooldown)
            {
                Attack();
                
                _lastAttackTime = Time.time;
            }
        }

        void Attack()
        {
            if (!target) 
                return;

            if (target.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(attackDamage, gameObject);
            }
        }

        void FlipSprite()
        {
            Vector3 moveDirection = transform.position - _lastPosition;

            if (moveDirection.x > 0.01f)
                _spriteRenderer.flipX = false;
            else if (moveDirection.x < -0.01f)
                _spriteRenderer.flipX = true;

            _lastPosition = transform.position;
        }

        void HandleDeath()
        {
            _navMeshAgent.enabled = false;
            enabled = false;
        }

        void OnDestroy()
        {
            _health.OnDie -= HandleDeath;
        }
    }
}
