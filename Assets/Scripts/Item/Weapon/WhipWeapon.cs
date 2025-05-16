using UnityEngine;

public class WhipWeapon : ProjectileWeapon
{
    int currentSpawnCount;
    float currentSpawnYOffset;
    PlayerAnimator playerAnimator;

    void Start()
    {
        playerAnimator = playerMovement.gameObject.GetComponent<PlayerAnimator>();
    }
    protected override bool Attack(int attackCount = 1)
    {
        if (!weaponStats.projectilePrefab)
        {
            currentCooldown = weaponData.BaseStats.cooldown;
            return false;
        }

        if (!CanAttack()) return false;

        if (currentCooldown <= 0)
        {
            currentSpawnCount = 0;
            currentSpawnYOffset = 0f;
        }

        float spawnDir = Mathf.Sign(playerAnimator.CheckSpriteDirection() ? -1 : 1) * (currentSpawnCount % 2 != 0 ? -1 : 1);
        Vector2 spawnOffset = new Vector2(
            spawnDir * Random.Range(weaponStats.spawnVariance.xMin, weaponStats.spawnVariance.xMax),
            currentSpawnYOffset
        );

        Projectile prefab = Instantiate(
            weaponStats.projectilePrefab,
            playerStats.transform.position + (Vector3)spawnOffset,
            Quaternion.identity
        );
        prefab.Intitialize(this, playerStats);
        prefab.PlayerStats = playerStats;

        if (spawnDir < 0)
        {
            prefab.transform.localScale = new Vector3(
                -Mathf.Abs(prefab.transform.localScale.x),
                prefab.transform.localScale.y,
                prefab.transform.localScale.z
            );
        }

        prefab.Weapon = this;
        currentCooldown = weaponData.BaseStats.cooldown;
        attackCount--;

        currentSpawnCount++;
        if (currentSpawnCount > 1 && currentSpawnCount % 2 == 0)
            currentSpawnYOffset += 1;

        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = weaponData.BaseStats.projectileInterval;
        }
        return true;
    }

}