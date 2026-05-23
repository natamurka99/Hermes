using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estado")]
    public int currentLevel = 1;
    public int totalPoints = 0;
    public bool isGameOver = false;

    // Stats de la run actual
    private int _wavesCleared = 0;
    private int _enemiesDefeated = 0;
    private int _pointsThisRun = 0;
    private bool _tookDamage = false;

    private EndScreenUI _endScreen;

    public int highestLevelUnlocked = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //totalPoints = 500; // <- temporal para testear
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        WaveManager.onAllWavesCleared += OnLevelComplete;
        EnemyHealth.onAnyEnemyDied += OnEnemyDefeated;
    }

    void OnDisable()
    {
        WaveManager.onAllWavesCleared -= OnLevelComplete;
        EnemyHealth.onAnyEnemyDied -= OnEnemyDefeated;
    }

    void Start()
    {
        _endScreen = FindFirstObjectByType<EndScreenUI>();
    }


    void OnEnemyDefeated()
    {
        _enemiesDefeated++;
        _pointsThisRun += 3;
    }

    void OnLevelComplete()
    {
        _endScreen = FindFirstObjectByType<EndScreenUI>();

        int bonus = _tookDamage ? 0 : 200;
        _pointsThisRun += 100 + bonus;
        totalPoints += _pointsThisRun;

        // Desbloquear siguiente nivel
        if (currentLevel >= highestLevelUnlocked)
        {
            highestLevelUnlocked = currentLevel + 1;
            PlayerPrefs.SetInt("Level2Unlocked", 1);
            PlayerPrefs.Save();
        }

        _endScreen?.ShowVictory(_wavesCleared,
                                _enemiesDefeated,
                                _pointsThisRun,
                                bonus > 0);
    }

    public void OnWaveCleared()
    {
        _wavesCleared++;
        _pointsThisRun += 20;
    }

    public void OnPlayerHit()
    {
        _tookDamage = true;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("OnPlayerDeath() llamado");
        Debug.Log("_endScreen: " + (_endScreen != null));
        isGameOver = true;
        _endScreen?.ShowDefeat(_wavesCleared, _enemiesDefeated, _pointsThisRun);
    }

    public void StartLevel(int level)
    {
        currentLevel = level;
        _wavesCleared = 0;
        _enemiesDefeated = 0;
        _pointsThisRun = 0;
        _tookDamage = false;
        isGameOver = false;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Level" + level);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryLevel()
    {
        StartLevel(currentLevel);
    }
}
