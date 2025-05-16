using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Item item;
        public Image image;

        public void Assign(Item assignedItem)
        {
            item = assignedItem;
            if (item is Weapon weapon)
            {
                Weapon w = item as Weapon;
                image.enabled = true;
                image.sprite = w.WeaponData.Icon;
            }
            else
            {
                PassiveItem p = item as PassiveItem;
                image.enabled = true;
                image.sprite = p.PassiveItemData.Icon;
            }
        }

        public void Clear()
        {
            item = null;
            image.enabled = false;
            image.sprite = null;
        }

        public bool IsEmpty()
        {
            return item == null;
        }
    }

    [SerializeField] private List<Slot> weaponSlots = new List<Slot>(6);
    [SerializeField] private List<Slot> passiveItemSlots = new List<Slot>(6);

    public List<Slot> WeaponSlots { get => weaponSlots; }
    public List<Slot> PassiveItemSlots { get => passiveItemSlots; }
    public List<Image> WeaponImages
    {
        get
        {
            var list = new List<Image>();
            foreach (var Weapon in weaponSlots)
            {
                list.Add(Weapon.image);
            }
            return list;
        }
    }

    public List<Image> PassiveItemImages
    {
        get
        {
            var list = new List<Image>();
            foreach (var passiveItem in PassiveItemSlots)
            {
                list.Add(passiveItem.image);
            }
            return list;
        }
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    [Header("UI Elements")]
    [SerializeField] private List<WeaponDataScriptableObject> availableWeapons = new List<WeaponDataScriptableObject>();
    [SerializeField] private List<PassiveItemDataScriptableObject> availablePassives = new List<PassiveItemDataScriptableObject>();
    [SerializeField] private List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>(); //
    [SerializeField] private Sprite healSprite;
    PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public bool Has(ItemDataScriptableObject type) { return Get(type); }

    public Item Get(ItemDataScriptableObject type)
    {
        if (type is WeaponDataScriptableObject) return Get(type as WeaponDataScriptableObject);
        else if (type is PassiveItemDataScriptableObject) return Get(type as PassiveItemDataScriptableObject);
        return null;
    }

    public PassiveItem Get(PassiveItemDataScriptableObject type)
    {
        foreach (Slot s in passiveItemSlots)
        {
            PassiveItem p = s.item as PassiveItem;
            if (p == null) return null;
            if (p.PassiveItemData == type)
            {
                return p;
            }
        }
        return null;
    }

    public Weapon Get(WeaponDataScriptableObject type)
    {
        foreach (Slot s in weaponSlots)
        {
            Weapon w = s.item as Weapon;
            if (w == null) return null;
            if (w.WeaponData == type)
            {
                return w;
            }
        }
        return null;
    }

    public bool Remove(WeaponDataScriptableObject data, bool removeUpgradeAvailability = false)
    {
        if (removeUpgradeAvailability) availableWeapons.Remove(data);

        for (int i = 0; i < weaponSlots.Count; i++)
        {
            Weapon w = weaponSlots[i].item as Weapon;
            if (w.WeaponData == data)
            {
                weaponSlots[i].Clear();
                w.OnUnequip();
                Destroy(w.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool Remove(PassiveItemDataScriptableObject data, bool removeUpgradeAvailability = false)
    {
        if (removeUpgradeAvailability) availablePassives.Remove(data);

        for (int i = 0; i < weaponSlots.Count; i++)
        {
            PassiveItem p = weaponSlots[i].item as PassiveItem;
            if (p.PassiveItemData == data)
            {
                weaponSlots[i].Clear();
                p.OnUnequip();
                Destroy(p.gameObject);
                return true;
            }
        }

        return false;
    }

    public bool Remove(ItemDataScriptableObject data, bool removeUpgradeAvailability = false)
    {
        if (data is PassiveItemDataScriptableObject) return Remove(data as PassiveItemDataScriptableObject, removeUpgradeAvailability);
        else if (data is WeaponDataScriptableObject) return Remove(data as WeaponDataScriptableObject, removeUpgradeAvailability);
        return false;
    }

    public int Add(WeaponDataScriptableObject data)
    {
        int slotNum = -1;

        for (int i = 0; i < weaponSlots.Capacity; i++)
        {
            if (weaponSlots[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        if (slotNum < 0) return slotNum;
        Type weaponType = Type.GetType(data.Behaviour);

        if (weaponType != null)
        {
            GameObject go = new GameObject(data.BaseStats.name + " Controller");
            Weapon spawnedWeapon = (Weapon)go.AddComponent(weaponType);
            spawnedWeapon.Initialise(data);
            spawnedWeapon.transform.SetParent(transform);
            spawnedWeapon.transform.localPosition = Vector2.zero;
            spawnedWeapon.OnEquip();
            weaponSlots[slotNum].Assign(spawnedWeapon);
            if (spawnedWeapon.CurrentLevel >= spawnedWeapon.WeaponData.MaxLevel) availableWeapons.Remove(spawnedWeapon.WeaponData);

            if (GameManager.instance != null)
            {
                GameManager.instance.ChangePlayerInventoryOnGameOverScreen(WeaponImages, PassiveItemImages);
                if (GameManager.instance.IsChoosingUpdate)
                {
                    GameManager.instance.EndLevelUp();
                }
            }
            return slotNum;
        }
        return -1;
    }

    public int Add(PassiveItemDataScriptableObject data)
    {
        int slotNum = -1;
        for (int i = 0; i < passiveItemSlots.Capacity; i++)
        {
            if (passiveItemSlots[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }
        if (slotNum < 0) return slotNum;
        GameObject go = new GameObject(data.BaseStats.name + " Passive");
        PassiveItem p = go.AddComponent<PassiveItem>();
        p.Initialise(data);
        p.transform.SetParent(transform);
        p.transform.localPosition = Vector2.zero;
        passiveItemSlots[slotNum].Assign(p);
        if (p.CurrentLevel >= p.PassiveItemData.MaxLevel) availablePassives.Remove(p.PassiveItemData);

        if (GameManager.instance != null)
        {
            GameManager.instance.ChangePlayerInventoryOnGameOverScreen(WeaponImages, PassiveItemImages);
            if (GameManager.instance.IsChoosingUpdate)
            {
                GameManager.instance.EndLevelUp();
            }
        }
        playerStats.RecalculateStats();

        return slotNum;
    }

    public int Add(ItemDataScriptableObject data)
    {
        if (data is WeaponDataScriptableObject) return Add(data as WeaponDataScriptableObject);
        else if (data is PassiveItemDataScriptableObject) return Add(data as PassiveItemDataScriptableObject);
        return -1;
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            Weapon weapon = weaponSlots[slotIndex].item as Weapon;

            if (!weapon.DoLevelUp()) return;
            if (weapon.CurrentLevel >= weapon.WeaponData.MaxLevel) availableWeapons.Remove(weapon.WeaponData);

            if (GameManager.instance != null && GameManager.instance.IsChoosingUpdate)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem p = passiveItemSlots[slotIndex].item as PassiveItem;

            if (!p.DoLevelUp()) return;
            if (p.CurrentLevel >= p.PassiveItemData.MaxLevel) availablePassives.Remove(p.PassiveItemData);

            if (GameManager.instance != null && GameManager.instance.IsChoosingUpdate)
            {
                GameManager.instance.EndLevelUp();
            }
            playerStats.RecalculateStats();
        }
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponDataScriptableObject> availableWeaponUpgrades = new List<WeaponDataScriptableObject>(availableWeapons);
        List<PassiveItemDataScriptableObject> availablePassiveItemUpgrades = new List<PassiveItemDataScriptableObject>(availablePassives);
        if (availableWeapons.Count == 0 && availablePassives.Count == 0)
        {
            EnableUpgradeUI(upgradeUIOptions[0]);
            upgradeUIOptions[0].upgradeButton.onClick.AddListener(() => RestorePlayerHealth(100f));
            upgradeUIOptions[0].upgradeDescriptionDisplay.text = "Restore 100 health";
            upgradeUIOptions[0].upgradeNameDisplay.text = "Heart vitamins";
            upgradeUIOptions[0].upgradeIcon.sprite = healSprite;
            return;
        }
        foreach (UpgradeUI upgradeOption in upgradeUIOptions)
        {
            int upgradeType;
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
                return;
            else if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = UnityEngine.Random.Range(1, 3);
            }

            if (upgradeType == 1)
            {
                WeaponDataScriptableObject chosenWeaponUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);
                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool isLevelUp = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        Weapon w = weaponSlots[i].item as Weapon;
                        if (w != null && w.WeaponData == chosenWeaponUpgrade)
                        {
                            if (chosenWeaponUpgrade.MaxLevel <= w.CurrentLevel)
                            {
                                isLevelUp = false;
                                break;
                            }

                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));
                            Weapon.Stats nextLevel = chosenWeaponUpgrade.GetLevelUpData(w.CurrentLevel + 1);
                            upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.Icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenWeaponUpgrade));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.BaseStats.description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.BaseStats.name;
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.Icon;
                    }
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemDataScriptableObject chosenPassiveUpgrade = availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveUpgrade);

                if (chosenPassiveUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool isLevelUp = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        PassiveItem p = passiveItemSlots[i].item as PassiveItem;
                        if (p != null && p.PassiveItemData == chosenPassiveUpgrade)
                        {
                            if (chosenPassiveUpgrade.MaxLevel <= p.CurrentLevel)
                            {
                                isLevelUp = false;
                                break;
                            }
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, i));
                            PassiveItem.Modifier nextLevel = chosenPassiveUpgrade.GetLevelData(p.CurrentLevel + 1);
                            upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.Icon;
                            isLevelUp = true;
                            break;
                        }
                    }
                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenPassiveUpgrade));
                        PassiveItem.Modifier nextLevel = chosenPassiveUpgrade.BaseStats;
                        upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                        upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                        upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.Icon;
                    }
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (UpgradeUI upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    void RestorePlayerHealth(float health)
    {
        playerStats.RestoreHealth(100f);
        if (GameManager.instance != null && GameManager.instance.IsChoosingUpdate)
        {
            GameManager.instance.EndLevelUp();
        }
    }
}