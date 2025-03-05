using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject _playerStartingWeapon;
    [SerializeField] private float _playerMaxHealth;
    [SerializeField] private float _playerHealthRecovery;
    [SerializeField] private float _playerMoveSpeed;
    [SerializeField] private float _playerMight;
    [SerializeField] private float _playerProjectileSpeed;
    [SerializeField] private float _playerMagnet;

    public GameObject startingWeapon { get => _playerStartingWeapon; private set => _playerStartingWeapon = value; }
    public float maxHealth { get => _playerMaxHealth; private set => _playerMaxHealth = value; }
    public float healthRecovery { get => _playerHealthRecovery; private set => _playerHealthRecovery = value; }
    public float moveSpeed { get => _playerMoveSpeed; private set => _playerMoveSpeed = value; }
    public float might { get => _playerMight; private set => _playerMight = value; }
    public float projectileSpeed { get => _playerProjectileSpeed; private set => _playerProjectileSpeed = value; }
    public float magnet { get => _playerMagnet; private set => _playerMagnet = value; }
}
