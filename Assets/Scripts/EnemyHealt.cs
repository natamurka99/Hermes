using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    public int currentHealth;
    public int xpReward = 15;

    // Evento de instancia (para WaveManager)
    public event Action onDied;

    // Evento estático (para HealOnKill)
    public static event Action onAnyEnemyDied;

    private MomentumBar _momentum;
    private XPManager _xp;

    void Start()
    {
        currentHealth = maxHealth;
        var player = GameObject.FindWithTag("Player");
        _momentum = player?.GetComponent<MomentumBar>();
        _xp = player?.GetComponent<XPManager>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        _momentum?.OnEnemyKilled();
        _xp?.GainXP(xpReward);
        onDied?.Invoke();
        onAnyEnemyDied?.Invoke();
        Destroy(gameObject);
    }
}