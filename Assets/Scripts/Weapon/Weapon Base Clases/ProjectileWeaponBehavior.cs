using UnityEngine;

public class ProjectileWeaponBehavior : MonoBehaviour
{

    [SerializeField] protected WeaponScriptableObject _weaponStats;

    protected Vector3 projectileDirection;

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
    virtual protected void Start()
    {
        Destroy(gameObject, currentLifetime);
    }

    public void DirectionCheker(Vector3 dir)
    {
        projectileDirection = dir;

        var scale = transform.localScale;
        var rotation = transform.rotation.eulerAngles;

        scale.x *= dir.x < 0 ? -1 : 1;
        rotation.z += dir.y == 0 && dir.x > 0 ? -45 : 0;
        rotation.z += dir.y < 0 && dir.x > 0 ? -90 : 0;
        rotation.z += dir.y > 0 && dir.x == 0 ? 45 : 0;
        rotation.z += dir.y < 0 && dir.x == 0 ? -135 : 0;
        rotation.z += dir.y == 0 && dir.x < 0 ? 45 : 0;
        rotation.z += dir.y < 0 && dir.x < 0 ? 90 : 0;

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyMovement>())
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(GetCurrentDamage());
            ReducePierce();
        }
        else if (other.TryGetComponent<BreakableProps>(out BreakableProps breakable))
        {
            breakable.TakeDamage(GetCurrentDamage());
            ReducePierce();
        }

    }

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindFirstObjectByType<PlayerStats>().CurrentMight;
    }
}
