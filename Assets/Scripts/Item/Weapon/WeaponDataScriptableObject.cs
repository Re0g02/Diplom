using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "ScriptableObjects/WeaponData")]
public class WeaponDataScriptableObject : ItemDataScriptableObject
{
    private string behaviour;
    [SerializeField] private Weapon.Stats baseStats;
    [SerializeField] private Weapon.Stats[] levelUpStats;
    [SerializeField] private Weapon.Stats[] randomLevelUpStats;

    public Weapon.Stats BaseStats { get => baseStats; }
    public Weapon.Stats[] LevelUpStats { get => levelUpStats; }
    public Weapon.Stats[] RandomLevelUpStats { get => randomLevelUpStats; }
    public string Behaviour { get => behaviour; set => behaviour = value; }
    public Weapon.Stats GetLevelUpData(int level)
    {
        if (level - 2 < levelUpStats.Length) return levelUpStats[level - 2];
        if (randomLevelUpStats.Length > 0) return randomLevelUpStats[Random.Range(0, randomLevelUpStats.Length)];
        return new Weapon.Stats();
    }

}
