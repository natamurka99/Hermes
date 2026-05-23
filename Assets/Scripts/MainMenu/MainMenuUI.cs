using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject mainPanel;
    public GameObject selectLevelPanel;

    [Header("Main Panel - Botones")]
    public Button btnPlay;
    public Button btnDivineUpgrades;

    [Header("Select Level Panel")]
    public Button btnLevel1;
    public Button btnLevel2;
    public Button btnBack;
    public TextMeshProUGUI level2LockedText;

    // Niveles desbloqueados (por ahora hardcoded, luego usará PlayerPrefs)
    private bool _level2Unlocked = false;

    public DivinUpgradePanel divineUpgradePanel;

    void Start()
    {
        // Carga si level2 está desbloqueado
        _level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        // Main Panel
        btnPlay.onClick.AddListener(ShowSelectLevel);
        btnDivineUpgrades.onClick.AddListener(ShowDivineUpgrades);

        // Select Level
        btnLevel1.onClick.AddListener(() => StartLevel(1));
        btnLevel2.onClick.AddListener(OnLevel2Click);
        btnBack.onClick.AddListener(ShowMainPanel);

        // Estado inicial
        ShowMainPanel();
        UpdateLevelButtons();
    }


    void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
    }

    void ShowSelectLevel()
    {
        mainPanel.SetActive(false);
        selectLevelPanel.SetActive(true);
    }

    void ShowDivineUpgrades()
    {
        mainPanel.SetActive(false);
        divineUpgradePanel.Show();
    }

    public void ShowMainPanelPublic()
    {
        mainPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
    }

    void OnLevel2Click()
    {
        if (_level2Unlocked)
            StartLevel(2);
        else
            level2LockedText.text = "Complete Level 1 first!";
    }

    void StartLevel(int level)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.StartLevel(level);
        else
            SceneManager.LoadScene("Level" + level);
    }

    void UpdateLevelButtons()
    {
        // Level 2 visualmente bloqueado si no está desbloqueado
        Color lockedColor = new Color(1f, 1f, 1f, 0.4f);
        Color normalColor = Color.white;

        btnLevel2.image.color = _level2Unlocked ? normalColor : lockedColor;
        level2LockedText.gameObject.SetActive(!_level2Unlocked);
    }
}
