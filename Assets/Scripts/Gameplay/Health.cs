using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")]
        [SerializeField]
        private float _maxHealth = 10f;

        [Header("Sound Settings")]
        [SerializeField] private AudioClip DamageSound;
        [SerializeField] private float DamageVolume = 1f;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float soundCooldown = 0.1f;

        private float _lastSoundTime;
        private static float _globalLastSoundTime; // Статическая переменная для всех экземпляров

        public float MaxHealth => _maxHealth;

        public delegate void DamageEventHandler(float damage, [CanBeNull] GameObject damageSource);
        public delegate void HealEventHandler(float healAmount);
        public delegate void DeathEventHandler();

        public event DamageEventHandler OnDamaged;
        public event HealEventHandler OnHealed;
        public event DeathEventHandler OnDie;

        public float CurrentHealth { get; private set; }
        public bool Invincibility { get; set; }

        bool _isDead;

        void Start()
        {
            CurrentHealth = MaxHealth;

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            }
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincibility)
                return;

            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            OnDamaged?.Invoke(damage, damageSource);

            PlayDamageSound();

            HandleDeath();
        }

        public void PlayDamageSound()
        {
            if (DamageSound == null) return;

            float currentTime = Time.time;

            // Проверяем задержки
            if (currentTime - _lastSoundTime < soundCooldown ||
                currentTime - _globalLastSoundTime < soundCooldown)
                return;

            AudioSource.PlayClipAtPoint(DamageSound, transform.position, DamageVolume);

            _lastSoundTime = currentTime;
            _globalLastSoundTime = currentTime;
        }

        public void Heal(float healAmount)
        {
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
        }

        public void Kill()
        {

            CurrentHealth = 0f;
            OnDamaged?.Invoke(MaxHealth, null);
            HandleDeath();
        }

        void HandleDeath()
        {
            if (_isDead)
                return;

            if (CurrentHealth <= 0f)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        }
    }
}