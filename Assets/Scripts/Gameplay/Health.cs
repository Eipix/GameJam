using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")] [SerializeField]
        private float _maxHealth = 10f;

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
        }

        public void Heal(float healAmount)
        {
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            // call OnHeal event
            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincibility)
                return;

            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
            //Debug.Log(CurrentHealth);
            OnDamaged?.Invoke(damage, damageSource);
            
            HandleDeath();
        }

        public void Kill()
        {
            CurrentHealth = 0f;

            // call OnDamage event
            OnDamaged?.Invoke(MaxHealth, null);

            HandleDeath();
        }

        void HandleDeath()
        {
            if (_isDead)
                return;

            // call OnDie event
            if (CurrentHealth <= 0f)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        }
    }
}