using UnityEngine;

public class XPManager : MonoBehaviour
{
    [Header("Stats")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 50;
    public float xpScaling = 1.5f; // cuánto aumenta el XP necesario por nivel

    private LevelUpUI _levelUpUI;

    void Start()
    {
        _levelUpUI = FindFirstObjectByType<LevelUpUI>();
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"XP: {currentXP}/{xpToNextLevel}");

        if (currentXP >= xpToNextLevel)
            LevelUp();
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpScaling);

        Debug.Log($"¡LEVEL UP! Nivel {currentLevel}");
        _levelUpUI?.ShowLevelUp();
    }
}
