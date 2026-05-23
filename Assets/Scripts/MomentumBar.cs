using UnityEngine;
using UnityEngine.UI;

public class MomentumBar : MonoBehaviour
{
    [Header("Stats")]
    public float maxMomentum = 100f;
    public float currentMomentum = 0f;
    public float gainPerKill = 25f;   // cuánto sube al matar
    public float lossOnHit = 30f;   // cuánto baja al recibir daño
    public float stasisDuration = 5f;    // segundos en Stasis State

    [Header("UI")]
    public Slider momentumSlider;
    public Image fillImage;
    public Color normalColor = new Color(0.5f, 0f, 0.8f);
    public Color fullColor = Color.yellow;

    // Estado
    public bool InStasis { get; private set; } = false;
    private float _stasisTimer = 0f;

    void Update()
    {
        if (InStasis)
        {
            _stasisTimer -= Time.deltaTime;
            if (_stasisTimer <= 0f)
                ExitStasis();
        }

        UpdateUI();
    }

    public void OnEnemyKilled()
    {
        if (InStasis) return;

        currentMomentum = Mathf.Min(maxMomentum, currentMomentum + gainPerKill);

        if (currentMomentum >= maxMomentum)
            EnterStasis();
    }

    public void OnPlayerHit()
    {
        if (InStasis) return;
        currentMomentum = Mathf.Max(0f, currentMomentum - lossOnHit);
    }


    void EnterStasis()
    {
        InStasis = true;
        _stasisTimer = stasisDuration;
        currentMomentum = maxMomentum;
        Debug.Log("¡STASIS STATE ACTIVADO!");
    }

    void ExitStasis()
    {
        InStasis = false;
        currentMomentum = 0f;
        Debug.Log("Stasis State terminado");
    }


    void UpdateUI()
    {
        if (momentumSlider == null) return;

        momentumSlider.value = currentMomentum / maxMomentum;

        if (fillImage != null)
            fillImage.color = InStasis ? fullColor : normalColor;
    }
}
