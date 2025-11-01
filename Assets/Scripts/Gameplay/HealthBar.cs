using Gameplay;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Transform _parentBar;

    private Vector3 _defaultScale;

    private void OnEnable()
    {
        _playerHealth.OnDamaged += (_, _) => UpdateParams(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        _playerHealth.OnHealed += (_) => UpdateParams(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
    }

    private void OnDisable()
    {
        _playerHealth.OnDamaged -= (_, _) => UpdateParams(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        _playerHealth.OnHealed -= (_) => UpdateParams(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
    }

    private void Awake()
    {
        _defaultScale = _parentBar.transform.localScale;
    }

    private void UpdateParams(float currentHealth, float maxHealth)
    {
        var parent = _parentBar.transform;
        parent.localScale = new Vector3(currentHealth / maxHealth, parent.localScale.y, parent.localScale.z);
    }

    private void Retore() => _parentBar.transform.localScale = _defaultScale;
}
