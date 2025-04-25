using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerDataScriptableObject playerData;
    private PlayerDataScriptableObject.Stats baseStats;
    private PlayerDataScriptableObject.Stats actualStats;
    private float health;

    #region Stats Properties
    public float CurrentHealth
    {
        get => health;
        set
        {
            if (health != value)
            {
                health = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthText = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                    );
                    GameManager.instance.HealthBarFillAmount = (float)value / actualStats.maxHealth;
                }
            }
        }
    }

    public float MaxHealth
    {
        get => actualStats.maxHealth;
        set
        {
            if (actualStats.maxHealth != value)
            {
                actualStats.maxHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthText = string.Format(
                                                                            "Healt: {0} / {1}",
                                                                            health, actualStats.maxHealth);
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get => Recovery;
        set => Recovery = value;
    }

    public float Recovery
    {
        get => actualStats.recovery;
        set
        {
            if (actualStats.recovery != value)
            {
                actualStats.recovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryText = "Recovery: " + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get => MoveSpeed;
        set => MoveSpeed = value;
    }

    public float MoveSpeed
    {
        get => actualStats.moveSpeed;
        set
        {
            if (actualStats.moveSpeed != value)
            {
                actualStats.moveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryText = "MoveSpeed: " + actualStats.moveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get => Might;
        set => Might = value;
    }

    public float Might
    {
        get => actualStats.might;
        set
        {
            if (actualStats.might != value)
            {
                actualStats.might = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightText = "Might: " + actualStats.might;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get => Speed;
        set => Speed = value;
    }

    public float Speed
    {
        get => actualStats.speed;
        set
        {
            if (actualStats.speed != value)
            {
                actualStats.speed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedText = "Speed: " + actualStats.speed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get => Magnet;
        set => Magnet = value;
    }

    public float Magnet
    {
        get => actualStats.magnet;
        set
        {
            if (actualStats.magnet != value)
            {
                actualStats.magnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetText = "Magnet: " + actualStats.recovery;
                }
            }
        }
    }

    public int Experience
    {
        get => experience;
        set
        {
            if (experience != value)
            {
                experience = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.ExpBarFillAmount = (float)value / experienceCap;
                }
            }
        }
    }

    public int Level
    {
        get => level;
        set
        {
            if (level != value)
            {
                level = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.ExpBarText = "lvl. " + value;
                }
            }
        }
    }
    #endregion

    [SerializeField]
    private ParticleSystem damageEffect;

    [SerializeField]
    private int experience = 0;
    [SerializeField] private int level = 0;
    [SerializeField] private int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [SerializeField] private float invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;

    [SerializeField] private List<LevelRange> levelRanges;

    private PlayerInventory inventory;
    [SerializeField] private int weaponIndex;
    [SerializeField] private int passiveItemIndex;

    void Awake()
    {
        playerData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
        inventory = GetComponent<PlayerInventory>();
        baseStats = actualStats = playerData.playerStats;
        CurrentHealth = actualStats.maxHealth;
    }

    void Start()
    {
        inventory.Add(playerData.StartingWeapon);
        GameManager.instance.currentHealthText = "Health: " + CurrentHealth;
        GameManager.instance.currentRecoveryText = "Recovery: " + CurrentRecovery;
        GameManager.instance.currentMoveSpeedText = "Move Speed: " + CurrentMoveSpeed;
        GameManager.instance.currentMightText = "Might: " + CurrentMight;
        GameManager.instance.currentProjectileSpeedText = "Projectile Speed: " + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetText = "Magnet: " + CurrentMagnet;

        GameManager.instance.ChangePlayerUIOnGameOverScreen(playerData);
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.PassiveItemSlots)
        {
            PassiveItem p = s.item as PassiveItem;
            if (p)
            {
                actualStats += p.GetBoosts();
            }
        }
    }

    public void IncreaseExperience(int amount)
    {
        Experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (Experience >= experienceCap)
        {
            Level++;
            Experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (Level >= range.startLevel && Level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            GameManager.instance.LevelUp();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;
            if (damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        if (!GameManager.instance.IsGameOver)
        {
            GameManager.instance.ChangePlayerLevelOnGameOverScreen(Level);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }

    void Recover()
    {
        if (CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            CurrentHealth += Recovery * Time.deltaTime;

            if (CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }

    public void CreateWeaponController(GameObject weaponController)
    {
        if (weaponIndex >= inventory.WeaponSlots.Count - 1)
        {
            Debug.LogError("weaponsInventory Overflow!");
            return;
        }
        var controller = Instantiate(weaponController, transform.position, Quaternion.identity);
        controller.transform.SetParent(transform);

        weaponIndex++;
    }

    public void CreatePassiveItemController(GameObject passiveItemController)
    {
        if (passiveItemIndex >= inventory.PassiveItemSlots.Count - 1)
        {
            Debug.LogError("PassiveItemsInventory Overflow!");
            return;
        }
        var controller = Instantiate(passiveItemController, transform.position, Quaternion.identity);
        controller.transform.SetParent(transform);

        passiveItemIndex++;
    }
}