using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int contactDamage = 10;
    public float damageCooldown = 1f;

    private Transform _player;
    private NavMeshAgent _agent;
    private float _damageTimer;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player")?.transform;

        // Configurar el agente para top-down 2D
        _agent.speed = moveSpeed;
        _agent.angularSpeed = 999f;
        _agent.acceleration = 20f;
        _agent.stoppingDistance = 0.5f;

        // Importante: evitar que el NavMeshAgent mueva en Y
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
    }

    void Update()
    {
        if (_player == null) return;

        _agent.SetDestination(_player.position);
        _damageTimer -= Time.deltaTime;

        // Rotar hacia el jugador manualmente
        Vector3 dir = (_player.position - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.forward = new Vector3(dir.x, 0f, dir.z);
    }

    void OnCollisionStay(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        if (_damageTimer > 0f) return;

        col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(contactDamage);
        _damageTimer = damageCooldown;
    }
}
