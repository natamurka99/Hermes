using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject enemyPrefab;
        public int enemyCount;
        public float spawnInterval = 0.8f;
    }

    [Header("Waves")]
    public List<Wave> waves = new List<Wave>();

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public TextMeshProUGUI waveCounterText;

    // Estado
    private int _currentWave = 0;
    private int _enemiesAlive = 0;
    private bool _waveInProgress = false;
    private bool _allWavesCleared = false;

    // Evento para el GameManager
    public static event System.Action onAllWavesCleared;

    void Start()
    {
        StartCoroutine(StartWave(_currentWave));
    }

    void Update()
    {
        // Cuando no hay enemigos vivos y la oleada terminó de spawnear
        if (!_waveInProgress && _enemiesAlive == 0 && !_allWavesCleared)
            NextWave();
    }


    IEnumerator StartWave(int index)
    {
        _waveInProgress = true;
        Wave wave = waves[index];

        UpdateWaveUI();
        Debug.Log($"Oleada {index + 1}: {wave.waveName}");

        // Pequeña pausa antes de spawnear
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        _waveInProgress = false;
    }

    void NextWave()
    {
        _currentWave++;

        if (_currentWave >= waves.Count)
        {
            _allWavesCleared = true;
            Debug.Log("¡Todas las oleadas completadas!");
            onAllWavesCleared?.Invoke();
            return;
        }

        GameManager.Instance?.OnWaveCleared();

        StartCoroutine(StartWave(_currentWave));
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No hay spawn points asignados");
            return;
        }

        // Punto de spawn aleatorio
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

        // Suscribirse a la muerte del enemigo
        enemy.GetComponent<EnemyHealth>().onDied += OnEnemyDied;
        _enemiesAlive++;
    }

    void OnEnemyDied()
    {
        _enemiesAlive = Mathf.Max(0, _enemiesAlive - 1);
    }

    void UpdateWaveUI()
    {
        if (waveCounterText == null) return;
        waveCounterText.text = $"Wave {_currentWave + 1}/{waves.Count}";
    }
}
