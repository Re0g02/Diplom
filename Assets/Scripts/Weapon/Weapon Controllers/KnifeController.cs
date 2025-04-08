public class KnifeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        var spawnedKnife = Instantiate(_weaponStats.prefab, transform.position, _weaponStats.prefab.transform.rotation);
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionCheker(playerMovementScript.lastMoveVector);
    }
}