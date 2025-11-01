using Gameplay;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class Item : MonoBehaviour
{
    public event UnityAction Picked;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) is false)
            return;

        OnPlayerEnter(player);
    }

    protected abstract void OnPlayerEnter(Player player);

    protected void OnPicked() => Picked?.Invoke();
}