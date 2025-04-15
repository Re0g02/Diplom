using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyStats;
    [SerializeField] private float relocateDistance;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;
    private Transform playerTransform;
    private EnemySpawner enemySpawner;

    [SerializeField] private Color _damagedColor = new Color(1, 0, 0, 1);
    [SerializeField] private float _damageFlashDuration = 0.2f;
    [SerializeField] private float _deathFadeDuration = 0.6f;
    private Color originalColor;
    private SpriteRenderer srComponent;
    private EnemyMovement movement;

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
        srComponent = GetComponent<SpriteRenderer>();
        originalColor = srComponent.color;
        movement = GetComponent<EnemyMovement>();
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) >= relocateDistance)
        {
            RelocateEnemy();
        }
    }
    public void TakeDamage(float damage, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= damage;
        StartCoroutine(DamageFlash());

        if (damage > 0) GameManager.GenerateDamageText(Mathf.FloorToInt(damage).ToString(), transform, 1f);
        if (knockbackForce > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

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
        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float origAlpha = srComponent.color.a;

        while (t < _deathFadeDuration)
        {
            yield return w;
            t += Time.deltaTime;
            srComponent.color = new Color(srComponent.color.r,
                                            srComponent.color.g,
                                            srComponent.color.b,
                                            (1 - t / _deathFadeDuration) * origAlpha);
        }

        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        srComponent.color = _damagedColor;
        yield return new WaitForSeconds(_damageFlashDuration);
        srComponent.color = originalColor;
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