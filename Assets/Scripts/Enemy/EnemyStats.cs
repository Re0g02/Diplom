using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyStats;
    [SerializeField] private float relocateDistance;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;
    private Transform playerTransform;
    private EnemySpawner enemySpawner;

    void Awake()
    {
        currentMoveSpeed = _enemyStats.speed;
        currentHealth = _enemyStats.health;
        currentDamage = _enemyStats.damage;
    }

    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerStats>().transform;
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) >= relocateDistance)
        {
            RelocateEnemy();
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    private void RelocateEnemy()
    {
        transform.position = enemySpawner.GenerateRandomSpawnPosition();
    }

    void Kill()
    {
        Destroy(gameObject);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>())
        {
            var player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        var enemySpawner = FindFirstObjectByType<EnemySpawner>();
        if (enemySpawner != null)
            enemySpawner.OnEnemyKilled();
    }
}
