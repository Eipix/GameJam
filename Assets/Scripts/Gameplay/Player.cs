using UnityEngine;


namespace Gameplay
{
    [RequireComponent(typeof(CharacterController), typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        public readonly float DefaultSpeed = 5f;

        [Header("Movement Settings")] [SerializeField]
        private float moveSpeed = 5f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundedGravity = -2f;
    
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
        }

        void Update()
        {
            ApplyGravity();
            HandleMovement();
        }

        public void ChangeSpeed(float speed)
        {
            moveSpeed += speed;
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
    }
}