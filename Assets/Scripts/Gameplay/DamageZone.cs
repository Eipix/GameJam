using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class DamageZone : MonoBehaviour
    {
        [Tooltip("Damage per tick")]
        [SerializeField] private float _damage = 1f;
        [Tooltip("Interval between damage ticks in seconds")]
        [SerializeField] private float _interval = 1f;

        private readonly HashSet<Health> _inside = new HashSet<Health>();
        private Coroutine _damageCoroutine;

        void Awake()
        {
            var col = GetComponent<Collider>();
            if (col)
                col.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Health>(out var health))
                _inside.Add(health);

            if (_damageCoroutine == null && _inside.Count > 0)
                _damageCoroutine = StartCoroutine(DamageLoop());
        }

        void OnTriggerExit(Collider other)
        {
            var health = other.GetComponentInParent<Health>();
            if (health)
                _inside.Remove(health);

            if (_inside.Count == 0 && _damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }
        }

        private IEnumerator DamageLoop()
        {
            var wait = new WaitForSeconds(_interval);
            while (_inside.Count > 0)
            {
                var targets = _inside.ToArray();
                
                foreach (var health in targets)
                {
                    if (health)
                        health.TakeDamage(_damage, gameObject);
                }
                yield return wait;
            }
            _damageCoroutine = null;
        }

        void OnDisable()
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }
            _inside.Clear();
        }
    }
}