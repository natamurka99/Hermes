using UnityEngine;
using System.Collections.Generic;

public class DivinUpgradeManager : MonoBehaviour
{
    public static DivinUpgradeManager Instance { get; private set; }

    [Header("Upgrades disponibles")]
    public List<DivinePowerUpData> allUpgrades = new List<DivinePowerUpData>();

    // Niveles actuales de cada upgrade (persistente)
    private Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUpgradesPublic();
        }
        else Destroy(gameObject);
    }


    public void LoadUpgradesPublic()
    {
        foreach (var upgrade in allUpgrades)
        {
            int level = PlayerPrefs.GetInt("Divine_" + upgrade.upgradeName, 0);
            _upgradeLevels[upgrade.upgradeName] = level;
        }
    }

    void SaveUpgrades()
    {
        foreach (var kvp in _upgradeLevels)
            PlayerPrefs.SetInt("Divine_" + kvp.Key, kvp.Value);
        PlayerPrefs.Save();
    }


    public int GetLevel(DivinePowerUpData upgrade)
    {
        return _upgradeLevels.TryGetValue(upgrade.upgradeName, out int lvl)
               ? lvl : 0;
    }

    public bool CanUpgrade(DivinePowerUpData upgrade)
    {
        int currentLevel = GetLevel(upgrade);
        if (currentLevel >= upgrade.maxLevel) return false;
        return GameManager.Instance != null &&
               GameManager.Instance.totalPoints >= upgrade.costPerLevel;
    }

    public void PurchaseUpgrade(DivinePowerUpData upgrade)
    {
        if (!CanUpgrade(upgrade)) return;

        _upgradeLevels[upgrade.upgradeName]++;
        GameManager.Instance.totalPoints -= upgrade.costPerLevel;
        SaveUpgrades();

        Debug.Log($"{upgrade.upgradeName} ? Nivel {_upgradeLevels[upgrade.upgradeName]}");
    }

    // Aplica todos los upgrades al jugador al inicio de una run
    public void ApplyAllUpgrades(GameObject player)
    {
        var health = player.GetComponent<PlayerHealth>();
        var controller = player.GetComponent<PlayerController>();
        var momentum = player.GetComponent<MomentumBar>();

        foreach (var upgrade in allUpgrades)
        {
            int level = GetLevel(upgrade);
            if (level == 0) continue;

            float totalValue = upgrade.valuePerLevel * level;

            switch (upgrade.upgradeType)
            {
                case DivinePowerUpData.UpgradeType.MaxHealth:
                    health.maxHealth += (int)totalValue;
                    health.currentHealth = health.maxHealth;
                    break;
                case DivinePowerUpData.UpgradeType.MoveSpeed:
                    controller.moveSpeed += totalValue;
                    break;
                case DivinePowerUpData.UpgradeType.AttackDamage:
                    PlayerProjectileStats.baseDamage += (int)totalValue;
                    break;
                case DivinePowerUpData.UpgradeType.AttackSpeed:
                    controller.attackCooldown =
                        Mathf.Max(0.1f, controller.attackCooldown - totalValue);
                    break;
                case DivinePowerUpData.UpgradeType.MomentumGain:
                    momentum.gainPerKill += totalValue;
                    break;
                case DivinePowerUpData.UpgradeType.StasisDuration:
                    momentum.stasisDuration += totalValue;
                    break;
            }
        }
        Debug.Log("Divine Upgrades aplicados al jugador");
    }
}
