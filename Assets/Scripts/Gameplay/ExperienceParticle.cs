using Gameplay;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class ExperienceParticle : MonoBehaviour
{
    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.TryGetComponent(out Player player) {
    }
}
