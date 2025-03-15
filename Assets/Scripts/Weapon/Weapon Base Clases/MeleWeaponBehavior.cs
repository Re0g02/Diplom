using UnityEngine;

public class MeleWeaponBehavior : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject _weaponStats;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCdDuration;
    protected float currentLifetime;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = _weaponStats.damage;
        currentSpeed = _weaponStats.speed;
        currentCdDuration = _weaponStats.cdDuration;
        currentLifetime = _weaponStats.lifetime;
        currentPierce = _weaponStats.pierce;

    }

    protected virtual void Start()
    {
        Destroy(gameObject, _weaponStats.lifetime);
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyMovement>())
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(GetCurrentDamage());
        }
        else if (other.TryGetComponent<BreakableProps>(out BreakableProps breakable))
        {
            breakable.TakeDamage(GetCurrentDamage());
        }
    }
    public float GetCurrentDamage()
    {
        return currentDamage *= FindFirstObjectByType<PlayerStats>().CurrentMight;
    }
}

