using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Botones")]
    public Button button1, button2, button3;
    public TextMeshProUGUI text1, text2, text3;
    public TextMeshProUGUI rarity1, rarity2, rarity3;

    private List<PowerUpData> _options;
    private PowerUpManager _powerUpManager;

    void Start()
    {
        _powerUpManager = FindFirstObjectByType<PowerUpManager>();
        panel.SetActive(false);

        button1.onClick.AddListener(() => ChoosePowerUp(0));
        button2.onClick.AddListener(() => ChoosePowerUp(1));
        button3.onClick.AddListener(() => ChoosePowerUp(2));
    }

    public void ShowLevelUp()
    {
        _options = _powerUpManager.GetRandomOptions();

        SetButton(text1, rarity1, _options[0]);
        SetButton(text2, rarity2, _options[1]);
        SetButton(text3, rarity3, _options[2]);

        panel.SetActive(true);
        Time.timeScale = 0f; // pausa el juego
    }

    void SetButton(TextMeshProUGUI nameText,
                   TextMeshProUGUI rarityText,
                   PowerUpData data)
    {
        nameText.text = $"{data.powerUpName}\n<size=60%>{data.description}</size>";
        rarityText.text = data.rarity.ToString();

        rarityText.color = data.rarity switch
        {
            PowerUpData.Rarity.Common => Color.white,
            PowerUpData.Rarity.Rare => Color.cyan,
            PowerUpData.Rarity.Epic => Color.magenta,
            _ => Color.white
        };
    }

    void ChoosePowerUp(int index)
    {
        _powerUpManager.ApplyPowerUp(_options[index]);
        panel.SetActive(false);
        Time.timeScale = 1f; // reanuda el juego
    }
}
