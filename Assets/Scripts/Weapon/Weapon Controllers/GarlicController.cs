public class GarlicController : WeaponController
{
    override protected void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        var spawnedGarlic = Instantiate(_weaponStats.prefab, transform.position, transform.rotation);
        spawnedGarlic.transform.parent = transform;
    }
}