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
                GameManager.instance.currentHealthText = "Health: " + currentHealth;
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
    #endregion

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

    //weapons and passiveItems
    private InvetntoryManager inventory;
    private int weaponIndex = 0;
    private int passiveItemIndex = 0;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    void Awake()
    {
        _playerStats = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
        inventory = GetComponent<InvetntoryManager>();

        CurrentMaxHealth = _playerStats.maxHealth;
        CurrentHealthRecovery = _playerStats.healthRecovery;
        CurrentMoveSpeed = _playerStats.moveSpeed;
        CurrentMight = _playerStats.might;
        CurrentProjectileSpeed = _playerStats.projectileSpeed;
        CurrentHealth = _playerStats.maxHealth;
        CurrentMagnet = _playerStats.magnet;

        CreateWeaponController(_playerStats.startingWeapon);
        CreatePassiveItemController(item1);
        GameManager.instance.ChangePlayerUIOnGameOverScreen(_playerStats);
        GameManager.instance.ChangePlayerInventoryOnGameOverScreen(inventory.weaponUI, inventory.itemUI);
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
            GameManager.instance.ChangePlayerLevelOnGameOverScreen(currentLevel);
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
            CurrentHealth -= dmg;
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

    private void CreateWeaponController(GameObject weaponController)
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

    private void CreatePassiveItemController(GameObject passiveItemController)
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