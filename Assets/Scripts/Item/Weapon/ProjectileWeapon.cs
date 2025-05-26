using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileWeapon : Weapon
{
    protected float currentAttackInterval;
    protected int currentAttackCount;
    protected override void Update()
    {
        base.Update();

        if (currentAttackInterval > 0)
        {
            currentAttackInterval -= Time.deltaTime;
            if (currentAttackInterval <= 0) Attack(currentAttackCount);
        }
    }

    protected override bool CanAttack()
    {
        if (currentAttackCount > 0) return true;
        return base.CanAttack();
    }

    protected override bool Attack(int attackCount = 1)
    {
        if (!weaponStats.projectilePrefab)
        {
            currentCooldown = weaponData.BaseStats.cooldown;
            return false;
        }

        if (!CanAttack()) return false;

        var attackAngle = GetAttackAngle();
        Projectile prefab = Instantiate(weaponStats.projectilePrefab,
                                        playerStats.transform.position + (Vector3)GetAttackOffset(attackAngle),
                                        Quaternion.Euler(0, 0, attackAngle));
        prefab.Intitialize(this, playerStats);

        if (currentCooldown <= 0)
            currentCooldown += weaponStats.cooldown;
        attackCount--;

        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = weaponData.BaseStats.projectileInterval;
        }

        return true;
    }

    protected virtual float GetAttackAngle()
    {
        return Mathf.Atan2(playerMovement.lastMoveVector.y, playerMovement.lastMoveVector.x) * Mathf.Rad2Deg;
    }

    protected virtual Vector2 GetAttackOffset(float attackAngle = 0)
    {
        return Quaternion.Euler(0, 0, attackAngle) * new Vector2(Random.Range(weaponStats.spawnVariance.xMin, weaponStats.spawnVariance.xMax),
                                                                Random.Range(weaponStats.spawnVariance.yMin, weaponStats.spawnVariance.yMax));
    }

    protected virtual void OnDestroy()
    {
        var list = FindObjectsByType<Projectile>(FindObjectsSortMode.None);
        foreach (var i in list)
        {
            Destroy(i.gameObject);
        }
    }

}
