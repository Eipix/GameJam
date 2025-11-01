using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Scythe : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 360f;
        [SerializeField] private float attackRadius = 2f;

        [Header("Damage Settings")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float damageInterval = 0.2f;
        [SerializeField] private LayerMask enemyLayerMask = 1;

        [Header("References")]
        [SerializeField] private Transform playerTransform;

        private Collider scytheCollider;
        private float currentAngle = 0f;
        private readonly HashSet<Health> hitEnemies = new HashSet<Health>();
        private Coroutine damageCoroutine;

        private void Awake()
        {
            scytheCollider = GetComponent<Collider>();

            // Автопоиск игрока если не назначен
            if (playerTransform == null)
            {
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }

            // Настройка коллайдера
            if (scytheCollider != null)
            {
                scytheCollider.isTrigger = true;
                scytheCollider.enabled = true; 
            }
        }

        private void Start()
        {

            StartRotation();
            damageCoroutine = StartCoroutine(DamageLoop());
        }

        private void Update()
        {

            RotateScythe();
        }

        /// <summary>
        /// Постоянное вращение косы вокруг игрока
        /// </summary>
        private void RotateScythe()
        {
            if (playerTransform == null) return;

            currentAngle += rotationSpeed * Time.deltaTime;

 
            if (currentAngle >= 360f)
            {
                currentAngle -= 360f;
            }

            // Вычисление позиции на окружности
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * attackRadius;
            transform.position = playerTransform.position + offset;

            // Вращение самой косы для визуального эффекта
            transform.rotation = Quaternion.Euler(0f, -currentAngle, 0f);
        }

        /// <summary>
        /// Запуск постоянного вращения
        /// </summary>
        private void StartRotation()
        {
            // Устанавливаем начальную позицию
            if (playerTransform != null)
            {
                transform.position = playerTransform.position + Vector3.right * attackRadius;
            }
        }

        /// <summary>
        /// Цикл нанесения урона (работает постоянно)
        /// </summary>
        private IEnumerator DamageLoop()
        {
            var wait = new WaitForSeconds(damageInterval);

            while (true)
            {
                if (hitEnemies.Count > 0)
                {
                    // Создаем копию для безопасной итерации
                    var targets = hitEnemies.ToArray();

                    foreach (var health in targets)
                    {
                        if (health != null && IsValidEnemy(health.gameObject))
                        {
                            health.TakeDamage(damage, gameObject);
                        }
                    }
                }
                yield return wait;
            }
        }

        /// <summary>
        /// Обработка входа врагов в зону косы
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // Проверка слоя врага
            if (((1 << other.gameObject.layer) & enemyLayerMask) == 0) return;

            // Проверка что это не игрок
            if (other.GetComponent<Player>() != null) return;

            // Добавляем здоровье врага в список
            if (other.TryGetComponent<Health>(out var health))
            {
                hitEnemies.Add(health);
            }
        }

        /// <summary>
        /// Обработка выхода врагов из зоны косы
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            var health = other.GetComponentInParent<Health>();
            if (health != null && hitEnemies.Contains(health))
            {
                hitEnemies.Remove(health);
            }
        }

        /// <summary>
        /// Проверка что объект валидный враг
        /// </summary>
        private bool IsValidEnemy(GameObject enemy)
        {
            return enemy != null &&
                   enemy != playerTransform?.gameObject &&
                   ((1 << enemy.layer) & enemyLayerMask) != 0;
        }

        /// <summary>
        /// Визуализация радиуса атаки в редакторе
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (playerTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(playerTransform.position, attackRadius);

                // Показываем текущую позицию косы
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, 0.3f);
                Gizmos.DrawLine(playerTransform.position, transform.position);
            }
        }

        /// <summary>
        /// Очистка при уничтожении
        /// </summary>
        private void OnDestroy()
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }

        
    }
}