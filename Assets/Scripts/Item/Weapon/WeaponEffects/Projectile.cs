using UnityEngine;
using Random = UnityEngine.Random;
public class Projectile : WeaponEffect
{
    private enum DamageSource { projectile, player };
    [SerializeField] private DamageSource damageSource = DamageSource.projectile;
    [SerializeField] private bool hasAutoAim = false;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 0, 0);
    protected Rigidbody2D weaponRB;
    protected int punching;

    protected virtual void Awake()
    {
        weaponRB = GetComponent<Rigidbody2D>();
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    public void Intitialize(Weapon w, PlayerStats p)
    {
        weapon = w;
        playerStats = p;
        Weapon.Stats stats = weapon.GetStats();
        if (weaponRB.bodyType == RigidbodyType2D.Dynamic)
        {
            weaponRB.angularVelocity = rotationSpeed.z;
            weaponRB.linearVelocity = transform.right * stats.speed;
        }
        float area = stats.damageArea == 0 ? 1 : stats.damageArea;
        transform.localScale = new Vector3(area * Mathf.Sign(transform.localScale.x),
                                           area * Mathf.Sign(transform.localScale.y), 1);

        punching = stats.punching;
        if (stats.lifeTime > 0) Destroy(gameObject, stats.lifeTime);
        if (hasAutoAim) AcquireAutoFacing();
    }

    protected virtual void FixedUpdate()
    {
        if (weapon == null) return;
        if (weaponRB.bodyType == RigidbodyType2D.Kinematic)
        {
            Weapon.Stats stats = weapon.GetStats();
            transform.position += transform.right * stats.speed * Time.fixedDeltaTime;
            weaponRB.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void AcquireAutoFacing()
    {
        float attackAngle;
        EnemyStats[] targets = FindObjectsByType<EnemyStats>(FindObjectsSortMode.None);
        if (targets.Length > 0)
        {
            var randomTarget = targets[Random.Range(0, targets.Length)];
            var attackDirection = randomTarget.transform.position - transform.position;
            attackAngle = Mathf.Atan2(attackDirection.x, attackDirection.y) * Mathf.Rad2Deg;
        }
        else
        {
            attackAngle = Random.Range(0f, 360f);
        }

        transform.rotation = Quaternion.Euler(0, 0, attackAngle);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyStats>();
        var prop = other.GetComponent<BreakableProps>();

        if (enemy)
        {
            Vector3 sorce = damageSource == DamageSource.player && playerStats ? playerStats.transform.position : transform.position;
            enemy.TakeDamage(GetDamage(), sorce);

            Weapon.Stats stats = weapon.GetStats();
            punching--;
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity));
            }
        }

        if (prop)
        {
            prop.TakeDamage(GetDamage());

            Weapon.Stats stats = weapon.GetStats();
            punching--;
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity));
            }
        }

        if (punching <= 0) Destroy(gameObject);
    }
}
