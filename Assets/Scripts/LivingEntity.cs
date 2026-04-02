using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float maxHp = 100f;
    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    public UnityEvent onDead;

    protected virtual void OnEnable()
    {
        Health = maxHp;
        IsDead = false;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health -= damage;

        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }

        Debug.Log($"{damage} 피해 받음");
    }

    public virtual void Die()
    {
        onDead?.Invoke();

        IsDead = true;
    }

    public void SetHealth()
    {
        Health = maxHp;
    }
}
