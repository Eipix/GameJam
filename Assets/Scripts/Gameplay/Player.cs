using System;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    [RequireComponent(typeof(CharacterController), typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        public const float MinSpeed = 1.0f;
        public readonly float DefaultSpeed = 5f;

        [Header("Movement Settings")] [SerializeField]
        private float moveSpeed = 5f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundedGravity = -2f;
    
        [SerializeField] private List<GameObject> weapons = new List<GameObject>();
        
        // [SerializeField] private SpriteRenderer StateAndMove;
        // [SerializeField] private SpriteRenderer StateAndMove;
        
        private CharacterController _characterController;
        private SpriteRenderer _spriteRenderer;
    
        private Vector3 velocity;

        public float Speed => moveSpeed;
    
        void Start()
        {
            Init();
        }

        void Init()
        {
            _characterController = GetComponent<CharacterController>();
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

        void Update()
        {
            ApplyGravity();
            HandleMovement();
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

            _characterController.Move(direction * (moveSpeed * Time.deltaTime));
            FlipSprite(horizontal);
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded && velocity.y < 0)
            {
                velocity.y = groundedGravity;
            }

            velocity.y += gravity * Time.deltaTime;
            _characterController.Move(velocity * Time.deltaTime);
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