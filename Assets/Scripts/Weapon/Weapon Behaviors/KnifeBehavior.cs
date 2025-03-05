using UnityEngine;

public class KnifeBehavior : ProjectileWeaponBehavior
{
    void Update()
    {
        transform.position += projectileDirection * currentSpeed * Time.deltaTime;
    }
}
