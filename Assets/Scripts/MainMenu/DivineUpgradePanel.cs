using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DivinUpgradePanel : MonoBehaviour
{
    [Header("Puntos")]
    public TextMeshProUGUI pointsText;

    [Header("Back")]
    public Button btnBack;

    [Header("Reset")]
    public Button btnReset;

    [Header("Vitality")]
    public TextMeshProUGUI vitalityName;
    public TextMeshProUGUI vitalityLevel;
    public Button vitalityBtn;

    [Header("Speed")]
    public TextMeshProUGUI speedName;
    public TextMeshProUGUI speedLevel;
    public Button speedBtn;

    [Header("Wrath")]
    public TextMeshProUGUI wrathName;
    public TextMeshProUGUI wrathLevel;
    public Button wrathBtn;

    [Header("Strike")]
    public TextMeshProUGUI strikeName;
    public TextMeshProUGUI strikeLevel;
    public Button strikeBtn;

    [Header("Momentum")]
    public TextMeshProUGUI momentumName;
    public TextMeshProUGUI momentumLevel;
    public Button momentumBtn;

    [Header("Stasis")]
    public TextMeshProUGUI stasisName;
    public TextMeshProUGUI stasisLevel;
    public Button stasisBtn;

    void Start()
    {
        gameObject.SetActive(false);

        btnReset.onClick.AddListener(ResetUpgrades);

        btnBack.onClick.AddListener(Hide);

        vitalityBtn.onClick.AddListener(() => Buy(0));
        speedBtn.onClick.AddListener(() => Buy(1));
        wrathBtn.onClick.AddListener(() => Buy(2));
        strikeBtn.onClick.AddListener(() => Buy(3));
        momentumBtn.onClick.AddListener(() => Buy(4));
        stasisBtn.onClick.AddListener(() => Buy(5));
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        FindFirstObjectByType<MainMenuUI>()?.ShowMainPanelPublic();
    }

    void Buy(int index)
    {
        var manager = DivinUpgradeManager.Instance;
        if (manager == null) return;

        manager.PurchaseUpgrade(manager.allUpgrades[index]);
        Refresh();
    }

    void Refresh()
    {
        var manager = DivinUpgradeManager.Instance;
        if (manager == null)
        {
            Debug.LogWarning("DivinUpgradeManager no encontrado");
            return;
        }

        if (GameManager.Instance != null)
            pointsText.text = $"Points: {GameManager.Instance.totalPoints}";

        RefreshRow(0, vitalityName, vitalityLevel, vitalityBtn);
        RefreshRow(1, speedName, speedLevel, speedBtn);
        RefreshRow(2, wrathName, wrathLevel, wrathBtn);
        RefreshRow(3, strikeName, strikeLevel, strikeBtn);
        RefreshRow(4, momentumName, momentumLevel, momentumBtn);
        RefreshRow(5, stasisName, stasisLevel, stasisBtn);
    }

    void RefreshRow(int index,
                    TextMeshProUGUI nameText,
                    TextMeshProUGUI levelText,
                    Button btn)
    {
        var manager = DivinUpgradeManager.Instance;
        var upgrade = manager.allUpgrades[index];
        int level = manager.GetLevel(upgrade);
        bool maxed = level >= upgrade.maxLevel;
        bool canBuy = manager.CanUpgrade(upgrade);

        nameText.text = upgrade.upgradeName;
        levelText.text = maxed ? "MAX" : $"Lv {level}/{upgrade.maxLevel}  |  {upgrade.costPerLevel} pts";

        btn.interactable = canBuy;
    }
    void ResetUpgrades()
    {
        var manager = DivinUpgradeManager.Instance;
        if (manager == null) return;

        foreach (var upgrade in manager.allUpgrades)
        {
            // Devuelve los puntos gastados
            int level = manager.GetLevel(upgrade);
            GameManager.Instance.totalPoints += level * upgrade.costPerLevel;

            // Resetea el nivel
            PlayerPrefs.SetInt("Divine_" + upgrade.upgradeName, 0);
        }

        PlayerPrefs.Save();
        manager.LoadUpgradesPublic(); // recarga los niveles
        Refresh();

        Debug.Log("Upgrades reseteados");
    }
}
