using UnityEngine;

public class AxeWeapon : ProjectileWeapon
{
    protected override float GetAttackAngle()
    {
        int offset = currentAttackCount > 0 ? weaponStats.projectileQuantity - currentAttackCount : 0;
        return 90f - Mathf.Sign(playerMovement.lastMoveVector.x) * (5 * offset);

    }

    protected override Vector2 GetAttackOffset(float spawnAngle = 0)
    {
        return new Vector2(
            Random.Range(weaponStats.spawnVariance.xMin, weaponStats.spawnVariance.xMax),
            Random.Range(weaponStats.spawnVariance.yMin, weaponStats.spawnVariance.yMax)
        );
    }
}