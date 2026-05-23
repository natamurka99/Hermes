using UnityEngine;

public class RunInitializer : MonoBehaviour
{
    void Start()
    {
        // Resetea stats base antes de aplicar upgrades
        PlayerProjectileStats.baseDamage = 10;

        if (DivinUpgradeManager.Instance != null)
            DivinUpgradeManager.Instance.ApplyAllUpgrades(gameObject);
        else
            Debug.LogWarning("DivinUpgradeManager no encontrado");
    }
}
