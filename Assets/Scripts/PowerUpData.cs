using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "HermesGo/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string powerUpName;
    public string description;
    public enum Rarity { Common, Rare, Epic }
    public Rarity rarity;
    public enum EffectType
    {
        IncreaseDamage,
        IncreaseSpeed,
        IncreaseMomentumGain,
        IncreaseAttackSpeed,
        HealOnKill,
    }
    public EffectType effectType;
    public float value; // cuánto aplica el efecto
}
