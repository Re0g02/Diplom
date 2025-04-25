using UnityEngine;

public abstract class WeaponEffect : MonoBehaviour
{
    protected PlayerStats playerStats;
    protected Weapon weapon;

    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }
    public Weapon Weapon { get => weapon; set => weapon = value; }
    public float GetDamage()
    {
        return weapon.GetFinalFinalDamage();
    }
}
