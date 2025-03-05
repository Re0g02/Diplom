using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //character stats
    private PlayerScriptableObject _playerStats;
    //current stats
    [HideInInspector] public float currentMaxHealth;
    [HideInInspector] public float currentHealthRecovery;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentMight;
    [HideInInspector] public float currentProjectileSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMagnet;
    //expirience
    [SerializeField] private int currentExpirience = 0;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExpirienceCap;
    [SerializeField] private List<LevelRange> levelRanges;
    [System.Serializable]
    private class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int expirienceCapIncrease;
    }
    //invincibility
    [SerializeField] private float _invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;
    //weapons
    [SerializeField] private List<GameObject> spawnedWeapons;

    void Awake()
    {
        _playerStats = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        currentMaxHealth = _playerStats.maxHealth;
        currentHealthRecovery = _playerStats.healthRecovery;
        currentMoveSpeed = _playerStats.moveSpeed;
        currentMight = _playerStats.might;
        currentProjectileSpeed = _playerStats.projectileSpeed;
        currentHealth = _playerStats.maxHealth;
        currentMagnet = _playerStats.magnet;

        CreateWeaponController(_playerStats.startingWeapon);
    }

    void Start()
    {
        currentExpirienceCap = levelRanges[0].expirienceCapIncrease;
    }
    void Update()
    {
        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;
        else if (isInvincible)
            isInvincible = false;
        HealsRocover();
    }
    public void IncreaseExperience(int amount)
    {
        currentExpirience += amount;
        LevelUpcheker();
    }

    void LevelUpcheker()
    {
        if (currentExpirience >= currentExpirienceCap)
        {
            currentLevel++;
            currentExpirience -= currentExpirienceCap;

            var expirienceCapIncrease = 0;
            foreach (LevelRange levelRange in levelRanges)
                if (currentLevel >= levelRange.startLevel && currentLevel <= levelRange.endLevel)
                {
                    expirienceCapIncrease = levelRange.expirienceCapIncrease;
                    break;
                }
            currentExpirienceCap += expirienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;
            invincibilityTimer = _invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("Player Died!");
    }

    public void Heal(float heals)
    {
        if (currentHealth + heals > currentMaxHealth)
            currentHealth = currentMaxHealth;
        else
            currentHealth += heals;
    }

    void HealsRocover()
    {
        if (currentHealth + currentHealthRecovery * Time.deltaTime > currentMaxHealth)
            currentHealth = currentMaxHealth;
        else if (currentHealth + currentHealthRecovery * Time.deltaTime != currentMaxHealth)
            currentHealth += currentHealthRecovery * Time.deltaTime;
    }

    private void CreateWeaponController(GameObject weaponController)
    {
        var controller = Instantiate(weaponController, transform.position, Quaternion.identity);
        controller.transform.SetParent(transform);
        spawnedWeapons.Add(controller);
    }
}
