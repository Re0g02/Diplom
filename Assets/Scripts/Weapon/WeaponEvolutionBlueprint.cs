using UnityEngine;
[CreateAssetMenu(fileName = "WeaponEvolutionBlueprint", menuName = "ScriptableObjects/WeaponEvolutionBlueptint")]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    [SerializeField] private WeaponScriptableObject baseWeaponStats;
    [SerializeField] private PassiveItemScriptableObject basePassiveItemStats;
    [SerializeField] private int weaponRequiredLevel;
    [SerializeField] private int passiveItemRequiredLevel;
    [SerializeField] private WeaponScriptableObject evolvedWeaponStats;
    [SerializeField] private GameObject evolvedWeaponPrefab;
    public WeaponScriptableObject BaseWeaponStats { get => baseWeaponStats; }
    public PassiveItemScriptableObject BasePassiveItemStats { get => basePassiveItemStats; }
    public int WeaponRequiredLevel { get => weaponRequiredLevel; }
    public int PassiveItemRequiredLevel { get => passiveItemRequiredLevel; }
    public WeaponScriptableObject EvolvedWeaponStats { get => evolvedWeaponStats; }
    public GameObject EvolvedWeaponPrefab { get => evolvedWeaponPrefab; }
}
