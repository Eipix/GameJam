using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Gameplay
{
    [RequireComponent(
        typeof(NavMeshAgent),
        typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float stoppingDistance = 2f;

        [Header("Attack Settings")]
        [SerializeField] private float attackDamage = 1f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;

        [Header("Attack Animation")]
        [SerializeField] private float attackTiltAngle = 30f;
        [SerializeField] private float attackDuration = 0.15f;
        [SerializeField] private float returnDuration = 0.15f;

        
        [Header("Target")]
        [SerializeField] private Transform target;
        
        [Header("Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private NavMeshAgent _navMeshAgent;
        private Health _health;
        private Vector3 _lastPosition;
        private float _lastAttackTime;

        private Sequence _attackSequence;
        
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            

            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = true;
            _navMeshAgent.speed = moveSpeed;
            _navMeshAgent.stoppingDistance = stoppingDistance;
            _navMeshAgent.baseOffset = 1f;
            
            //_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            
            _health.OnDie += HandleDeath;
            _lastPosition = transform.position;
        }

        public void Init(Transform character)
        {
            target = character;
        }

        void Update()
        {
            if (!_navMeshAgent.isActiveAndEnabled) 
                return;
            
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                _navMeshAgent.SetDestination(transform.position);
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
                PlayAttackAnimation();
                
                _lastAttackTime = Time.time;
            }
        }

        void Attack()
        {
            if (target.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(attackDamage, gameObject);
            }
        }

        void PlayAttackAnimation()
        {
            _attackSequence?.Kill();

            float direction = spriteRenderer.flipX ? 1f : -1f;
            float attackAngle = attackTiltAngle * direction;

            _attackSequence = DOTween.Sequence();
            
            _attackSequence.Append(
                spriteRenderer.transform.DORotate(new Vector3(0, 0, attackAngle), attackDuration)
                    .SetEase(Ease.OutBack)
            );
            _attackSequence.AppendCallback(Attack);
            
            _attackSequence.Append(
                spriteRenderer.transform.DORotate(Vector3.zero, returnDuration)
                    .SetEase(Ease.InOutQuad)
            );
        }
        
        void FlipSprite()
        {
            Vector3 moveDirection = transform.position - _lastPosition;

            if (moveDirection.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (moveDirection.x < -0.01f)
                spriteRenderer.flipX = true;

            _lastPosition = transform.position;
        }

        void HandleDeath()
        {
            // TODO  судя по всему drop мешает врагам ходить
            ItemFactory.Instance.TrySpawnRandom(transform.position, out Item item);

            Destroy(gameObject);
            _navMeshAgent.enabled = false;
            enabled = false;
        }

        void OnDestroy()
        {
            _attackSequence?.Kill();
            _health.OnDie -= HandleDeath;
        }
    }
}
