using UnityEngine;

namespace Gameplay.Actions
{
    public class RotationAroundAxis : MonoBehaviour
    {
        
        [SerializeField] private float rotationSpeed;
        
        void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
        }
    }
}
