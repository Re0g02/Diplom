using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvetntoryManager : MonoBehaviour
{
    [SerializeField] private List<Image> _weaponUISlots = new List<Image>(6);
    [SerializeField] private List<Image> _itemUISlots = new List<Image>(6);
    private List<WeaponController> _weaponsInventory = new List<WeaponController>(6);
    private int[] _weaponLevels = new int[6];
    private List<PassiveItem> _passiveItemsInventory = new List<PassiveItem>(6);
    private int[] _passiveItemLevels = new int[6];
    public List<WeaponController> weapons { get => _weaponsInventory; private set => _weaponsInventory = value; }
    public List<PassiveItem> passiveItem { get => _passiveItemsInventory; private set => _passiveItemsInventory = value; }
    public List<Image> weaponUI { get => _weaponUISlots; private set => _weaponUISlots = value; }
    public List<Image> itemUI { get => _itemUISlots; private set => _itemUISlots = value; }

    [System.Serializable]
    private class WeaponUpgrade
    {
        [HideInInspector] public int upgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponStats;
    }

    [System.Serializable]
    private class PassiveItemUpgrade
    {
        [HideInInspector] public int upgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemStats;
    }

    [System.Serializable]
    private class UpgradeUI
    {
        public TextMeshProUGUI upgradeNameDisplay;
        public TextMeshProUGUI upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    [SerializeField] private List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    [SerializeField] private List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    [SerializeField] private List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    private List<WeaponUpgrade> availableWeaponUpgradeOptions;
    private List<PassiveItemUpgrade> availablePassiveItemUpgradeOptions;

    [SerializeField] private List<WeaponEvolutionBlueprint> weaponEvolutions = new List<WeaponEvolutionBlueprint>();
    PlayerStats player;

    void Awake()
    {
        AssignOptionIndexes();
    }

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }
    public void AddWeapon(int inventorySlotNumber, WeaponController weapon)
    {
        if (_weaponsInventory.Count - 1 < inventorySlotNumber)
            _weaponsInventory.Add(weapon);
        else _weaponsInventory[inventorySlotNumber] = weapon;
        _weaponLevels[inventorySlotNumber] = weapon.stats.level;
        _weaponUISlots[inventorySlotNumber].enabled = true;
        _weaponUISlots[inventorySlotNumber].sprite = weapon.stats.icon;

        EndInventoryManipulation();
    }

    public void AddPassiveItem(int inventorySlotNumber, PassiveItem passiveItem)
    {
        if (_passiveItemsInventory.Count - 1 < inventorySlotNumber)
            _passiveItemsInventory.Add(passiveItem);
        else _passiveItemsInventory[inventorySlotNumber] = passiveItem;

        _passiveItemLevels[inventorySlotNumber] = passiveItem.stats.level;
        _itemUISlots[inventorySlotNumber].enabled = true;
        _itemUISlots[inventorySlotNumber].sprite = passiveItem.stats.icon;

        EndInventoryManipulation();
    }

    public void LevelUpWeapon(int inventorySlotNumber, int upgradeIndex)
    {
        if (_weaponsInventory.Count > inventorySlotNumber)
        {
            var weaponController = _weaponsInventory[inventorySlotNumber];
            if (!weaponController.stats.nextLevelPrefab)
            {
                Debug.LogError(weaponController.name + " has not next level!");
                return;
            }
            var nextLevelWeaponController = Instantiate(weaponController.stats.nextLevelPrefab, transform.position, quaternion.identity);

            nextLevelWeaponController.transform.SetParent(transform);
            AddWeapon(inventorySlotNumber, nextLevelWeaponController.GetComponent<WeaponController>());
            Destroy(weaponController.gameObject);
            _weaponLevels[inventorySlotNumber] = nextLevelWeaponController.GetComponent<WeaponController>().stats.level;

            weaponUpgradeOptions[upgradeIndex].weaponStats = nextLevelWeaponController.GetComponent<WeaponController>().stats;
            EndInventoryManipulation();
        }
    }

    public void LevelUpPassiveItem(int inventorySlotNumber, int upgradeIndex)
    {
        if (_passiveItemsInventory.Count > inventorySlotNumber)
        {
            var itemController = _passiveItemsInventory[inventorySlotNumber];
            if (!itemController.stats.nextLevelPrefab)
            {
                Debug.LogError(itemController.name + " has not next level!");
                return;
            }
            var nextLevelItemController = Instantiate(itemController.stats.nextLevelPrefab, transform.position, quaternion.identity);

            nextLevelItemController.transform.SetParent(transform);
            AddPassiveItem(inventorySlotNumber, nextLevelItemController.GetComponent<PassiveItem>());
            Destroy(itemController.gameObject);
            _passiveItemLevels[inventorySlotNumber] = nextLevelItemController.GetComponent<PassiveItem>().stats.level;

            passiveItemUpgradeOptions[upgradeIndex].passiveItemStats = nextLevelItemController.GetComponent<PassiveItem>().stats;
            EndInventoryManipulation();
        }
    }

    void ApplyUpgradeOptions()
    {
        availableWeaponUpgradeOptions = new List<WeaponUpgrade>(weaponUpgradeOptions);
        availablePassiveItemUpgradeOptions = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach (var upgradeOption in upgradeUIOptions)
        {
            int upgradeType = 0;

            if (availablePassiveItemUpgradeOptions.Count == 0 && availableWeaponUpgradeOptions.Count == 0) return;
            if (availablePassiveItemUpgradeOptions.Count == 0)
                upgradeType = 1;
            else if (availableWeaponUpgradeOptions.Count == 0)
                upgradeType = 2;
            else upgradeType = UnityEngine.Random.Range(1, 3);

            if (upgradeType == 1)
            {
                WeaponUpgradeOption(upgradeOption);
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgradeOption(upgradeOption);
            }
        }
    }

    private void WeaponUpgradeOption(UpgradeUI upgradeOption)
    {
        bool isNewWeapon = true;
        var weaponUpgrade = availableWeaponUpgradeOptions[UnityEngine.Random.Range(0, availableWeaponUpgradeOptions.Count)];
        availableWeaponUpgradeOptions.Remove(weaponUpgrade);

        EnableUpgradeUI(upgradeOption);
        for (int i = 0; i < _weaponsInventory.Count; i++)
        {
            if (_weaponsInventory[i] != null && _weaponsInventory[i].stats == weaponUpgrade.weaponStats)
            {
                isNewWeapon = false;
                if (!weaponUpgrade.weaponStats.nextLevelPrefab)
                {
                    DisableUpgradeUI(upgradeOption);
                    break;
                }
                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, weaponUpgrade.upgradeIndex));
                upgradeOption.upgradeIcon.sprite = weaponUpgrade.weaponStats.nextLevelPrefab.GetComponent<WeaponController>().stats.icon;
                upgradeOption.upgradeDescriptionDisplay.text = weaponUpgrade.weaponStats.nextLevelPrefab.GetComponent<WeaponController>().stats.description;
                upgradeOption.upgradeNameDisplay.text = weaponUpgrade.weaponStats.nextLevelPrefab.GetComponent<WeaponController>().stats.weaponName;
                break;
            }
        }
        if (isNewWeapon)
        {
            upgradeOption.upgradeButton.onClick.AddListener(() => player.CreateWeaponController(weaponUpgrade.initialWeapon));
            upgradeOption.upgradeIcon.sprite = weaponUpgrade.weaponStats.nextLevelPrefab.GetComponent<WeaponController>().stats.icon;
            upgradeOption.upgradeDescriptionDisplay.text = weaponUpgrade.weaponStats.description;
            upgradeOption.upgradeNameDisplay.text = weaponUpgrade.weaponStats.weaponName;
        }
    }

    private void PassiveItemUpgradeOption(UpgradeUI upgradeOption)
    {
        bool isNewPassiveItem = true;
        var passiveItemUpgrade = availablePassiveItemUpgradeOptions[UnityEngine.Random.Range(0, availablePassiveItemUpgradeOptions.Count)];
        availablePassiveItemUpgradeOptions.Remove(passiveItemUpgrade);
        EnableUpgradeUI(upgradeOption);
        for (int i = 0; i < _passiveItemsInventory.Count; i++)
        {
            if (_passiveItemsInventory[i] != null && _passiveItemsInventory[i].stats == passiveItemUpgrade.passiveItemStats)
            {
                isNewPassiveItem = false;
                if (!passiveItemUpgrade.passiveItemStats.nextLevelPrefab)
                {
                    DisableUpgradeUI(upgradeOption);
                    break;
                }
                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, passiveItemUpgrade.upgradeIndex));
                upgradeOption.upgradeIcon.sprite = passiveItemUpgrade.passiveItemStats.nextLevelPrefab.GetComponent<PassiveItem>().stats.icon;
                upgradeOption.upgradeDescriptionDisplay.text = passiveItemUpgrade.passiveItemStats.nextLevelPrefab.GetComponent<PassiveItem>().stats.description;
                upgradeOption.upgradeNameDisplay.text = passiveItemUpgrade.passiveItemStats.nextLevelPrefab.GetComponent<PassiveItem>().stats.itemName;
                break;
            }
        }
        if (isNewPassiveItem)
        {
            upgradeOption.upgradeButton.onClick.AddListener(() => player.CreatePassiveItemController(passiveItemUpgrade.initialPassiveItem));
            upgradeOption.upgradeIcon.sprite = passiveItemUpgrade.passiveItemStats.nextLevelPrefab.GetComponent<PassiveItem>().stats.icon;
            upgradeOption.upgradeDescriptionDisplay.text = passiveItemUpgrade.passiveItemStats.description;
            upgradeOption.upgradeNameDisplay.text = passiveItemUpgrade.passiveItemStats.itemName;
        }
    }

    private void AssignOptionIndexes()
    {
        int weaponIndex = 0;
        int passiveItemIndex = 0;
        foreach (var weaponUpgrade in weaponUpgradeOptions)
        {
            weaponUpgrade.upgradeIndex = weaponIndex;
            weaponIndex++;
        }
        foreach (var passiveItem in passiveItemUpgradeOptions)
        {
            passiveItem.upgradeIndex = passiveItemIndex;
            passiveItemIndex++;
        }
    }

    private void RemoveUpdateOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    private void EndInventoryManipulation()
    {
        if (GameManager.instance != null && GameManager.instance.IsChoosingUpdate)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void RemoveAndApplyLevelUp()
    {
        RemoveUpdateOptions();
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

    public List<WeaponEvolutionBlueprint> GetPossibleEvolutions()
    {
        List<WeaponEvolutionBlueprint> possibleEvolutions = new List<WeaponEvolutionBlueprint>();
        foreach (var weapon in _weaponsInventory)
            if (weapon != null)
                foreach (var evolvedWeapon in weaponEvolutions)
                    if (weapon.stats.weaponName.Contains(evolvedWeapon.BaseWeaponStats.weaponName) && weapon.stats.level >= evolvedWeapon.WeaponRequiredLevel)
                        foreach (var passiveItem in _passiveItemsInventory)
                            if (passiveItem != null)
                                if (passiveItem.stats.itemName.Contains(evolvedWeapon.BasePassiveItemStats.itemName) && passiveItem.stats.level >= evolvedWeapon.PassiveItemRequiredLevel)
                                {
                                    possibleEvolutions.Add(evolvedWeapon);
                                    break;
                                }
        return possibleEvolutions;
    }

    public void EvolveWeapon(WeaponEvolutionBlueprint evolution)
    {
        for (int weaponIndex = 0; weaponIndex < _weaponsInventory.Count; weaponIndex++)
        {
            var weapon = _weaponsInventory[weaponIndex];

            if (weapon == null) continue;
            if (weapon.stats.weaponName.Contains(evolution.BaseWeaponStats.weaponName))
            {
                var evolvedWeapon = Instantiate(evolution.EvolvedWeaponPrefab, transform.position, Quaternion.identity);
                var evolvedWeaponController = evolvedWeapon.GetComponent<WeaponController>();

                evolvedWeapon.transform.SetParent(transform);
                AddWeapon(weaponIndex, evolvedWeaponController);
                Destroy(weapon.gameObject);
                _weaponLevels[weaponIndex] = evolvedWeaponController.stats.level;
                _weaponUISlots[weaponIndex].sprite = evolvedWeaponController.stats.icon;
                foreach (var weaponUpgrade in weaponUpgradeOptions)
                {
                    if (weaponUpgrade.weaponStats.weaponName.Contains(evolution.BaseWeaponStats.weaponName))
                    {
                        weaponUpgradeOptions.Remove(weaponUpgrade);
                        AssignOptionIndexes();
                        break;
                    }
                }
                return;
            }
        }
    }
}