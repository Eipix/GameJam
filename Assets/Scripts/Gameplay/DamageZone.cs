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
        // [Tooltip("Interval between damage ticks in seconds")]
        // [SerializeField] private float _interval = 1f;

        // private readonly HashSet<Health> _inside = new HashSet<Health>();
        // private bool _isDamaging;

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(_damage, gameObject);
            }
            
            // if (other.TryGetComponent<Health>(out var health))
            // {
            //     _inside.Add(health);
            //     health.OnDie += () => _inside.Remove(health);
            // }
            //
            // if (!_isDamaging && _inside.Count > 0)
            // {
            //     InvokeRepeating(nameof(ApplyDamage), 0f, _interval);
            //     _isDamaging = true;
            // }
        }
    //
    //     void OnTriggerExit(Collider other)
    //     {
    //         if (other.TryGetComponent<Health>(out var health))
    //         {
    //             _inside.Remove(health);
    //         }
    //
    //         if (_inside.Count == 0 && _isDamaging)
    //         {
    //             CancelInvoke(nameof(ApplyDamage));
    //             _isDamaging = false;
    //         }
    //     }
    //
    //     private void ApplyDamage()
    //     {
    //         _inside.RemoveWhere(h => h == null);
    //
    //         if (_inside.Count == 0)
    //         {
    //             CancelInvoke(nameof(ApplyDamage));
    //             _isDamaging = false;
    //             return;
    //         }
    //
    //         foreach (var health in _inside.ToArray())
    //         {
    //             Debug.Log(_damage);
    //             if (health != null)
    //                 health.TakeDamage(_damage, gameObject);
    //         }
    //     }
    //
    //     void OnDisable()
    //     {
    //         if (_isDamaging)
    //         {
    //             CancelInvoke(nameof(ApplyDamage));
    //             _isDamaging = false;
    //         }
    //         
    //         _inside.Clear();
    //     }
    }
}
