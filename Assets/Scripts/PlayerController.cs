using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Attack")]
    public float attackRange = 5f;
    public float attackCooldown = 0.5f;
    public GameObject projectilePrefab;

    // Componentes
    private Rigidbody _rb;
    private Camera _cam;
    private MomentumBar _momentum;

    // Estado
    private Vector2 _joystickInput;
    private bool _isMoving;
    private float _attackTimer;

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _momentum = GetComponent<MomentumBar>();

        _rb.constraints = RigidbodyConstraints.FreezeRotation
                        | RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        HandleInput();
        _attackTimer -= Time.deltaTime;

        bool canAttack = !_isMoving || (_momentum != null && _momentum.InStasis);

        if (canAttack && _attackTimer <= 0f)
            TryAttack();
    }

    void FixedUpdate() => Move();

    void HandleInput()
    {
#if UNITY_EDITOR
        // WASD
        float h = 0f, v = 0f;
        if (Input.GetKey(KeyCode.W)) v = 1f;
        if (Input.GetKey(KeyCode.S)) v = -1f;
        if (Input.GetKey(KeyCode.A)) h = -1f;
        if (Input.GetKey(KeyCode.D)) h = 1f;

        if (h != 0f || v != 0f)
        {
            _isMoving = true;
            _joystickInput = new Vector2(h, v).normalized;
        }
        else
        {
            _isMoving = false;
            _joystickInput = Vector2.zero;
        }
        return;
#endif
        // Móvil: touch (código anterior sin cambios)
        var touches = Touch.activeTouches;
        if (touches.Count == 0)
        {
            _isMoving = false;
            _joystickInput = Vector2.zero;
            return;
        }
        foreach (var touch in touches)
        {
            bool inLowerHalf = touch.screenPosition.y < Screen.height * 0.5f;
            if (!inLowerHalf) continue;
            Vector3 worldPos = ScreenToWorld(touch.screenPosition);
            Vector2 dir = new Vector2(worldPos.x - transform.position.x,
                                      worldPos.z - transform.position.z);
            _isMoving = dir.magnitude > 0.3f;
            _joystickInput = dir.normalized;
            return;
        }
        _isMoving = false;
        _joystickInput = Vector2.zero;
    }


    void Move()
    {
        if (!_isMoving)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 velocity = new Vector3(_joystickInput.x, 0f, _joystickInput.y)
                         * moveSpeed;
        _rb.linearVelocity = velocity;

        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }


    void TryAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;
            float d = Vector3.Distance(transform.position, hit.transform.position);
            if (d < minDist) { minDist = d; nearest = hit.transform; }
        }

        if (nearest == null || projectilePrefab == null) return;

        Vector3 dir = (nearest.position - transform.position).normalized;
        Instantiate(projectilePrefab,
                    transform.position + dir * 0.6f,
                    Quaternion.LookRotation(dir));
        _attackTimer = attackCooldown;
    }


    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0));
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float dist);
        return ray.GetPoint(dist);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}