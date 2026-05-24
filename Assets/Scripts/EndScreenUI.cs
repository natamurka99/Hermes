using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Victoria")]
    public GameObject victorySection;
    public TextMeshProUGUI victoryTitle;

    [Header("Derrota")]
    public GameObject defeatSection;
    public TextMeshProUGUI defeatTitle;

    [Header("Stats compartidos")]
    public TextMeshProUGUI wavesText;
    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI bonusText;

    [Header("Botones Victoria")]
    public Button btnMainMenuV;
    public Button btnDivineUpgradesV;

    [Header("Botones Derrota")]
    public Button btnRetry;
    public Button btnMainMenuD;
    public Button btnDivineUpgradesD;

    void OnEnable()
    {
        GameManager.onVictory += ShowVictory;
        GameManager.onDefeat += ShowDefeat;
    }

    void OnDisable()
    {
        GameManager.onVictory -= ShowVictory;
        GameManager.onDefeat -= ShowDefeat;
    }

    void Start()
    {
        panel.SetActive(false);

        btnMainMenuV.onClick.AddListener(()
            => GameManager.Instance.GoToMainMenu());
        btnDivineUpgradesV.onClick.AddListener(()
            => GameManager.Instance.GoToMainMenu()); // por ahora al menú

        btnRetry.onClick.AddListener(()
            => GameManager.Instance.RetryLevel());
        btnMainMenuD.onClick.AddListener(()
            => GameManager.Instance.GoToMainMenu());
        btnDivineUpgradesD.onClick.AddListener(()
            => GameManager.Instance.GoToMainMenu());
    }

    public void ShowVictory(int waves, int enemies, int points, bool noDamageBonus)
    {
        panel.SetActive(true);
        Time.timeScale = 0f;

        victorySection.SetActive(true);
        defeatSection.SetActive(false);

        victoryTitle.text = "RUN COMPLETED";
        wavesText.text = $"Waves cleared:     +{waves * 20} pts";
        enemiesText.text = $"Enemies defeated:  +{enemies * 3} pts";
        pointsText.text = $"TOTAL:             +{points} pts";
        bonusText.text = noDamageBonus ? "No damage bonus:   +200 pts" : "";
    }

    public void ShowDefeat(int waves, int enemies, int points)
    {
        Debug.Log("ShowDefeat() llamado");
        Debug.Log("panel: " + (panel != null));
        panel.SetActive(true);
        Time.timeScale = 0f;

        victorySection.SetActive(false);
        defeatSection.SetActive(true);

        defeatTitle.text = "HERMES HAS FALLEN";
        wavesText.text = $"Wave reached:      {waves}";
        enemiesText.text = $"Enemies defeated:  {enemies}";
        pointsText.text = $"Points earned:     +{points} pts";
        bonusText.text = "";
    }


}
