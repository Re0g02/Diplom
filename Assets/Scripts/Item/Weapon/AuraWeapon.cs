using UnityEngine;

public class AuraWeapon : Weapon
{
    protected Aura currentAura;

    protected override void Update() { }

    public override void OnEquip()
    {
        if (weaponStats.auraPrefab)
        {
            if (currentAura) Destroy(currentAura);
            currentAura = Instantiate(weaponStats.auraPrefab, transform);
            currentAura.Weapon = this;
            currentAura.PlayerStats = playerStats;
        }
    }

    public override void OnUnequip()
    {
        if (currentAura) Destroy(currentAura);
    }

    public override bool DoLevelUp()
    {
        if (!base.DoLevelUp()) return false;

        if (currentAura)
        {
            currentAura.transform.localScale = new Vector3(weaponStats.damageArea, weaponStats.damageArea, weaponStats.damageArea);
        }

        return true;
    }
}
