using UnityEngine;

[CreateAssetMenu(fileName = "DivinePowerUp", menuName = "HermesGo/DivinePowerUp")]
public class DivinePowerUpData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int maxLevel = 5;
    public int costPerLevel = 80;

    public enum UpgradeType
    {
        MaxHealth,
        MoveSpeed,
        AttackDamage,
        AttackSpeed,
        MomentumGain,
        StasisDuration,
    }
    public UpgradeType upgradeType;
    public float valuePerLevel; // cuánto suma cada nivel
}
