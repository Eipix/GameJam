using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Weapons;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(SpriteRenderer), typeof(Health))]
    public class Player : MonoBehaviour
    {
        public const float MinSpeed = 1.0f;
        public readonly float DefaultSpeed = 5f;

        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private Cutscene _gameOverCutscene;
        [SerializeField] private EnemySpawner _spawner;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private List<GameObject> weapons = new List<GameObject>();

        [SerializeField] private Sprite StateAndMove;
        [SerializeField] private Sprite Damagable;
        [SerializeField] private Sprite AttackHolyWater;
        
        [Header("Sprite Settings")]
        [SerializeField] private float damageSpriteDuration = 0.5f;
        [SerializeField] private float attackSpriteDuration = 0.3f;

        private Rigidbody _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _spriteCoroutine;
        private Health _health;

        public float Speed => moveSpeed;

        void Start()
        {
            Init();
        }

        void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _health = GetComponent<Health>();

            _spawner.WaveEnded += OnWaveEnded;
            _health.OnDamaged += OnDamaged;
            _health.OnDie += () =>
            {
                _gameOverCutscene.Launch();
                _gameOverCutscene.Ended += () => _mainMenu.Show();
            };
            
            HolyWaterWeapon holyWaterWeapon = null;

            foreach (var weapon in weapons)
            {
                if(weapon.TryGetComponent<HolyWaterWeapon>(out holyWaterWeapon))
                {
                    break;
                }
            }
            
            if (holyWaterWeapon)
            {
                holyWaterWeapon.OnAttack += ShowAttackSprite;
            }
            
            SetDefaultSprite();
        }

        private void OnDamaged(float damage, GameObject damageSource)
        {
            ShowDamageSprite();
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

        private void ShowDamageSprite()
        {
            if (_spriteCoroutine != null)
                StopCoroutine(_spriteCoroutine);
            
            _spriteCoroutine = StartCoroutine(ShowSpriteTemporarily(Damagable, damageSpriteDuration));
        }
        
        private void ShowAttackSprite()
        {
            if (_spriteCoroutine != null)
                StopCoroutine(_spriteCoroutine);
            
            _spriteCoroutine = StartCoroutine(ShowSpriteTemporarily(AttackHolyWater, attackSpriteDuration));
        }

        private IEnumerator ShowSpriteTemporarily(Sprite sprite, float duration)
        {
            _spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(duration);
            SetDefaultSprite();
            _spriteCoroutine = null;
        }

        private void SetDefaultSprite()
        {
            _spriteRenderer.sprite = StateAndMove;
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
                _spriteRenderer.flipX = false;
            else if (horizontal < 0)
                _spriteRenderer.flipX = true;
        }

        private void OnDestroy()
        {
            _spawner.WaveEnded -= OnWaveEnded;
            _health.OnDamaged -= OnDamaged;
            
            var holyWaterWeapon = GetComponentInChildren<Weapons.HolyWaterWeapon>();
            if (holyWaterWeapon)
            {
                holyWaterWeapon.OnAttack -= ShowAttackSprite;
            }
        }
    }
}
