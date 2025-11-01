using Gameplay;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Item : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player) is false)
            return;

        OnPlayerEnter(player);
    }

    protected abstract void OnPlayerEnter(Player player);
}