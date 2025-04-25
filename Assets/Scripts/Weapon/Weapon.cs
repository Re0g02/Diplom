using UnityEngine;

public abstract class Weapon : Item
{
    [System.Serializable]
    public struct Stats
    {
        public string name;
        public string description;
        public Projectile projectilePrefab;
        public Aura auraPrefab;
        public ParticleSystem hitEffect;
        public Rect spawnVariance;
        public float lifeTime;
        public float damage;
        public float damageVariance;
        public float damageArea;
        public float speed;
        public float cooldown;
        public float projectileInterval;
        public float knockback;
        public int projectileQuantity;
        public int maxProjectileQuantity;
        public int punching;

        public static Stats operator +(Stats l, Stats r)
        {
            var res = new Stats();
            res.name = r.name ?? l.name;
            res.description = r.description ?? l.description;
            res.projectilePrefab = r.projectilePrefab ?? l.projectilePrefab;
            //res.auraPrefab = r.auraPrefab ?? l.auraPrefab;
            res.hitEffect = r.hitEffect ?? l.hitEffect;
            res.spawnVariance = r.spawnVariance != null ? r.spawnVariance : l.spawnVariance;
            res.lifeTime = r.lifeTime + l.lifeTime;
            res.damage = r.damage + l.damage;
            res.damageVariance = r.damageVariance + l.damageVariance;
            res.damageArea = r.damageArea + l.damageArea;
            res.speed = r.speed + l.speed;
            res.cooldown = r.cooldown + l.cooldown;
            res.projectileInterval = r.projectileInterval + l.projectileInterval;
            res.knockback = r.knockback + l.knockback;
            res.projectileQuantity = r.projectileQuantity + l.projectileQuantity;
            res.maxProjectileQuantity = r.maxProjectileQuantity + l.maxProjectileQuantity;
            res.punching = r.punching + l.punching;
            return res;
        }

        public float GetFinalDamage()
        {
            return damage + Random.Range(0, damageVariance);
        }
    }

    [SerializeField] protected WeaponDataScriptableObject weaponData;
    protected float currentCooldown;
    protected Stats weaponStats;
    protected PlayerMovement playerMovement;
    public WeaponDataScriptableObject WeaponData { get => weaponData; }

    public virtual void Initialise(WeaponDataScriptableObject weaponData)
    {
        base.Initialise(weaponData);
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        this.weaponData = weaponData;
        weaponStats = weaponData.BaseStats;
        currentCooldown = weaponStats.cooldown;
    }

    protected virtual void Awake()
    {
        if (weaponData) weaponStats = weaponData.BaseStats;
    }

    protected virtual void Start()
    {
        if (weaponData)
        {
            Initialise(weaponData);
        }
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack(weaponStats.projectileQuantity);
        }
    }

    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        if (!CanLevelUp()) return false;
        weaponStats += weaponData.GetLevelUpData(++currentLevel);
        return true;
    }

    protected virtual bool CanAttack()
    {
        return currentCooldown <= 0;
    }

    protected virtual bool Attack(int attackCount = 1)
    {
        if (!CanAttack()) return false;

        currentCooldown += weaponStats.cooldown;
        return true;
    }

    public virtual float GetFinalFinalDamage()
    {
        return weaponStats.GetFinalDamage() * playerStats.CurrentMight;
    }

    public virtual Stats GetStats()
    {
        return weaponStats;
    }
}
