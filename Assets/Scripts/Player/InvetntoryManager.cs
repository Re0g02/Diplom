using System.Collections.Generic;
using Unity.Mathematics;
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
    public void AddWeapon(int inventorySlotNumber, WeaponController weapon)
    {
        if (_weaponsInventory.Count - 1 < inventorySlotNumber)
            _weaponsInventory.Add(weapon);
        else _weaponsInventory[inventorySlotNumber] = weapon;

        _weaponLevels[inventorySlotNumber] = weapon.stats.level;
        _weaponUISlots[inventorySlotNumber].enabled = true;
        _weaponUISlots[inventorySlotNumber].sprite = weapon.stats.icon;
    }

    public void AddPassiveItem(int inventorySlotNumber, PassiveItem passiveItem)
    {
        if (_passiveItemsInventory.Count - 1 < inventorySlotNumber)
            _passiveItemsInventory.Add(passiveItem);
        else _passiveItemsInventory[inventorySlotNumber] = passiveItem;

        _passiveItemLevels[inventorySlotNumber] = passiveItem.stats.level;
        _itemUISlots[inventorySlotNumber].enabled = true;
        _itemUISlots[inventorySlotNumber].sprite = passiveItem.stats.icon;
    }

    public void LevelUpWeapon(int inventorySlotNumber)
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
        }
    }

    public void LevelUpPassiveItem(int inventorySlotNumber)
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
    }
}
