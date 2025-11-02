using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        public const float MinSpeed = 1.0f;
        public readonly float DefaultSpeed = 5f;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private List<GameObject> weapons = new List<GameObject>();
        
        [SerializeField] private Sprite StateAndMove;
        [SerializeField] private Sprite Damagable;
        [SerializeField] private Sprite AttackHolyWater;
        
        private Rigidbody _rigidbody;
        private SpriteRenderer _spriteRenderer;

        public float Speed => moveSpeed;
    
        void Start()
        {
            Init();
        }

        void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            EnemySpawner.Instance.WaveEnded += OnWaveEnded;
        }

        private void OnWaveEnded(int waveIndex)
        {
            if(waveIndex < weapons.Count)
            {
                weapons[waveIndex].SetActive(false);
            }
        }

        void FixedUpdate()
        {
            HandleMovement();
        }

        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            FlipSprite(horizontal);
        }

        public void ChangeSpeed(float speed)
        {
            moveSpeed += speed;
            moveSpeed = Mathf.Max(MinSpeed, moveSpeed);
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 movement = direction * moveSpeed;

            _rigidbody.velocity = new Vector3(movement.x, 0f, movement.z);
        }
    
        private void FlipSprite(float horizontal)
        {
            if (horizontal > 0)
                _spriteRenderer.flipX = true;
            else if (horizontal < 0)
                _spriteRenderer.flipX = false;
        }

        private void OnDestroy()
        {
            EnemySpawner.Instance.WaveEnded -= OnWaveEnded;
        }
    }
}
