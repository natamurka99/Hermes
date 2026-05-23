using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;
    public Image fillImage;
    public Color normalColor = Color.green;
    public Color dangerColor = Color.red;

    private MomentumBar _momentum;

    void Start()
    {
        currentHealth = maxHealth;
        _momentum = GetComponent<MomentumBar>();
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        // Curación si amount es negativo
        if (amount < 0)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Abs(amount));
            UpdateUI();
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        _momentum?.OnPlayerHit();
        GameManager.Instance?.OnPlayerHit();
        UpdateUI();

        if (currentHealth <= 0) Die();
    }

    void UpdateUI()
    {
        if (healthSlider == null) return;
        healthSlider.value = (float)currentHealth / maxHealth;
        if (fillImage != null)
            fillImage.color = healthSlider.value < 0.25f ? dangerColor : normalColor;
    }

    void Die()
    {
        Debug.Log("Die() llamado");
        Debug.Log("GameManager.Instance: " + (GameManager.Instance != null));
        GameManager.Instance?.OnPlayerDeath();
        gameObject.SetActive(false);
    }
}
