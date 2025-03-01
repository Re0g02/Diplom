using UnityEngine;

public class KnifeBehavior : ProjectileWeaponBehavior
{
    void Update()
    {
        transform.position += projectileDirection * _weaponStats.speed * Time.deltaTime;
    }
}
