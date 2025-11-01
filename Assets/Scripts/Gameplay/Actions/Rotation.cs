using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay
{
    public class Rotation : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 360f;

        [Header("References")]
        [SerializeField] private Transform centerTransform;

        private Collider rotationCollider;
        private float currentAngle = 0f;
        private float radiusRotation;

        private void Awake()
        {
            rotationCollider = GetComponent<Collider>();


            if (centerTransform == null)
            {
                centerTransform = transform.parent != null ? transform.parent : CreateCenterPoint();
            }

            if (centerTransform != null)
            {
                radiusRotation = Vector3.Distance(transform.position, centerTransform.position);
            }

            if (rotationCollider != null)
            {
                rotationCollider.isTrigger = true;
                rotationCollider.enabled = true;
            }
        }

        private void Start()
        {
            StartRotation();
        }

        private void Update()
        {
            Rotate();
        }

        /// <summary>
        /// Постоянное вращение вокруг центрального объекта
        /// </summary>
        private void Rotate()
        {
            if (centerTransform == null) return;

            currentAngle += rotationSpeed * Time.deltaTime;

            if (currentAngle >= 360f)
            {
                currentAngle -= 360f;
            }

            // Вычисление позиции на окружности
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * radiusRotation;
            transform.position = centerTransform.position + offset;

            transform.rotation = Quaternion.Euler(0f, -currentAngle, 0f);
        }


        private void StartRotation()
        {
            // Устанавливаем начальную позицию и вычисляем начальный угол
            if (centerTransform != null)
            {
                Vector3 directionToCenter = transform.position - centerTransform.position;
                directionToCenter.y = 0f; // Игнорируем разницу по высоте

                currentAngle = Vector3.SignedAngle(Vector3.right, directionToCenter.normalized, Vector3.up);
                if (currentAngle < 0) currentAngle += 360f;
            }
        }

        /// <summary>
        /// Создает точку центра вращения в текущей позиции объекта
        /// </summary>
        private Transform CreateCenterPoint()
        {
            GameObject centerObject = new GameObject("RotationCenter_" + gameObject.name);
            centerObject.transform.position = transform.position;
            return centerObject.transform;
        }




    }
}