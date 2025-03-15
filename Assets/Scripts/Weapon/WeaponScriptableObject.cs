using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject _weaponPrefab;
    [SerializeField] private Sprite _weaponIcon;
    [SerializeField] private float _weaponDamage;
    [SerializeField] private float _weaponSpeed;
    [SerializeField] private float _weaponCdDuration;
    [SerializeField] private float _weaponLifetime;
    [SerializeField] private int _weaponPierce;
    [SerializeField] private int _weaponLevel;
    [SerializeField] private GameObject _weaponNextLevelPrefab;
    public GameObject prefab { get => _weaponPrefab; private set => prefab = value; }
    public Sprite icon { get => _weaponIcon; private set => _weaponIcon = value; }
    public float damage { get => _weaponDamage; private set => _weaponDamage = value; }
    public float speed { get => _weaponSpeed; private set => _weaponSpeed = value; }
    public float cdDuration { get => _weaponCdDuration; private set => _weaponCdDuration = value; }
    public float lifetime { get => _weaponLifetime; private set => _weaponLifetime = value; }
    public int pierce { get => _weaponPierce; private set => _weaponPierce = value; }
    public int level { get => _weaponLevel; private set => _weaponLevel = value; }
    public GameObject nextLevelPrefab { get => _weaponNextLevelPrefab; private set => _weaponNextLevelPrefab = value; }
}
