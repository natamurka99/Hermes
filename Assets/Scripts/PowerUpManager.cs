using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    [Header("Available PowerUps")]
    public List<PowerUpData> allPowerUps = new List<PowerUpData>();

    // Activos en esta run
    private List<PowerUpData> _activePowerUps = new List<PowerUpData>();

    private PlayerController _controller;
    private PlayerHealth _health;
    private MomentumBar _momentum;

    void Start()
    {
        _controller = GetComponent<PlayerController>();
        _health = GetComponent<PlayerHealth>();
        _momentum = GetComponent<MomentumBar>();
    }

    // Devuelve 3 opciones aleatorias (sin repetir)
    public List<PowerUpData> GetRandomOptions()
    {
        List<PowerUpData> pool = new List<PowerUpData>(allPowerUps);
        List<PowerUpData> options = new List<PowerUpData>();

        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            options.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return options;
    }

    public void ApplyPowerUp(PowerUpData data)
    {
        _activePowerUps.Add(data);

        switch (data.effectType)
        {
            case PowerUpData.EffectType.IncreaseSpeed:
                _controller.moveSpeed += data.value;
                break;

            case PowerUpData.EffectType.IncreaseDamage:
                // Busca el proyectil activo y aumenta daño base
                PlayerProjectileStats.baseDamage += (int)data.value;
                break;

            case PowerUpData.EffectType.IncreaseMomentumGain:
                _momentum.gainPerKill += data.value;
                break;

            case PowerUpData.EffectType.IncreaseAttackSpeed:
                _controller.attackCooldown =
                    Mathf.Max(0.1f, _controller.attackCooldown - data.value);
                break;

            case PowerUpData.EffectType.HealOnKill:
                EnemyHealth.onAnyEnemyDied += () =>
                    _health.TakeDamage(-(int)data.value); // daño negativo = curar
                break;
        }

        Debug.Log($"PowerUp aplicado: {data.powerUpName}");
    }

    public void ClearRunPowerUps()
    {
        _activePowerUps.Clear();
    }
}
