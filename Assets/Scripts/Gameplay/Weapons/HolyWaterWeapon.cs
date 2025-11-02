using System;
using UnityEngine;
using Gameplay.Projectiles;

namespace Gameplay.Weapons
{
    public class HolyWaterWeapon : MonoBehaviour
    {
        public event Action OnAttack;
        
        [SerializeField] private HolyWaterProjectile projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float projectileSpeed = 10f;

        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform groundLevel;

        private float _nextFireTime;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= _nextFireTime)
            {
                Debug.Log("Holy Water Fired");
                Fire();
                OnAttack?.Invoke();
                _nextFireTime = Time.time + cooldown;
            }
        }

        private void Fire()
        {
            Vector3 position = GetMousePosition();
            position.y = groundLevel.position.y;
            Vector3 direction = (position - firePoint.position).normalized;
            float distance = Vector3.Distance(firePoint.position, position);

            float lifetime = distance / projectileSpeed;

            var projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.Init(damage, projectileSpeed, lifetime, direction);
        }

        private Vector3 GetMousePosition()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, groundLevel.transform.position.y, 0));

            if (horizontalPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return ray.origin + ray.direction * 50f;
        }
    }
}