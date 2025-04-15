using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //character stats
    private PlayerScriptableObject _playerStats;
    //current stats
    private float currentMaxHealth;
    private float currentHealthRecovery;
    private float currentMoveSpeed;
    private float currentMight;
    private float currentProjectileSpeed;
    private float currentHealth;
    private float currentMagnet;

    #region properties
    public float CurrentMaxHealth
    {
        get => currentMaxHealth;
        set
        {
            currentMaxHealth = value;
        }
    }
    public float CurrentHealthRecovery
    {
        get => currentHealthRecovery;
        set
        {
            currentHealthRecovery = value;
            if (GameManager.instance != null)
                GameManager.instance.currentRecoveryText = "HealtRecovery: " + currentHealthRecovery;
        }
    }
    public float CurrentMoveSpeed
    {
        get => currentMoveSpeed;
        set
        {
            currentMoveSpeed = value;
            if (GameManager.instance != null)
                GameManager.instance.currentMoveSpeedText = "MoveSpeed: " + currentMoveSpeed;
        }
    }
    public float CurrentMight
    {
        get => currentMight;
        set
        {
            currentMight = value;
            if (GameManager.instance != null)
                GameManager.instance.currentMightText = "Might: " + currentMight;
        }
    }
    public float CurrentProjectileSpeed
    {
        get => currentProjectileSpeed;
        set
        {
            currentProjectileSpeed = value;
            if (GameManager.instance != null)
                GameManager.instance.currentProjectileSpeedText = "ProjectileSpeed: " + currentProjectileSpeed;
        }
    }
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (GameManager.instance != null)
            {
                GameManager.instance.currentHealthText = "Health: " + currentHealth;
                GameManager.instance.HealthBarFillAmount = currentHealth / currentMaxHealth;
            }
        }
    }
    public float CurrentMagnet
    {
        get => currentMagnet;
        set
        {
            currentMagnet = value;
            if (GameManager.instance != null)
                GameManager.instance.currentMagnetText = "Magnet: " + currentMagnet;
        }
    }

    public int CurrentExpirience
    {
        get => currentExpirience;
        set
        {
            currentExpirience = value;
            if (GameManager.instance != null)
                GameManager.instance.ExpBarFillAmount = (float)value / currentExpirienceCap;
        }
    }

    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
            GameManager.instance.ExpBarText = "LVL." + currentLevel;
        }
    }
    #endregion

    //expirience    
    private int currentExpirience = 0;
    private int currentLevel = 1;
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

    //weapons and passiveItems
    private InvetntoryManager inventory;
    private int weaponIndex = 0;
    private int passiveItemIndex = 0;
    [SerializeField] private PlayerScriptableObject _defaultPlayerStats;
    [SerializeField] private ParticleSystem damageEffect;
    void Awake()
    {
        if (CharacterSelector.instance)
        {
            _playerStats = CharacterSelector.GetData();
            CharacterSelector.instance.DestroySingleton();
        }
        else
        {
            _playerStats = _defaultPlayerStats;
        }
        inventory = GetComponent<InvetntoryManager>();

        CurrentMaxHealth = _playerStats.maxHealth;
        CurrentHealthRecovery = _playerStats.healthRecovery;
        CurrentMoveSpeed = _playerStats.moveSpeed;
        CurrentMight = _playerStats.might;
        CurrentProjectileSpeed = _playerStats.projectileSpeed;
        CurrentHealth = _playerStats.maxHealth;
        CurrentMagnet = _playerStats.magnet;

        CreateWeaponController(_playerStats.startingWeapon);
        GameManager.instance.ChangePlayerUIOnGameOverScreen(_playerStats);
        GameManager.instance.ChangePlayerInventoryOnGameOverScreen(inventory.weaponUI, inventory.itemUI);
    }

    void Start()
    {
        CurrentExpirience = 0;
        CurrentLevel = 1;
        GameManager.instance.ChangePlayerLevelOnGameOverScreen(CurrentLevel);
        currentExpirienceCap += levelRanges[0].expirienceCapIncrease;
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
        CurrentExpirience += amount;
        LevelUpcheker();
    }

    void LevelUpcheker()
    {
        if (CurrentExpirience >= currentExpirienceCap)
        {
            CurrentLevel++;
            GameManager.instance.ChangePlayerLevelOnGameOverScreen(CurrentLevel);
            CurrentExpirience -= currentExpirienceCap;

            var expirienceCapIncrease = 0;
            foreach (LevelRange levelRange in levelRanges)
                if (CurrentLevel >= levelRange.startLevel && CurrentLevel <= levelRange.endLevel)
                {
                    expirienceCapIncrease = levelRange.expirienceCapIncrease;
                    break;
                }
            currentExpirienceCap += expirienceCapIncrease;

            GameManager.instance.LevelUp();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            if (damageEffect) Instantiate(damageEffect, transform.position, Quaternion.identity);

            invincibilityTimer = _invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        if (GameManager.instance != null)
            if (!GameManager.instance.IsGameOver)
            {
                GameManager.instance.ChangePlayerInventoryOnGameOverScreen(inventory.weaponUI, inventory.itemUI);
                GameManager.instance.GameOver();
            }
    }

    public void Heal(float heals)
    {
        if (CurrentHealth + heals > CurrentMaxHealth)
            CurrentHealth = CurrentMaxHealth;
        else
            CurrentHealth += heals;
    }

    void HealsRocover()
    {
        if (CurrentHealth + CurrentHealthRecovery * Time.deltaTime > CurrentMaxHealth)
            CurrentHealth = CurrentMaxHealth;
        else if (CurrentHealth + CurrentHealthRecovery * Time.deltaTime != CurrentMaxHealth)
            CurrentHealth += CurrentHealthRecovery * Time.deltaTime;
    }

    public void CreateWeaponController(GameObject weaponController)
    {
        if (weaponIndex >= inventory.weapons.Capacity - 1)
        {
            Debug.LogError("weaponsInventory Overflow!");
            return;
        }
        var controller = Instantiate(weaponController, transform.position, Quaternion.identity);
        controller.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, controller.GetComponent<WeaponController>());
        weaponIndex++;
    }

    public void CreatePassiveItemController(GameObject passiveItemController)
    {
        if (passiveItemIndex >= inventory.passiveItem.Capacity - 1)
        {
            Debug.LogError("PassiveItemsInventory Overflow!");
            return;
        }
        var controller = Instantiate(passiveItemController, transform.position, Quaternion.identity);
        controller.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, controller.GetComponent<PassiveItem>());
        passiveItemIndex++;
    }
}