using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyStats;

    private float currentMoveSpeed;
    private float currentHealth;
    private float currentDamage;

    void Awake()
    {
        currentMoveSpeed = _enemyStats.speed;
        currentHealth = _enemyStats.health;
        currentDamage = _enemyStats.damage;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    void Kill()
    {
        Destroy(gameObject);
    }
}
