using System.Collections.Generic;
using UnityEngine;

public class GarlicBehavior : MeleWeaponBehavior
{
    private List<GameObject> hittedEnemies;
    override protected void Start()
    {
        base.Start();
        hittedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyMovement>() && !hittedEnemies.Contains(other.gameObject))
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(GetCurrentDamage(),transform.position);

            hittedEnemies.Add(other.gameObject);
        }
        else if (other.TryGetComponent<BreakableProps>(out BreakableProps breakable))
        {
            breakable.TakeDamage(GetCurrentDamage());
            hittedEnemies.Add(other.gameObject);
        }
    }
}
